# MtgDiscovery Authentication Flow Design

## Design Principles

1. **Token-Centric**: Auth0 tokens are the single source of truth for authentication state
2. **Backend Authority**: The backend determines user existence and state, never the client
3. **Silent Session Recovery**: Returning users authenticate without seeing login screens
4. **Progressive Enhancement**: Anonymous users can browse; authentication unlocks features
5. **Single Sync Pattern**: One idempotent endpoint handles both new and returning users
6. **Graceful Degradation**: Auth failures don't break the browsing experience

---

## Authentication States

The application has exactly five authentication states:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                         APPLICATION AUTH STATES                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  ┌──────────────┐     ┌────────────────┐     ┌─────────────────┐            │
│  │              │     │                │     │                 │            │
│  │ INITIALIZING │────►│ UNAUTHENTICATED│────►│ AUTHENTICATING  │            │
│  │              │     │                │     │                 │            │
│  └──────────────┘     └────────────────┘     └─────────────────┘            │
│         │                    ▲                       │                       │
│         │                    │                       ▼                       │
│         │                    │              ┌─────────────────┐              │
│         │                    │              │                 │              │
│         │                    └──────────────│     SYNCING     │              │
│         │                    (logout)       │                 │              │
│         │                                   └─────────────────┘              │
│         │                                          │                         │
│         │         ┌────────────────┐               │                         │
│         │         │                │               │                         │
│         └────────►│ AUTHENTICATED  │◄──────────────┘                         │
│       (has        │                │         (sync success)                  │
│        session)   └────────────────┘                                         │
│                                                                              │
└─────────────────────────────────────────────────────────────────────────────┘
```

| State | Description | User Experience |
|-------|-------------|-----------------|
| `initializing` | Auth0 SDK loading, checking for existing session | Brief loading indicator (< 500ms typical) |
| `unauthenticated` | No valid session exists | Full anonymous access, login button visible |
| `authenticating` | OAuth flow in progress (redirect or popup) | "Signing you in..." overlay |
| `syncing` | Have valid token, synchronizing with backend | "Setting up your profile..." message |
| `authenticated` | Fully authenticated and synced with backend | Full authenticated experience |

---

## Core Flows

### Flow 1: Initial Page Load

Every page load follows this sequence:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                         INITIAL PAGE LOAD                                    │
└─────────────────────────────────────────────────────────────────────────────┘

User visits any page
        │
        ▼
┌───────────────────┐
│ Auth0 SDK         │
│ Initializes       │
│ (checks session)  │
└───────────────────┘
        │
        ├─────────────────────────────────────┐
        │                                     │
        ▼                                     ▼
┌───────────────────┐               ┌───────────────────┐
│ No Session        │               │ Session Exists    │
│ (anonymous)       │               │ (returning user)  │
└───────────────────┘               └───────────────────┘
        │                                     │
        ▼                                     ▼
┌───────────────────┐               ┌───────────────────┐
│ State:            │               │ Silent Token      │
│ UNAUTHENTICATED   │               │ Retrieval         │
│                   │               │ (no redirect)     │
│ User browses      │               └───────────────────┘
│ anonymously       │                         │
└───────────────────┘                         ▼
                                    ┌───────────────────┐
                                    │ Apollo Cache      │
                                    │ Check (user data) │
                                    └───────────────────┘
                                              │
                                    ┌─────────┴─────────┐
                                    │                   │
                                    ▼                   ▼
                          ┌─────────────────┐ ┌─────────────────┐
                          │ Cache Hit       │ │ Cache Miss      │
                          │ (common case)   │ │ (first load)    │
                          └─────────────────┘ └─────────────────┘
                                    │                   │
                                    │                   ▼
                                    │         ┌─────────────────┐
                                    │         │ Call SyncUser   │
                                    │         │ Mutation        │
                                    │         └─────────────────┘
                                    │                   │
                                    ▼                   ▼
                          ┌───────────────────────────────────┐
                          │ State: AUTHENTICATED              │
                          │ User sees authenticated UI        │
                          └───────────────────────────────────┘
```

**Key Points:**
- Auth0 SDK automatically checks for existing sessions using refresh tokens
- No localStorage checks needed - Auth0 manages session persistence
- Cache-first strategy means most returning visits require zero network calls
- Anonymous browsing is fully functional during initialization

---

### Flow 2: Login (New or Returning User)

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                              LOGIN FLOW                                      │
└─────────────────────────────────────────────────────────────────────────────┘

User clicks "Sign In"
        │
        ▼
┌───────────────────┐
│ Save intended     │  ← Remember where user wanted to go
│ destination       │
└───────────────────┘
        │
        ▼
