# Authentication Implementation Plan

This document provides an ordered implementation plan for the ideal authentication flow, organized into phases with specific tasks.

---

## Overview

### Goals
1. Backend determines new vs returning user (not client localStorage)
2. Single idempotent `SyncUser` mutation replaces separate register/query
3. Explicit auth state machine replaces ad-hoc flags
4. Remove token subscription complexity
5. No PII in localStorage
6. Fast, informative user experience with appropriate welcome messages

### Dependencies
```
Phase 1 (Backend) ─────────────────────────────────────────────┐
                                                               │
Phase 2 (Auth State) ──────────────────────────────────────────┼──► Phase 5 (Callback Flow)
                                                               │
Phase 3 (Token Simplification) ────────────────────────────────┤
                                                               │
Phase 4 (GraphQL Frontend) ────────────────────────────────────┘

Phase 5 (Callback Flow) ──────► Phase 6 (UX Polish) ──────► Phase 7 (Cleanup)
```

---

## Phase 1: Backend - User Sync Foundation

**Goal:** Backend returns `isFirstLogin` flag and tracks login timestamps.

**Why First:** Frontend cannot show correct welcome messages without this data from the backend.

### Task 1.1: Add Timestamp Fields to User Entity

**File:** `src/Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/UserInfoExtEntity.cs`

**Changes:**
```csharp
// Add to existing class
[JsonProperty("created_at")]
public DateTime CreatedAt { get; init; }

[JsonProperty("last_login_at")]
public DateTime LastLoginAt { get; init; }
```

**Acceptance:**
- [ ] Entity compiles with new properties
- [ ] Existing UPSERT operations still work (new fields optional initially)

---

### Task 1.2: Update Scribe to Handle Timestamps

**File:** `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Scribes/UserInfoScribe.cs`

**Changes:**
- Create new method or update existing to check if user exists before upsert
- If user doesn't exist: set `CreatedAt` and `LastLoginAt` to now
- If user exists: only update `LastLoginAt`

**Option A - Query First Pattern:**
```csharp
public async Task<(UserInfoExtEntity User, bool IsFirstLogin)> SyncUserAsync(UserInfoExtEntity input)
{
    var existing = await TryGetByIdAsync(input.UserId);
    var now = DateTime.UtcNow;

    var entity = input with
    {
        CreatedAt = existing?.CreatedAt ?? now,
        LastLoginAt = now
    };

    await UpsertAsync(entity);
    return (entity, existing is null);
}
```

**Acceptance:**
- [ ] New user: `CreatedAt` and `LastLoginAt` both set to current time
- [ ] Returning user: `CreatedAt` preserved, `LastLoginAt` updated
- [ ] Returns `isFirstLogin` boolean

---

### Task 1.3: Update Adapter Layer

**File:** `src/Lib.Adapter.User/Commands/RegisterUserAdapter.cs`

**Changes:**
- Call new `SyncUserAsync` method
- Return `isFirstLogin` in response

**New Response Type:**
```csharp
// File: src/Lib.Shared.DataModels/Entities/Oufs/IUserSyncOufEntity.cs
public interface IUserSyncOufEntity
{
    string UserId { get; }
    string DisplayName { get; }
    string Email { get; }
    DateTime CreatedAt { get; }
    DateTime LastLoginAt { get; }
    bool IsFirstLogin { get; }
}
```

**Acceptance:**
- [ ] Adapter returns complete user data including `IsFirstLogin`

---

### Task 1.4: Update Domain and Aggregator Layers

**Files:**
- `src/Lib.Domain.User/Commands/RegisterUserDomainService.cs`
- `src/Lib.Aggregator.User/Commands/RegisterUserAggregatorService.cs`

**Changes:**
- Pass through the new response type
- No business logic changes needed (pass-through)

**Acceptance:**
- [ ] Response flows correctly through layers

---

### Task 1.5: Update Entry Layer Response

**File:** `src/Lib.MtgDiscovery.Entry/Queries/User/RegisterUserEntryService.cs`

**Changes:**
- Map aggregator response to new out entity
- Include `isFirstLogin` in response

