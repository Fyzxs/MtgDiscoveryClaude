# Authentication Flow Refactoring Plan

## Overview

This plan addresses the "first time every time" feeling in the authentication flow by improving returning user detection, simplifying token management, and establishing consistent patterns for user state persistence.

## Current Problems

| Issue | Location | Impact |
|-------|----------|--------|
| No returning user detection | SignInRedirectPage.tsx | Users see registration flow every login |
| `mtg-user-data` stored but never read | SignInRedirectPage.tsx:95 | Can't detect returning users |
| REGISTER_USER called every redirect | SignInRedirectPage.tsx:78 | Unnecessary API calls |
| Complex token subscription system | apollo-client.ts:11-56 | Hard to debug, race conditions |
| `isFirstTimeUser` from query errors | useUserSync.ts:110-114 | Unclear semantics |
| No cache-first on GET_USER_INFO | useUserSync.ts:87 | Extra network requests |

---

## Phase 1: Quick Win - Returning User Detection

**Estimated Time**: 2-4 hours
**Risk**: Low

### Goal
Skip registration flow for returning users by checking localStorage before making any GraphQL calls.

### 1.1 Create User Storage Utility

**New File**: `client/src/utils/userStorage.ts`

```typescript
import { logger } from './logger';

const USER_STORAGE_KEY = 'mtg-user-data';

export interface StoredUserData {
  sub: string;           // Auth0 subject ID
  email?: string;
  name?: string;
  picture?: string;
  userId: string;        // Backend user ID
  displayName?: string;
  registeredAt: string;  // ISO timestamp of first registration
  lastLoginAt: string;   // ISO timestamp of last login
}

/**
 * Get stored user data from localStorage.
 * Returns null if no user data exists or if data is invalid.
 */
export function getStoredUserData(): StoredUserData | null {
  try {
    const stored = localStorage.getItem(USER_STORAGE_KEY);
    if (stored === null) {
      return null;
    }

    const parsed = JSON.parse(stored) as StoredUserData;

    if (parsed.sub === undefined || parsed.userId === undefined) {
      logger.warn('UserStorage - Invalid stored user data');
      return null;
    }

    return parsed;
  } catch (error) {
    logger.error('UserStorage - Failed to parse stored user data:', error);
    return null;
  }
}

/**
 * Check if a returning user exists for the given Auth0 subject ID.
 */
export function isReturningUser(auth0Sub: string): boolean {
  const stored = getStoredUserData();
  return stored !== null && stored.sub === auth0Sub;
}

/**
 * Save user data to localStorage after successful registration.
 */
export function saveUserData(data: StoredUserData): void {
  try {
    localStorage.setItem(USER_STORAGE_KEY, JSON.stringify(data));
  } catch (error) {
    logger.error('UserStorage - Failed to save user data:', error);
  }
}

/**
 * Update the last login timestamp for returning users.
 */
export function updateLastLogin(): void {
  const stored = getStoredUserData();
  if (stored !== null) {
    stored.lastLoginAt = new Date().toISOString();
    saveUserData(stored);
  }
}

/**
 * Clear stored user data (on logout).
 */
export function clearUserData(): void {
  localStorage.removeItem(USER_STORAGE_KEY);
}
```

### 1.2 Modify SignInRedirectPage.tsx

**File**: `client/src/components/pages/SignInRedirectPage.tsx`

Add returning user check at the start of `handleUserSetup`:

```typescript
// Add imports
import {
  isReturningUser,
  saveUserData,
  updateLastLogin,
  StoredUserData
} from '../../utils/userStorage';

// In handleUserSetup, add after auth checks and BEFORE token wait:
if (isReturningUser(user.sub ?? '')) {
  logger.debug('SignInRedirectPage - Returning user detected, skipping registration');
  updateLastLogin();
  setSetupStatus('complete');
  setStatusMessage('Welcome back! Redirecting...');
  setTimeout(() => {
    navigate('/', { replace: true });
  }, 500);
  return;
}

// Existing token wait and registration code continues below...
```

Update the successful registration block to use new storage:

```typescript
// After successful registration, replace existing localStorage code:
const userData: StoredUserData = {
  sub: user.sub ?? '',
  email: user.email,
  name: user.name ?? user.email,
  picture: user.picture,
  userId: registrationData?.userId ?? '',
  displayName: registrationData?.displayName,
  registeredAt: new Date().toISOString(),
  lastLoginAt: new Date().toISOString(),
};

saveUserData(userData);
```

### 1.3 Clear User Data on Logout

**File**: `client/src/components/auth/AuthButton.tsx`

```typescript
// Add import
import { clearUserData } from '../../utils/userStorage';

// In logout onClick handlers (lines ~74 and ~102):
onClick={() => {
  clearUserData();
  logout({ logoutParams: { returnTo: window.location.origin } });
}}
```

### 1.4 Testing Checklist

- [ ] New user: Clear localStorage, login → see registration flow → redirect to home
- [ ] Returning user: Login with existing localStorage → skip registration → fast redirect
- [ ] Logout clears localStorage
- [ ] Re-login after logout treated as new user (registration called once)

---

## Phase 2: Separate Auth Callback from Registration

**Estimated Time**: 3-5 hours
**Risk**: Medium (requires Auth0 dashboard update)

### Goal
Create fast callback path for returning users, dedicated registration path for new users.

### 2.1 Create Auth Callback Hook

**New File**: `client/src/hooks/auth/useAuthCallback.ts`

```typescript
import { useEffect, useState } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { logger } from '../../utils/logger';
import { isReturningUser, updateLastLogin } from '../../utils/userStorage';

type CallbackStatus = 'processing' | 'returning' | 'new-user' | 'error';

interface AuthCallbackResult {
  status: CallbackStatus;
  redirectTo: string;
}

export const useAuthCallback = (): AuthCallbackResult => {
  const { isLoading, isAuthenticated, user, error } = useAuth0();
  const [result, setResult] = useState<AuthCallbackResult>({
    status: 'processing',
    redirectTo: '/'
  });

  useEffect(() => {
    if (isLoading) return;

    if (error !== undefined) {
      logger.error('Auth callback error:', error);
      setResult({ status: 'error', redirectTo: '/' });
      return;
    }

    if (isAuthenticated === false || user === undefined) {
      setResult({ status: 'error', redirectTo: '/' });
      return;
    }

    if (isReturningUser(user.sub ?? '')) {
      updateLastLogin();
      setResult({ status: 'returning', redirectTo: '/' });
    } else {
      setResult({ status: 'new-user', redirectTo: '/signin-redirect' });
    }
  }, [isLoading, isAuthenticated, user, error]);

  return result;
};
```

### 2.2 Create Fast Callback Page

**New File**: `client/src/components/pages/AuthCallbackPage.tsx`

```typescript
import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, CircularProgress, Typography } from '@mui/material';
import { useAuthCallback } from '../../hooks/auth/useAuthCallback';

export const AuthCallbackPage: React.FC = () => {
  const navigate = useNavigate();
  const { status, redirectTo } = useAuthCallback();

  useEffect(() => {
    if (status !== 'processing') {
      const delay = status === 'returning' ? 300 : 0;
      setTimeout(() => {
        navigate(redirectTo, { replace: true });
      }, delay);
    }
  }, [status, redirectTo, navigate]);

  return (
    <Box sx={{
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      justifyContent: 'center',
      minHeight: '60vh',
      gap: 2
    }}>
      <CircularProgress />
      <Typography variant="body1" color="text.secondary">
        {status === 'returning' ? 'Welcome back!' : 'Completing sign-in...'}
      </Typography>
    </Box>
  );
};

export default AuthCallbackPage;
```

### 2.3 Update Routing

**File**: `client/src/App.tsx`

```typescript
// Add import
const AuthCallbackPage = lazy(() => import('./components/pages/AuthCallbackPage'));

// Add route before signin-redirect
<Route path="/auth/callback" element={
  <PageErrorBoundary name="AuthCallbackPage">
    <AuthCallbackPage />
  </PageErrorBoundary>
} />
```

### 2.4 Update Auth0 Configuration

**File**: `client/src/main.tsx`

```typescript
authorizationParams={{
  redirect_uri: `${window.location.origin}/auth/callback`, // Changed from /signin-redirect
  audience: "api://mtg-discovery",
  scope: "openid profile email offline_access"
}}
```