┌───────────────────┐
│ Auth0 Redirect    │  ← State: AUTHENTICATING
│ (Universal Login) │
└───────────────────┘
        │
        │  User authenticates via Auth0
        │  (login, signup, social, passwordless)
        │
        ▼
┌───────────────────┐
│ Auth0 Callback    │  ← /auth/callback route
│ (code exchange)   │
└───────────────────┘
        │
        ▼
┌───────────────────┐
│ Tokens Received   │  ← Access token + ID token + Refresh token
│ (Auth0 SDK)       │
└───────────────────┘
        │
        ▼
┌───────────────────┐
│ Call SyncUser     │  ← State: SYNCING
│ Mutation          │    Single idempotent operation
└───────────────────┘
        │
        ▼
┌───────────────────────────────────────────────────────┐
│ Backend Response                                       │
│ {                                                      │
│   user: { id, displayName, email, ... },              │
│   isFirstLogin: boolean,                               │
│   needsOnboarding: boolean                             │
│ }                                                      │
└───────────────────────────────────────────────────────┘
        │
        ├─────────────────────────────────────┐
        │                                     │
        ▼                                     ▼
┌───────────────────┐               ┌───────────────────┐
│ isFirstLogin:     │               │ isFirstLogin:     │
│ false             │               │ true              │
└───────────────────┘               └───────────────────┘
        │                                     │
        ▼                                     ▼
┌───────────────────┐               ┌───────────────────┐
│ "Welcome back,    │               │ "Welcome to       │
│ {displayName}!"   │               │ MtgDiscovery!"    │
│                   │               │                   │
│ (1 second toast)  │               │ Optional:         │
│                   │               │ Quick onboarding  │
└───────────────────┘               └───────────────────┘
        │                                     │
        └─────────────────┬───────────────────┘
                          │
                          ▼
                ┌───────────────────┐
                │ Navigate to       │
                │ intended          │
                │ destination       │
                │ (or home)         │
                └───────────────────┘
                          │
                          ▼
                ┌───────────────────┐
                │ State:            │
                │ AUTHENTICATED     │
                └───────────────────┘
```

**Key Points:**
- Single `SyncUser` mutation handles both registration and login
- Backend determines if user is new (not client-side localStorage)
- Welcome message differs based on `isFirstLogin` flag
- Optional onboarding for first-time users
- User always ends up at their intended destination

---

### Flow 3: Logout

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                              LOGOUT FLOW                                     │
└─────────────────────────────────────────────────────────────────────────────┘

User clicks "Sign Out"
        │
        ▼
┌───────────────────┐
│ Clear Apollo      │  ← Remove cached user data
│ Cache             │
└───────────────────┘
        │
        ▼
┌───────────────────┐
│ Auth0 Logout      │  ← Clears Auth0 session + cookies
│                   │
└───────────────────┘
        │
        ▼
┌───────────────────┐
│ Redirect to       │  ← Clean anonymous state
│ Home Page         │
└───────────────────┘
        │
        ▼
┌───────────────────┐
│ State:            │
│ UNAUTHENTICATED   │
└───────────────────┘
```