**Updated Response:**
```csharp
// File: src/Lib.MtgDiscovery.Entry/Entities/Outs/User/UserSyncOutEntity.cs
public sealed class UserSyncOutEntity
{
    public string UserId { get; init; }
    public string DisplayName { get; init; }
    public string Email { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastLoginAt { get; init; }
    public bool IsFirstLogin { get; init; }
}
```

**Acceptance:**
- [ ] Entry service returns complete sync response

---

### Task 1.6: Update GraphQL Response Types

**File:** `src/App.MtgDiscovery.GraphQL/Mutations/UserMutationMethods.cs`

**Changes:**
- Update response model to include new fields
- Consider renaming mutation to `SyncUser` (or keep `RegisterUserInfo` for compatibility)

**GraphQL Response:**
```graphql
type UserSyncSuccessResponse {
  data: UserSyncData!
  status: StatusInfo!
}

type UserSyncData {
  userId: ID!
  displayName: String!
  email: String
  createdAt: DateTime!
  lastLoginAt: DateTime!
  isFirstLogin: Boolean!
}
```

**Acceptance:**
- [ ] GraphQL schema updated
- [ ] Mutation returns `isFirstLogin`
- [ ] Existing tests pass (if any)

---

### Phase 1 Deliverable
Backend mutation that:
- Creates user on first call, updates `lastLoginAt` on subsequent calls
- Returns `isFirstLogin: true` for new users, `false` for returning
- Is fully idempotent (safe to call multiple times)

---

## Phase 2: Frontend - Auth State Machine

**Goal:** Replace ad-hoc auth flags with explicit state machine.

**Why Now:** Foundation for all frontend auth logic.

### Task 2.1: Create Auth Status Types

**New File:** `client/src/types/auth.ts`

```typescript
export type AuthStatus =
  | 'initializing'    // Auth0 SDK loading
  | 'unauthenticated' // No session
  | 'authenticating'  // OAuth flow in progress
  | 'syncing'         // Have token, calling backend
  | 'authenticated';  // Fully ready

export interface AuthState {
  status: AuthStatus;
  user: UserProfile | null;
  isFirstLogin: boolean;
  error: AuthError | null;
}

export interface AuthError {
  code: string;
  message: string;
}
```

**Acceptance:**
- [ ] Types defined and exported

---

### Task 2.2: Create Auth State Context

**New File:** `client/src/contexts/AuthStateContext.tsx`

```typescript
interface AuthStateContextValue extends AuthState {
  // Actions
  setAuthenticating: () => void;
  setSyncing: () => void;
  setAuthenticated: (user: UserProfile, isFirstLogin: boolean) => void;
  setError: (error: AuthError) => void;
  reset: () => void;
}
```

**Implementation:**
- Use `useReducer` for state management
- Derive `status` from Auth0 state and internal state
- Provide transition methods

**Acceptance:**
- [ ] Context created with proper TypeScript types
- [ ] State transitions work correctly
- [ ] Can be used alongside existing UserContext (no breaking changes)

---

### Task 2.3: Create useAuthState Hook

**New File:** `client/src/hooks/auth/useAuthState.ts`

```typescript
export const useAuthState = () => {
  const context = useContext(AuthStateContext);
  if (context === undefined) {
    throw new Error('useAuthState must be used within AuthStateProvider');
  }
  return context;
};
```

**Acceptance:**
- [ ] Hook provides type-safe access to auth state

---

### Task 2.4: Integrate Auth State Provider

**File:** `client/src/App.tsx`

**Changes:**
- Add `AuthStateProvider` to provider hierarchy
- Position after Auth0Provider, before UserProvider

```typescript
<Auth0Provider>
  <Auth0TokenProvider>
    <ApolloProvider>
      <AuthStateProvider>  {/* NEW */}
        <UserProvider>
          {/* ... */}
        </UserProvider>
      </AuthStateProvider>
    </ApolloProvider>
  </Auth0TokenProvider>
</Auth0Provider>
```

**Acceptance:**
- [ ] Provider hierarchy correct
- [ ] Existing functionality unaffected

---

### Phase 2 Deliverable
Auth state machine that:
- Tracks explicit states: initializing → unauthenticated/authenticated
- Provides `isFirstLogin` from backend (will be wired in Phase 5)
- Coexists with existing UserContext (no breaking changes yet)

