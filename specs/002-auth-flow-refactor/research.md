# Research: Authentication Flow Refactoring

**Feature Branch**: `002-auth-flow-refactor`
**Date**: 2026-01-18
**Status**: Complete

## Research Summary

This document captures research findings and decisions for the authentication flow refactoring. The primary research was conducted during the initial design phase (see `.docs/auth-flow-design.md` and `.docs/auth-implementation-mapping.md`).

---

## 1. Backend User Sync Pattern

### Decision
Implement a single idempotent `SyncUser` operation that returns `isFirstLogin` flag based on comparing `createdAt` and `lastLoginAt` timestamps.

### Rationale
- **Backend authority**: The backend determines new vs returning user status, not the client
- **Idempotency**: Safe to call multiple times without side effects (UPSERT pattern)
- **Single operation**: Eliminates need for separate "register" and "get user" calls
- **Explicit state**: `isFirstLogin` flag provides clear signal for welcome message logic

### Alternatives Considered

| Alternative | Rejected Because |
|-------------|------------------|
| Client-side localStorage check | Client state can be manipulated, stale, or out of sync; security risk |
| Separate register + query operations | Requires multiple round trips, complex coordination |
| Backend returns nothing, client infers | Violates "backend authority" principle |

### Implementation Approach
- Add `CreatedAt` and `LastLoginAt` to `UserInfoExtEntity`
- Query for existing user before UPSERT
- If user doesn't exist: `isFirstLogin = true`, set both timestamps to now
- If user exists: `isFirstLogin = false`, preserve `CreatedAt`, update `LastLoginAt`

---

## 2. Frontend Auth State Machine

### Decision
Implement explicit auth state machine with five states: `initializing`, `unauthenticated`, `authenticating`, `syncing`, `authenticated`.

### Rationale
- **Explicit states**: Replaces ad-hoc boolean flags with clear state machine
- **Predictable behavior**: Each state has defined transitions and UI implications
- **Debugging**: Easy to understand current state and how it was reached
- **React pattern**: Uses `useReducer` for state management (standard React pattern)

### Alternatives Considered

| Alternative | Rejected Because |
|-------------|------------------|
| Multiple boolean flags | Complex to track, race conditions, unclear state combinations |
| Auth0 state only | Missing `syncing` state, can't distinguish backend sync from Auth0 loading |
| Global state (Redux/MobX) | Overkill for auth state; React context sufficient |

### State Transitions
```
initializing → unauthenticated (no session)
initializing → authenticated (has valid session + cached user)
unauthenticated → authenticating (login clicked)
authenticating → syncing (Auth0 callback received, token obtained)
syncing → authenticated (backend sync successful)
syncing → unauthenticated (backend sync failed)
authenticated → unauthenticated (logout)
```

---

## 3. Token Management Simplification

### Decision
Remove the global token subscription system and rely on Auth0 SDK's built-in token management.

### Rationale
- **Unnecessary complexity**: Auth0 SDK already handles token caching and refresh
- **Race conditions**: Subscription system added complexity without solving real problems
- **Simpler code**: Direct token getter registration is sufficient

### Alternatives Considered

| Alternative | Rejected Because |
|-------------|------------------|
| Keep subscription system | Adds complexity, no clear benefit over direct getter |
| Custom token caching | Auth0 SDK already provides this via `cacheLocation` and `useRefreshTokens` |
| Token in Apollo cache | Tokens should be memory-only for security |

### Implementation Approach
- Keep `setAuth0TokenGetter` for registering the token retrieval function
- Remove `subscribeToTokenReady`, `setTokenReadyState`, `getTokenReadyState`
- Components check Auth0 state directly (`isAuthenticated && !isLoading`)

---

## 4. PII Storage

### Decision
Store no PII in localStorage. User data lives only in Apollo cache (memory) and Auth0 session.

### Rationale
- **Security**: localStorage is vulnerable to XSS attacks
- **Data freshness**: Auth0 tokens are the source of truth; cached localStorage data can be stale
- **GDPR/Privacy**: Minimizes data footprint on user devices
- **No benefit**: The current localStorage PII is stored but never read for any functionality

### Alternatives Considered

| Alternative | Rejected Because |
|-------------|------------------|
| Encrypt PII in localStorage | Adds complexity, key management issues, still XSS vulnerable |
| Store minimal flags only | Even flags can leak information; Auth0 session is sufficient |
| IndexedDB with encryption | Same issues as localStorage plus browser compatibility |

### Implementation Approach
- Remove all `localStorage.setItem('mtg-user-data', ...)` calls
- User profile data from Apollo cache (cache-first strategy)
- Session persistence via Auth0 SDK (`cacheLocation: 'localStorage'` for session tokens, not PII)

---