**Key Points:**
- Apollo cache cleared (user data removed)
- Auth0 session invalidated (refresh tokens revoked)
- No localStorage to clear (we don't use it for auth)
- User returns to clean anonymous state

---

### Flow 4: Token Refresh (Automatic)

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                         TOKEN REFRESH (INVISIBLE)                            │
└─────────────────────────────────────────────────────────────────────────────┘

Access token expires (or nearing expiry)
        │
        ▼
┌───────────────────┐
│ Apollo Link       │  ← Intercepts GraphQL request
│ detects expiry    │
└───────────────────┘
        │
        ▼
┌───────────────────┐
│ Auth0 SDK         │  ← Uses refresh token
│ getAccessToken    │    Happens silently
│ Silently()        │
└───────────────────┘
        │
        ├─────────────────────────────────────┐
        │                                     │
        ▼                                     ▼
┌───────────────────┐               ┌───────────────────┐
│ Success           │               │ Failure           │
│ (new token)       │               │ (session expired) │
└───────────────────┘               └───────────────────┘
        │                                     │
        ▼                                     ▼
┌───────────────────┐               ┌───────────────────┐
│ Continue with     │               │ Clear state       │
│ original request  │               │ Redirect to login │
│                   │               │ with message      │
│ User never knows  │               │                   │
└───────────────────┘               └───────────────────┘
```

**Key Points:**
- Token refresh is invisible to the user
- Happens automatically via Auth0 SDK
- On failure, graceful redirect to login (not error screen)
- Message: "Your session expired. Please sign in again."

---

### Flow 5: Protected Route Access

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                         PROTECTED ROUTE ACCESS                               │
└─────────────────────────────────────────────────────────────────────────────┘

User navigates to /collection (protected)
        │
        ▼
┌───────────────────┐
│ Route Guard       │
│ checks auth state │
└───────────────────┘
        │
        ├─────────────────────────────────────┐
        │                                     │
        ▼                                     ▼
┌───────────────────┐               ┌───────────────────┐
│ AUTHENTICATED     │               │ UNAUTHENTICATED   │
└───────────────────┘               └───────────────────┘
        │                                     │
        ▼                                     ▼
┌───────────────────┐               ┌───────────────────┐
│ Render protected  │               │ Save /collection  │
│ component         │               │ as destination    │
└───────────────────┘               └───────────────────┘
                                              │
                                              ▼
                                    ┌───────────────────┐
                                    │ Redirect to       │
                                    │ Auth0 Login       │
                                    └───────────────────┘
                                              │
                                              │ (after login)
                                              ▼
                                    ┌───────────────────┐
                                    │ Redirect back to  │
                                    │ /collection       │
                                    └───────────────────┘
```

---

## Backend Contract

### SyncUser Mutation

The backend exposes a single idempotent mutation for user synchronization:

```graphql
type Mutation {
  """
  Synchronizes the authenticated user with the backend.
  Creates the user if they don't exist, updates last login if they do.
  Call this after every successful Auth0 authentication.
  """
  syncUser: SyncUserResult!
}

type SyncUserResult {
  """The user's profile information"""
  user: UserProfile!

  """True if this is the user's first time logging in"""
  isFirstLogin: Boolean!

  """True if the user should see onboarding (incomplete profile, etc.)"""
  needsOnboarding: Boolean!
}

type UserProfile {
  id: ID!
  displayName: String!
  email: String
  avatarUrl: String
  createdAt: DateTime!
  lastLoginAt: DateTime!
}
```

### Backend Behavior

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                         SYNCUSER BACKEND LOGIC                               │
└─────────────────────────────────────────────────────────────────────────────┘

Receive SyncUser mutation (with JWT)
        │
        ▼
┌───────────────────┐
│ Extract claims    │  ← sub, email, name, picture from JWT
│ from JWT          │
└───────────────────┘
        │
        ▼
┌───────────────────┐
│ Query user by     │  ← Use Auth0 'sub' as unique identifier
│ auth0 subject ID  │
└───────────────────┘
        │
        ├─────────────────────────────────────┐
        │                                     │
        ▼                                     ▼
┌───────────────────┐               ┌───────────────────┐
│ User exists       │               │ User not found    │
└───────────────────┘               └───────────────────┘
        │                                     │
        ▼                                     ▼
┌───────────────────┐               ┌───────────────────┐
│ Update:           │               │ Create user:      │
│ - lastLoginAt     │               │ - Generate ID     │
│ - email (if       │               │ - Set all fields  │
│   changed)        │               │ - createdAt = now │
│ - name (if        │               │ - lastLoginAt=now │
│   changed)        │               └───────────────────┘
└───────────────────┘                         │
        │                                     │
        ▼                                     ▼
┌───────────────────┐               ┌───────────────────┐
│ Return:           │               │ Return:           │
│ isFirstLogin:     │               │ isFirstLogin:     │
│ false             │               │ true              │
└───────────────────┘               └───────────────────┘
```

---

## Component Architecture

### Auth Provider Hierarchy

```tsx
<Auth0Provider>           // Auth0 SDK initialization
  <ApolloProvider>        // GraphQL client (uses Auth0 for tokens)
    <AuthStateProvider>   // App-level auth state machine
      <App />
    </AuthStateProvider>
  </ApolloProvider>
</Auth0Provider>
```

### Key Components

#### 1. AuthStateProvider
Manages the auth state machine and provides context:

```typescript
interface AuthState {
  status: 'initializing' | 'unauthenticated' | 'authenticating' | 'syncing' | 'authenticated';
  user: UserProfile | null;
  isFirstLogin: boolean;
  error: AuthError | null;
}

interface AuthContextValue extends AuthState {
  login: (returnTo?: string) => void;
  logout: () => void;
  isAuthenticated: boolean;
}
```

#### 2. AuthCallback Page
Minimal page that handles the OAuth callback:

```typescript
// /auth/callback
// Shown briefly during: AUTHENTICATING → SYNCING → AUTHENTICATED
// Then redirects to intended destination
```

#### 3. ProtectedRoute Component
Route wrapper that enforces authentication:

```typescript
// Checks auth state
// If unauthenticated: saves destination, redirects to login
// If authenticated: renders children
// If initializing: shows loading state
```

#### 4. AuthenticatedQuery/Mutation Wrappers
Apollo hooks that handle auth state:

```typescript
// Automatically waits for AUTHENTICATED state
// Handles token refresh failures gracefully
// Shows appropriate loading/error states
```

---

## User Experience Details

### Loading States

| State | Duration | What User Sees |
|-------|----------|----------------|
| `initializing` | 100-500ms | Skeleton UI or subtle spinner in header |
| `authenticating` | 2-10s | Full-page "Signing you in..." with spinner |
| `syncing` | 100-500ms | "Setting up your profile..." message |

### Messages

| Scenario | Message |
|----------|---------|
| First login | "Welcome to MtgDiscovery! Let's get you started." |
| Returning user | "Welcome back, {displayName}!" (toast, 3 seconds) |
| Session expired | "Your session expired. Please sign in again." |
| Auth error | "Something went wrong signing you in. Please try again." |
| Logout | "You've been signed out." (toast, 3 seconds) |

### Animation Guidelines

- **State transitions**: 200ms fade transitions between auth states
- **Welcome toast**: Slide in from top, auto-dismiss after 3 seconds
- **Loading spinners**: Use skeleton loaders for content, spinners for actions

---

## Route Structure

```
/                     Public    Home page (browse cards/sets)
/sets                 Public    All sets list
/sets/:setCode        Public    Set detail page
/cards/:cardId        Public    Card detail page
/search               Public    Card search
/artists/:artistId    Public    Artist page

/auth/callback        Special   OAuth callback handler (brief)

/collection           Protected User's card collection
/collection/add       Protected Add cards to collection
/profile              Protected User profile settings
/profile/preferences  Protected User preferences
```

---

## Error Handling

### Auth Errors

| Error | User Impact | Recovery |
|-------|-------------|----------|
| Auth0 unavailable | Can't log in | "Authentication service unavailable. Please try again later." |
| Token refresh failed | Session ended | Redirect to login with "Session expired" message |
| SyncUser failed | Can't complete login | "We couldn't set up your profile. Please try again." + Retry button |
| Network error during auth | Login incomplete | "Network error. Please check your connection and try again." |

### Graceful Degradation

- If auth fails during page load, user continues as anonymous
- If sync fails, user can retry without re-authenticating
- Protected routes show helpful message, not error screen

---

## Security Considerations

### What We Store

| Storage | Data | Purpose |
|---------|------|---------|
| Auth0 (cookies) | Session, refresh token | Session persistence |
| Memory (Apollo cache) | User profile | Avoid redundant API calls |
| Memory (React state) | Auth state | UI rendering |
| localStorage | **Nothing auth-related** | — |

### What We Don't Store

- No tokens in localStorage (XSS vulnerable)
- No user PII in localStorage
- No "isReturningUser" flags (backend determines this)

### Token Handling

- Access tokens: Short-lived (1 hour), in memory only
- Refresh tokens: Managed by Auth0 SDK, rotated automatically
- ID tokens: Used once to get claims, not stored

---

## Implementation Checklist

### Phase 1: Core Auth State Machine
- [ ] Create `AuthStateProvider` with state machine
- [ ] Implement `useAuthState` hook
- [ ] Create `/auth/callback` page
- [ ] Update Apollo client to use Auth0 token getter

### Phase 2: Backend Sync
- [ ] Create `SyncUser` mutation (backend)
- [ ] Implement upsert logic with `isFirstLogin` flag
- [ ] Update GraphQL schema

### Phase 3: UI Integration
- [ ] Update header with auth state awareness
- [ ] Create `ProtectedRoute` component
- [ ] Add welcome messages (toast system)
- [ ] Implement loading states

### Phase 4: Polish
- [ ] Add error boundaries for auth failures
- [ ] Implement session expiry handling
- [ ] Add "intended destination" preservation
- [ ] Test all flows end-to-end

---

## Comparison with Current Implementation

| Aspect | Current | Proposed |
|--------|---------|----------|
| Source of truth | localStorage | Auth0 tokens + backend |
| New vs returning | Client checks localStorage | Backend returns `isFirstLogin` |
| Registration | Separate mutation, called every time | Single `SyncUser`, idempotent |
| Session recovery | Custom token subscription | Auth0 SDK built-in |
| User data storage | localStorage (PII exposed) | Apollo cache (memory only) |
| Token refresh | Manual handling | Auth0 SDK automatic |
| State management | Ad-hoc flags | Explicit state machine |

---

## Summary

This design provides:

1. **Security**: No sensitive data in localStorage, tokens managed by Auth0
2. **Reliability**: Backend is authority, client state derived from tokens
3. **Performance**: Cache-first queries, silent token refresh, minimal network calls
4. **UX**: Clear loading states, appropriate messages, seamless session recovery
5. **Simplicity**: Single sync endpoint, explicit state machine, no complex subscriptions