---

## Phase 3: Frontend - Simplify Token Management

**Goal:** Remove subscription complexity from Apollo client.

**Why Now:** Cleaner codebase before adding new auth flow.

### Task 3.1: Remove Token Subscription System

**File:** `client/src/graphql/apollo-client.ts`

**Remove:**
```typescript
// DELETE lines 27-59 (approximately)
let isTokenReady = false;
const tokenReadySubscribers = new Set<(ready: boolean) => void>();

export const setTokenReadyState = (ready: boolean) => { ... };
export const getTokenReadyState = () => { ... };
export const subscribeToTokenReady = (callback) => { ... };
```

**Keep:**
```typescript
// KEEP - token getter registration
let getAuth0Token: (() => Promise<string | null>) | null = null;

export const setAuth0TokenGetter = (getter: () => Promise<string | null>) => {
  getAuth0Token = getter;
};
```

**Acceptance:**
- [ ] Subscription code removed
- [ ] `setAuth0TokenGetter` still works
- [ ] Auth link still attaches tokens to requests

---

### Task 3.2: Simplify Auth0TokenProvider

**File:** `client/src/components/auth/Auth0TokenProvider.tsx`

**Simplify to:**
```typescript
export const Auth0TokenProvider: React.FC<Props> = ({ children }) => {
  const { getAccessTokenSilently, isAuthenticated, isLoading, loginWithRedirect } = useAuth0();
  const registered = useRef(false);

  useEffect(() => {
    if (isLoading || !isAuthenticated || registered.current) {
      return;
    }

    const getToken = async (): Promise<string | null> => {
      try {
        return await getAccessTokenSilently({
          authorizationParams: { audience: 'api://mtg-discovery' }
        });
      } catch (error) {
        if (isLoginRequiredError(error)) {
          loginWithRedirect();
          return null;
        }
        console.error('Failed to get token:', error);
        return null;
      }
    };

    setAuth0TokenGetter(getToken);
    registered.current = true;
  }, [getAccessTokenSilently, isAuthenticated, isLoading, loginWithRedirect]);

  return <>{children}</>;
};
```

**Acceptance:**
- [ ] No more `setTokenReadyState` calls
- [ ] Token getter registered once when authenticated
- [ ] Login redirect on token failure still works

---

### Task 3.3: Remove Subscription Usage from useUserSync

**File:** `client/src/hooks/user/useUserSync.ts`

**Remove:**
```typescript
// DELETE subscription logic (lines ~61-70)
useEffect(() => {
  if (isAuthenticated && auth0Loading === false) {
    const unsubscribe = subscribeToTokenReady((ready) => {
      setTokenReady(ready);
    });
    return unsubscribe;
  }
}, [isAuthenticated, auth0Loading]);
```

**Replace with:**
```typescript
// Token is ready when Auth0 is authenticated and not loading
const tokenReady = isAuthenticated && !isLoading;
```

**Acceptance:**
- [ ] No more subscription usage
- [ ] User sync still works correctly
- [ ] Query executes when authenticated

---

### Phase 3 Deliverable
Simplified token management:
- No global subscription system
- Token getter registered directly
- Components check Auth0 state directly instead of subscribing

---

## Phase 4: Frontend - GraphQL Updates

**Goal:** Create frontend mutation that matches new backend response.

**Why Now:** Needed before building new callback page.

### Task 4.1: Create SyncUser Mutation

**File:** `client/src/graphql/mutations/user.ts`

**Add:**
```typescript
export const SYNC_USER = gql`
  mutation SyncUser {
    registerUserInfo {
      __typename
      ... on UserRegistrationSuccessResponse {
        data {
          userId
          displayName
          email
          createdAt
          lastLoginAt
          isFirstLogin
        }
        status {
          message
          statusCode
        }
      }
      ... on FailureResponse {
        status {
          message
          statusCode
        }
      }
    }
  }
`;
```

**Note:** Uses existing `registerUserInfo` mutation name for backend compatibility. Can be aliased if needed.