**Auth0 Dashboard**: Add `https://yourdomain.com/auth/callback` to allowed callback URLs before deployment.

---

## Phase 3: Simplify Token Readiness

**Estimated Time**: 2-3 hours
**Risk**: Medium

### Goal
Replace global subscription system with simpler, component-local approach.

### 3.1 Create Token Ready Hook

**New File**: `client/src/hooks/auth/useTokenReady.ts`

```typescript
import { useState, useEffect } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { logger } from '../../utils/logger';

interface TokenReadyState {
  isReady: boolean;
  isWaiting: boolean;
  error: string | null;
}

export const useTokenReady = (): TokenReadyState => {
  const { isAuthenticated, isLoading, getAccessTokenSilently } = useAuth0();
  const [state, setState] = useState<TokenReadyState>({
    isReady: false,
    isWaiting: true,
    error: null
  });

  useEffect(() => {
    const checkToken = async () => {
      if (isLoading) return;

      if (isAuthenticated === false) {
        setState({ isReady: false, isWaiting: false, error: null });
        return;
      }

      try {
        setState(prev => ({ ...prev, isWaiting: true }));
        await getAccessTokenSilently({
          authorizationParams: { audience: "api://mtg-discovery" }
        });
        setState({ isReady: true, isWaiting: false, error: null });
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Unknown error';
        logger.error('Token verification failed:', message);
        setState({ isReady: false, isWaiting: false, error: message });
      }
    };

    void checkToken();
  }, [isAuthenticated, isLoading, getAccessTokenSilently]);

  return state;
};
```

### 3.2 Simplify Auth0TokenProvider

**File**: `client/src/components/auth/Auth0TokenProvider.tsx`

Remove subscription system, keep only token getter registration:

```typescript
import React, { useEffect, useRef } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { setAuth0TokenGetter } from '../../graphql/apollo-client';
import { logger } from '../../utils/logger';

interface Auth0TokenProviderProps {
  children: React.ReactNode;
}

export const Auth0TokenProvider: React.FC<Auth0TokenProviderProps> = ({ children }) => {
  const { getAccessTokenSilently, isAuthenticated, isLoading, loginWithRedirect } = useAuth0();
  const registered = useRef(false);

  useEffect(() => {
    if (isLoading || isAuthenticated === false || registered.current) {
      return;
    }

    const getToken = async (): Promise<string | null> => {
      try {
        return await getAccessTokenSilently({
          authorizationParams: { audience: "api://mtg-discovery" }
        }) ?? null;
      } catch (error) {
        const message = error instanceof Error ? error.message : String(error);
        if (message.includes('Missing Refresh Token') || message.includes('login_required')) {
          loginWithRedirect();
          return null;
        }
        logger.error('Failed to get access token:', error);
        return null;
      }
    };

    setAuth0TokenGetter(getToken);
    registered.current = true;
  }, [getAccessTokenSilently, isAuthenticated, isLoading, loginWithRedirect]);

  return <>{children}</>;
};
```

### 3.3 Simplify apollo-client.ts

**File**: `client/src/graphql/apollo-client.ts`

Remove lines 11-56 (subscription system). Keep only:

```typescript
let getAuth0Token: (() => Promise<string | null>) | null = null;

export const setAuth0TokenGetter = (tokenGetter: () => Promise<string | null>) => {
  getAuth0Token = tokenGetter;
};

// authLink uses getAuth0Token directly (unchanged)
```

---

## Phase 4: Improve User State Caching

**Estimated Time**: 2-3 hours
**Risk**: Low

### Goal
Use Apollo cache-first strategy to avoid redundant network requests.

### 4.1 Update useUserSync Query

**File**: `client/src/hooks/user/useUserSync.ts`

```typescript
const { data, loading, error, refetch } = useQuery<UserInfoQueryData>(GET_USER_INFO, {
  skip: shouldQueryUserInfo === false,
  fetchPolicy: 'cache-first',
  nextFetchPolicy: 'cache-and-network',
  errorPolicy: 'all'
});
```

### 4.2 Add User Cache Policy

**File**: `client/src/graphql/apollo-client.ts`