## 5. Welcome Message Implementation

### Decision
Use MUI Snackbar/Alert for toast notifications, triggered based on `isFirstLogin` flag from backend.

### Rationale
- **Material-UI consistency**: Project already uses MUI; Snackbar is the standard toast component
- **Backend-driven**: Message type determined by `isFirstLogin` from server
- **Non-blocking**: Toast auto-dismisses, doesn't interrupt user flow

### Alternatives Considered

| Alternative | Rejected Because |
|-------------|------------------|
| Full-page welcome screen | Interrupts flow, unnecessary for returning users |
| Browser notification | Requires permission, inconsistent UX |
| Custom toast library | Would add dependency when MUI already provides Snackbar |

### Message Specifications
- New user: "Welcome to MtgDiscovery, {displayName}!" (severity: success)
- Returning user: "Welcome back, {displayName}!" (severity: success)
- Auto-dismiss: 4 seconds
- Position: top-center

---

## 6. OAuth Callback Route

### Decision
Change callback URL from `/signin-redirect` to `/auth/callback` with backward compatibility during transition.

### Rationale
- **Industry standard**: `/auth/callback` is the conventional OAuth callback route
- **Clear purpose**: Route name explicitly indicates its purpose
- **Separation of concerns**: Callback page handles OAuth only, separate from registration logic

### Alternatives Considered

| Alternative | Rejected Because |
|-------------|------------------|
| Keep `/signin-redirect` | Name doesn't reflect purpose; opportunity to improve |
| `/callback` | Too generic, could conflict with other features |
| No route change | Missed opportunity to align with conventions |

### Implementation Approach
- Create new `AuthCallbackPage` at `/auth/callback`
- Keep old `/signin-redirect` route during transition (both in code and Auth0 dashboard)
- Update Auth0 redirect URI in `main.tsx`
- Remove old route in cleanup phase after verification

---

## 7. Protected Routes

### Decision
Implement `ProtectedRoute` component that redirects unauthenticated users to login with return-to preservation.

### Rationale
- **Standard pattern**: Common React Router pattern for protected routes
- **User experience**: Preserves intended destination for seamless post-login navigation
- **Explicit protection**: Makes route protection visible in route configuration

### Alternatives Considered

| Alternative | Rejected Because |
|-------------|------------------|
| Feature-level protection only | Routes themselves should enforce protection |
| Higher-order component (HOC) | Wrapper component is more idiomatic in modern React |
| Route middleware | React Router doesn't have middleware; component wrapper is equivalent |

### Implementation Approach
- Create `ProtectedRoute` component that checks auth state
- If `initializing`: show loading spinner
- If `unauthenticated`: redirect to login with `appState.returnTo`
- If `authenticated`: render children
- Apply to collection routes (and future protected routes)

---

## 8. Session Expiry Handling

### Decision
Rely on Auth0 SDK's automatic token refresh with graceful fallback to re-authentication message.

### Rationale
- **Auth0 handles complexity**: SDK manages refresh token rotation automatically
- **Minimal custom code**: Only need to handle the failure case
- **Clear messaging**: User sees "Your session expired" only when refresh truly fails

### Alternatives Considered

| Alternative | Rejected Because |
|-------------|------------------|
| Custom token refresh logic | Auth0 SDK already does this; reinventing the wheel |
| Preemptive refresh | Auth0 SDK handles this with `useRefreshTokens: true` |
| Silent re-authentication | Auth0 SDK tries this automatically before failing |

### Implementation Approach
- Keep Auth0 configuration: `useRefreshTokens: true`
- In token getter error handler: detect `login_required` or `Missing Refresh Token`
- Show message: "Your session expired. Please sign in again."
- Trigger `loginWithRedirect()` for re-authentication

---

## Technology Decisions Summary

| Decision Area | Choice | Confidence |
|---------------|--------|------------|
| Backend sync pattern | Single idempotent SyncUser with isFirstLogin | High |
| Frontend state management | Explicit state machine with useReducer | High |
| Token management | Simplified getter, no subscription | High |
| PII storage | None in localStorage | High |
| Toast notifications | MUI Snackbar | High |
| Callback route | /auth/callback with transition period | High |
| Protected routes | ProtectedRoute wrapper component | High |
| Session expiry | Auth0 automatic refresh + graceful failure | High |

---

## Open Questions

None. All research questions resolved.

---

## References

- [Auth0 React SDK Documentation](https://auth0.com/docs/libraries/auth0-react)
- [OAuth 2.0 Authorization Framework (RFC 6749)](https://datatracker.ietf.org/doc/html/rfc6749)
- [Auth Implementation Mapping](.docs/auth-implementation-mapping.md)
- [Auth Flow Design](.docs/auth-flow-design.md)