**Acceptance:**
- [ ] Mutation defined
- [ ] TypeScript types generated (run codegen)

---

### Task 4.2: Run GraphQL Codegen

**Command:** `cd client && npm run codegen`

**Acceptance:**
- [ ] Generated types include new fields
- [ ] No TypeScript errors

---

### Task 4.3: Add Apollo Cache Policy for User

**File:** `client/src/graphql/apollo-client.ts`

**Add to cache config:**
```typescript
cache: new InMemoryCache({
  typePolicies: {
    Query: {
      fields: {
        // ... existing policies
      },
    },
    UserSyncData: {
      keyFields: ['userId'],
    },
  },
}),
```

**Acceptance:**
- [ ] User data cached by userId
- [ ] Subsequent reads use cache

---

### Phase 4 Deliverable
Frontend GraphQL ready:
- `SYNC_USER` mutation defined
- Types generated
- Cache policy configured

---

## Phase 5: Frontend - Auth Callback Flow

**Goal:** Replace SignInRedirectPage with fast AuthCallbackPage.

**Why Now:** Core feature, depends on Phases 1-4.

### Task 5.1: Create useAuthCallback Hook

**New File:** `client/src/hooks/auth/useAuthCallback.ts`

```typescript
interface AuthCallbackResult {
  status: 'processing' | 'success' | 'error';
  user: UserProfile | null;
  isFirstLogin: boolean;
  error: string | null;
}

export const useAuthCallback = (): AuthCallbackResult => {
  const { isLoading, isAuthenticated, user, error: auth0Error } = useAuth0();
  const [syncUser] = useMutation(SYNC_USER);
  const { setAuthenticated, setError } = useAuthState();

  const [result, setResult] = useState<AuthCallbackResult>({
    status: 'processing',
    user: null,
    isFirstLogin: false,
    error: null,
  });

  useEffect(() => {
    if (isLoading) return;

    if (auth0Error || !isAuthenticated || !user) {
      setResult({
        status: 'error',
        user: null,
        isFirstLogin: false,
        error: auth0Error?.message ?? 'Authentication failed',
      });
      setError({ code: 'auth_failed', message: auth0Error?.message ?? 'Authentication failed' });
      return;
    }

    // Call backend sync
    syncUser()
      .then((response) => {
        const data = response.data?.registerUserInfo;
        if (data?.__typename === 'UserRegistrationSuccessResponse') {
          const userData = data.data;
          const profile: UserProfile = {
            id: userData.userId,
            displayName: userData.displayName,
            email: userData.email ?? user.email,
            // ... map other fields
          };

          setAuthenticated(profile, userData.isFirstLogin);
          setResult({
            status: 'success',
            user: profile,
            isFirstLogin: userData.isFirstLogin,
            error: null,
          });
        } else {
          throw new Error(data?.status?.message ?? 'Sync failed');
        }
      })
      .catch((err) => {
        setResult({
          status: 'error',
          user: null,
          isFirstLogin: false,
          error: err.message,
        });
        setError({ code: 'sync_failed', message: err.message });
      });
  }, [isLoading, isAuthenticated, user, auth0Error]);

  return result;
};
```

**Acceptance:**
- [ ] Hook calls SyncUser mutation
- [ ] Updates AuthStateContext
- [ ] Returns status for UI

---

### Task 5.2: Create AuthCallbackPage

**New File:** `client/src/components/pages/AuthCallbackPage.tsx`

```typescript
export const AuthCallbackPage: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { status, user, isFirstLogin, error } = useAuthCallback();
  const { showToast } = useToast(); // Will be added in Phase 6

  useEffect(() => {
    if (status === 'success' && user) {
      // Show welcome message
      if (isFirstLogin) {
        showToast?.(`Welcome to MtgDiscovery, ${user.displayName}!`, 'success');
      } else {
        showToast?.(`Welcome back, ${user.displayName}!`, 'success');
      }

      // Navigate to intended destination or home
      const from = location.state?.from?.pathname ?? '/';
      navigate(from, { replace: true });
    }
  }, [status, user, isFirstLogin, navigate, location]);

  if (status === 'error') {
    return (
      <Box sx={{ textAlign: 'center', py: 8 }}>
        <Typography variant="h5" color="error" gutterBottom>
          Sign-in Failed
        </Typography>
        <Typography color="text.secondary" paragraph>
          {error}
        </Typography>
        <Button variant="contained" onClick={() => navigate('/')}>
          Return Home
        </Button>
      </Box>
    );
  }

  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', py: 8, gap: 2 }}>
      <CircularProgress />
      <Typography color="text.secondary">
        {status === 'processing' ? 'Completing sign-in...' : 'Setting up your profile...'}
      </Typography>
    </Box>
  );
};

export default AuthCallbackPage;
```