```typescript
cache: new InMemoryCache({
  typePolicies: {
    Query: {
      fields: {
        userInfo: {
          merge(existing, incoming) {
            return incoming ?? existing;
          },
        },
      },
    },
  },
}),
```

---

## Phase 5: Clean Up Storage Keys

**Estimated Time**: 1-2 hours
**Risk**: Low

### Goal
Centralize localStorage key constants for consistency.

**New File**: `client/src/utils/storageKeys.ts`

```typescript
export const STORAGE_KEYS = {
  USER_DATA: 'mtg-user-data',
  LANGUAGE: 'mtg-discovery-language',
  VISITED: 'mtg-discovery-visited',
  CARD_SIZE: 'mtg-card-size-preference',
} as const;
```

Update references in:
- `client/src/utils/userStorage.ts`
- `client/src/hooks/useCardSizePreference.ts`
- `client/src/hooks/useLanguageDetection.ts`

---

## Migration & Rollout

### Backward Compatibility

1. **Existing users**: Will go through registration once after deployment (backend is idempotent), then be recognized as returning users
2. **No backend changes required**: All changes are frontend-only

### Rollout Strategy

**Recommended: Phased Deployment**

1. Deploy Phase 1 only (quick win) → Monitor for 2-3 days
2. Deploy Phase 3-5 (simplifications) → Monitor
3. Deploy Phase 2 (new callback route) → Requires Auth0 dashboard update first

### Feature Flag Option

```typescript
const USE_FAST_AUTH = import.meta.env.VITE_FAST_AUTH_FLOW === 'true';
```

---

## Testing Verification

### Phase 1 Tests
- [ ] New user sees registration flow
- [ ] Returning user skips registration (< 1 second redirect)
- [ ] Logout clears localStorage
- [ ] "Welcome back!" message shown for returning users

### Phase 2 Tests
- [ ] `/auth/callback` routes returning users to `/`
- [ ] `/auth/callback` routes new users to `/signin-redirect`
- [ ] Direct navigation to `/signin-redirect` without auth redirects to login

### Phase 3 Tests
- [ ] `useTokenReady` hook works in components
- [ ] Authenticated GraphQL calls succeed
- [ ] No race conditions on page load

### Integration Tests
- [ ] Page refresh preserves auth state
- [ ] Token refresh works after extended session
- [ ] Graceful handling of network errors

---

## Risk Assessment

| Phase | Risk | Mitigation |
|-------|------|------------|
| Phase 1 | Low | Additive, no breaking changes |
| Phase 2 | Medium | Update Auth0 dashboard first, keep old route as fallback |
| Phase 3 | Medium | Thorough testing of token timing |
| Phase 4 | Low | Cache-first is standard Apollo pattern |
| Phase 5 | Low | Pure refactoring |

---

## Critical Files

| File | Phase | Changes |
|------|-------|---------|
| `client/src/utils/userStorage.ts` | 1 | New file |
| `client/src/components/pages/SignInRedirectPage.tsx` | 1 | Add returning user check |
| `client/src/components/auth/AuthButton.tsx` | 1 | Clear storage on logout |
| `client/src/hooks/auth/useAuthCallback.ts` | 2 | New file |
| `client/src/components/pages/AuthCallbackPage.tsx` | 2 | New file |
| `client/src/main.tsx` | 2 | Update redirect URI |
| `client/src/App.tsx` | 2 | Add new route |
| `client/src/hooks/auth/useTokenReady.ts` | 3 | New file |
| `client/src/components/auth/Auth0TokenProvider.tsx` | 3 | Simplify |
| `client/src/graphql/apollo-client.ts` | 3, 4 | Remove subscriptions, add cache policy |
| `client/src/hooks/user/useUserSync.ts` | 4 | Add cache-first policy |
| `client/src/utils/storageKeys.ts` | 5 | New file |

---

## Summary

**Quick Win (Phase 1)**: Add `isReturningUser()` check to SignInRedirectPage → returning users skip registration flow entirely.

**Full Solution (Phases 1-5)**: Clean separation between fast callback (returning users) and registration (new users), simplified token management, proper caching.

**Expected Outcome**: Returning users experience near-instant redirect after Auth0 callback instead of seeing "Registering your account..." every time.