**Acceptance:**
- [ ] Shows loading during processing
- [ ] Shows error state with retry
- [ ] Navigates on success
- [ ] No hardcoded delays

---

### Task 5.3: Update Routes

**File:** `client/src/App.tsx`

**Changes:**
```typescript
// Add import
const AuthCallbackPage = lazy(() => import('./components/pages/AuthCallbackPage'));

// Add route (keep old route temporarily for backward compatibility)
<Route path="/auth/callback" element={
  <PageErrorBoundary name="AuthCallbackPage">
    <AuthCallbackPage />
  </PageErrorBoundary>
} />

// Keep old route during transition
<Route path="/signin-redirect" element={
  <PageErrorBoundary name="SignInRedirectPage">
    <SignInRedirectPage />
  </PageErrorBoundary>
} />
```

**Acceptance:**
- [ ] New route `/auth/callback` works
- [ ] Old route still works (backward compatibility)

---

### Task 5.4: Update Auth0 Redirect URI

**File:** `client/src/main.tsx`

**Change:**
```typescript
authorizationParams={{
  redirect_uri: `${window.location.origin}/auth/callback`,  // Changed from /signin-redirect
  audience: "api://mtg-discovery",
  scope: "openid profile email offline_access"
}}
```

**Acceptance:**
- [ ] New logins go to `/auth/callback`

---

### Task 5.5: Update Auth0 Dashboard

**External Task:**
1. Go to Auth0 Dashboard → Applications → Your App → Settings
2. Add `https://yourdomain.com/auth/callback` to Allowed Callback URLs
3. Keep `https://yourdomain.com/signin-redirect` during transition

**Acceptance:**
- [ ] Both callback URLs allowed in Auth0

---

### Phase 5 Deliverable
New auth callback flow:
- Fast callback page with no artificial delays
- Backend-driven `isFirstLogin` determination
- Proper loading and error states
- Backward compatible (old route still works)

---

## Phase 6: Frontend - UX Polish

**Goal:** Welcome messages, logout improvements, route protection.

**Why Now:** Polish after core flow works.

### Task 6.1: Create Toast Context

**New File:** `client/src/contexts/ToastContext.tsx`

```typescript
interface Toast {
  id: string;
  message: string;
  severity: 'success' | 'info' | 'warning' | 'error';
}

interface ToastContextValue {
  showToast: (message: string, severity?: Toast['severity']) => void;
}

export const ToastProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [toasts, setToasts] = useState<Toast[]>([]);

  const showToast = useCallback((message: string, severity: Toast['severity'] = 'info') => {
    const id = crypto.randomUUID();
    setToasts((prev) => [...prev, { id, message, severity }]);
  }, []);

  const removeToast = useCallback((id: string) => {
    setToasts((prev) => prev.filter((t) => t.id !== id));
  }, []);

  return (
    <ToastContext.Provider value={{ showToast }}>
      {children}
      {toasts.map((toast) => (
        <Snackbar
          key={toast.id}
          open
          autoHideDuration={4000}
          onClose={() => removeToast(toast.id)}
          anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
        >
          <Alert severity={toast.severity} onClose={() => removeToast(toast.id)}>
            {toast.message}
          </Alert>
        </Snackbar>
      ))}
    </ToastContext.Provider>
  );
};
```

**Acceptance:**
- [ ] Toast appears on `showToast` call
- [ ] Auto-dismisses after 4 seconds
- [ ] Multiple toasts supported

---

### Task 6.2: Create useToast Hook

**New File:** `client/src/hooks/useToast.ts`

```typescript
export const useToast = () => {
  const context = useContext(ToastContext);
  if (context === undefined) {
    throw new Error('useToast must be used within ToastProvider');
  }
  return context;
};
```

**Acceptance:**
- [ ] Hook provides toast functionality

---

### Task 6.3: Add ToastProvider to App

**File:** `client/src/App.tsx`

```typescript
<ToastProvider>
  <UserProvider>
    {/* ... */}
  </UserProvider>
</ToastProvider>
```

**Acceptance:**
- [ ] Toasts work throughout app

---

### Task 6.4: Update Logout to Clear Cache

**File:** `client/src/components/auth/AuthButton.tsx`

**Changes:**
```typescript
import { useApolloClient } from '@apollo/client';

// In component:
const apolloClient = useApolloClient();
const { reset } = useAuthState();

const handleLogout = useCallback(() => {
  // Clear Apollo cache
  apolloClient.clearStore();

  // Reset auth state
  reset();

  // Auth0 logout
  logout({ logoutParams: { returnTo: window.location.origin } });
}, [apolloClient, reset, logout]);

// Update onClick handlers to use handleLogout
```

**Acceptance:**
- [ ] Apollo cache cleared on logout
- [ ] Auth state reset
- [ ] No stale user data after re-login

---

### Task 6.5: Create ProtectedRoute Component

**New File:** `client/src/components/auth/ProtectedRoute.tsx`

```typescript
interface ProtectedRouteProps {
  children: React.ReactNode;
}

export const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children }) => {
  const { status } = useAuthState();
  const location = useLocation();
  const { loginWithRedirect } = useAuth0();

  useEffect(() => {
    if (status === 'unauthenticated') {
      loginWithRedirect({
        appState: { returnTo: location.pathname + location.search },
      });
    }
  }, [status, location, loginWithRedirect]);

  if (status === 'initializing') {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', py: 8 }}>
        <CircularProgress />
      </Box>
    );
  }

  if (status !== 'authenticated') {
    return null; // Will redirect
  }

  return <>{children}</>;
};
```

**Acceptance:**
- [ ] Unauthenticated users redirected to login
- [ ] Return destination preserved
- [ ] Loading state during initialization

---

### Task 6.6: Apply ProtectedRoute to Collection Routes

**File:** `client/src/App.tsx`

```typescript
// If collection routes exist or are planned:
<Route path="/collection" element={
  <ProtectedRoute>
    <PageErrorBoundary name="CollectionPage">
      <CollectionPage />
    </PageErrorBoundary>
  </ProtectedRoute>
} />
```

**Acceptance:**
- [ ] Collection routes require authentication

---

### Phase 6 Deliverable
Polished UX:
- Welcome toast messages for new and returning users
- Clean logout with cache clearing
- Protected routes with proper redirects

---

## Phase 7: Cleanup

**Goal:** Remove deprecated code after new flow is tested and stable.

**Why Last:** Only after everything works.

### Task 7.1: Remove SignInRedirectPage

**Delete File:** `client/src/components/pages/SignInRedirectPage.tsx`

**Also remove:**
- Import in App.tsx
- Route definition for `/signin-redirect`

**Acceptance:**
- [ ] File deleted
- [ ] No references remain
- [ ] Build succeeds

---

### Task 7.2: Remove Old GET_USER_INFO Query

**File:** `client/src/graphql/mutations/user.ts`

**Remove:**
```typescript
// DELETE
export const GET_USER_INFO = gql`
  query GetUserInfo {
    userInfo {
      userId
      email
    }
  }
`;
```

**Acceptance:**
- [ ] Query removed
- [ ] No usages remain

---

### Task 7.3: Clean Up useUserSync

**File:** `client/src/hooks/user/useUserSync.ts`

**Options:**
1. Remove entirely (if AuthStateContext handles everything)
2. Simplify to just use AuthStateContext

**Acceptance:**
- [ ] No redundant user sync logic
- [ ] UserContext uses AuthStateContext for user data

---

### Task 7.4: Remove localStorage Auth Code

**Audit and remove any:**
```typescript
// Search for and remove
localStorage.setItem('mtg-user-data', ...)
localStorage.getItem('mtg-user-data')
```

**Acceptance:**
- [ ] No PII in localStorage
- [ ] Auth state from Auth0/Apollo only

---

### Task 7.5: Update Auth0 Dashboard

**External Task:**
1. Remove `https://yourdomain.com/signin-redirect` from Allowed Callback URLs
2. Keep only `https://yourdomain.com/auth/callback`

**Acceptance:**
- [ ] Only new callback URL allowed

---

### Phase 7 Deliverable
Clean codebase:
- No deprecated auth code
- No localStorage PII
- Single auth flow path

---

## File Summary

### New Files (9)

| File | Phase | Purpose |
|------|-------|---------|
| `client/src/types/auth.ts` | 2 | Auth type definitions |
| `client/src/contexts/AuthStateContext.tsx` | 2 | Auth state machine |
| `client/src/hooks/auth/useAuthState.ts` | 2 | Auth state hook |
| `client/src/hooks/auth/useAuthCallback.ts` | 5 | Callback handler |
| `client/src/components/pages/AuthCallbackPage.tsx` | 5 | Callback page |
| `client/src/contexts/ToastContext.tsx` | 6 | Notifications |
| `client/src/hooks/useToast.ts` | 6 | Toast hook |
| `client/src/components/auth/ProtectedRoute.tsx` | 6 | Route guard |
| `src/Lib.Shared.DataModels/.../IUserSyncOufEntity.cs` | 1 | Backend response type |

### Modified Files (14)

| File | Phase | Changes |
|------|-------|---------|
| `UserInfoExtEntity.cs` | 1 | Add timestamps |
| `UserInfoScribe.cs` | 1 | Sync logic with isFirstLogin |
| `RegisterUserAdapter.cs` | 1 | Return extended response |
| `RegisterUserDomainService.cs` | 1 | Pass through new response |
| `RegisterUserAggregatorService.cs` | 1 | Pass through new response |
| `RegisterUserEntryService.cs` | 1 | Map new response |
| `UserMutationMethods.cs` | 1 | Update GraphQL response |
| `apollo-client.ts` | 3 | Remove subscription system |
| `Auth0TokenProvider.tsx` | 3 | Simplify |
| `useUserSync.ts` | 3, 7 | Remove subscription, simplify |
| `user.ts` (mutations) | 4 | Add SYNC_USER |
| `App.tsx` | 5, 6 | Routes, providers |
| `main.tsx` | 5 | Update redirect URI |
| `AuthButton.tsx` | 6 | Clear cache on logout |

### Deleted Files (1)

| File | Phase | Reason |
|------|-------|--------|
| `SignInRedirectPage.tsx` | 7 | Replaced by AuthCallbackPage |

---

## Testing Checklist

### Phase 1 (Backend)
- [ ] New user: `isFirstLogin: true`
- [ ] Same user again: `isFirstLogin: false`
- [ ] `createdAt` preserved on repeat calls
- [ ] `lastLoginAt` updated on each call

### Phase 2-4 (Frontend Foundation)
- [ ] Auth state transitions correctly
- [ ] Token getter works without subscriptions
- [ ] GraphQL calls include auth header

### Phase 5 (Callback Flow)
- [ ] New user flow: Auth0 → Callback → SyncUser → Home
- [ ] Returning user flow: Auth0 → Callback → SyncUser → Home
- [ ] Error handling: Auth failure shows error page

### Phase 6 (UX)
- [ ] New user sees "Welcome to MtgDiscovery!"
- [ ] Returning user sees "Welcome back, {name}!"
- [ ] Logout clears all user data
- [ ] Protected routes redirect to login

### Phase 7 (Cleanup)
- [ ] Old routes removed
- [ ] No localStorage PII
- [ ] Build and tests pass

---

## Rollback Plan

If issues arise:

1. **Phase 1-4:** No user-facing changes, safe to iterate
2. **Phase 5:** Keep both routes active, revert `redirect_uri` if needed
3. **Phase 6:** Features are additive, can disable individually
4. **Phase 7:** Don't delete until everything stable

Keep `/signin-redirect` route and Auth0 callback URL until Phase 7 is complete and tested.
