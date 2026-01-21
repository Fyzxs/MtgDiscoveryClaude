# Quickstart Guide: Authentication Flow Refactoring

**Feature**: Authentication Flow Refactoring
**Branch**: 001-auth-refactoring
**Date**: 2026-01-17
**Constitution Version**: 1.0.0

## Overview

This guide provides step-by-step instructions for implementing the authentication flow refactoring following the constitution's development workflow and quality gates.

## Prerequisites

### Development Environment
- Node.js 18+ with npm
- Access to Auth0 Dashboard (for Phase 2)
- Azure DevOps access (for work items and PRs)

### Knowledge Requirements
- React 19 with TypeScript
- Material-UI component library
- Auth0 React SDK
- Apollo Client
- Constitution principles (read `.specify/memory/constitution.md`)

### Required Reading
1. **Constitution**: `.specify/memory/constitution.md` - Core principles
2. **Feature Spec**: `specs/001-auth-refactoring/spec.md` - Requirements
3. **Implementation Plan**: `specs/001-auth-refactoring/plan.md` - Technical approach
4. **Research**: `specs/001-auth-refactoring/research.md` - Decisions and rationale
5. **Data Model**: `specs/001-auth-refactoring/data-model.md` - Interfaces and validation
6. **Contracts**: `specs/001-auth-refactoring/contracts/` - API contracts

## Setup

### 1. Branch and Environment

```bash
# Ensure you're on the feature branch
git checkout 001-auth-refactoring

# Pull latest changes
git pull origin 001-auth-refactoring

# Install frontend dependencies
cd client
npm install

# Verify test framework
grep -E "(vitest|jest)" package.json

# Expected: vitest and @testing-library/react
```

### 2. Verify Test Setup

```bash
# Run existing tests to ensure baseline
npm test

# Should pass all existing tests before starting
```

### 3. Review Existing Code

```bash
# Key files to review before modifying
cat src/components/pages/SignInRedirectPage.tsx
cat src/components/auth/AuthButton.tsx
cat src/components/auth/Auth0TokenProvider.tsx
cat src/graphql/apollo-client.ts
cat src/hooks/user/useUserSync.ts

# Understand current auth flow
```

## Implementation Phases

**Note**: This quickstart uses simplified phase numbering (0-5). See tasks.md for detailed task breakdown (Phases 1-9 with 206 tasks).

### Phase 0: Preparation (Complete - see research.md)

✅ Research complete
✅ Decisions documented
✅ Constitution compliance verified

### Backend Implementation (tasks.md Phase 3) ⚠️ REQUIRED FIRST

**⚠️ CRITICAL PREREQUISITE**: Backend `verifyOrCreateUser` endpoint must be implemented BEFORE any frontend user story work.

**Purpose**: Implement server-authoritative authentication following MicroObjects layered architecture

See tasks.md Phase 3 (T015-T063) for detailed backend implementation tasks covering:
- **Entity definitions**: ArgEntity, ItrEntity, OufEntity, OutEntity (8 entity classes)
- **Aggregator service layer**: User verification aggregation with upsert logic (7 tasks)
- **Domain service layer**: Business logic validation and idempotency (6 tasks)
- **Entry service layer**: Validation, mapping, entry point (7 tasks)
- **GraphQL App layer**: Query, types, schema registration (10 tasks)
- **MSTest unit tests**: Aggregator, domain, entry tests per constitution (11 tasks)

**Expected Completion**: ~2-3 days for backend layer implementation

**Constitution Compliance**: Follows Principle II (Layered Architecture Flow) and Principle III (Test-First Development with MSTest)

**Deployment**: Backend must be deployed and accessible before frontend integration can proceed.

---

### Phase 1: Quick Win - Returning User Detection (tasks.md Phase 4)

**Goal**: Skip registration for returning users via localStorage

**Estimated Effort**: 3 files (1 new, 2 modified)

#### Steps:

1. **Create userStorage utility**

```bash
# Create new file
touch src/utils/userStorage.ts
```

Implement according to `contracts/utility-contracts.ts`:
- `getStoredUserData()`: Read and validate localStorage
- `isReturningUser()`: Check if user exists
- `saveUserData()`: Store user data
- `updateLastLogin()`: Update timestamp
- `clearUserData()`: Clear on logout

**Constitution Check**:
- ✅ TypeScript interface for `StoredUserData`
- ✅ Type guard for runtime validation
- ✅ Immutable update pattern
- ✅ Boundary validation (Principle IV)

2. **Modify SignInRedirectPage.tsx**

Add at start of `handleUserSetup()`:

```typescript
// Add imports
import {
  isReturningUser,
  saveUserData,
  updateLastLogin,
  type StoredUserData
} from '../../utils/userStorage';

// In handleUserSetup, BEFORE token wait:
if (isReturningUser(user.sub ?? '')) {
  logger.debug('Returning user detected, skipping registration');
  updateLastLogin();
  setSetupStatus('complete');
  setStatusMessage('Welcome back! Redirecting...');
  setTimeout(() => {
    navigate('/', { replace: true });
  }, 500);
  return;
}

// Existing logic continues...
```

**Constitution Check**:
- ✅ Early return (guard clause pattern)
- ✅ Clear status messages
- ✅ Immutable localStorage update

3. **Modify AuthButton.tsx**

Update logout handlers (2 locations, ~lines 74 and 102):

```typescript
// Add import
import { clearUserData } from '../../utils/userStorage';

// In logout onClick:
onClick={() => {
  clearUserData(); // NEW
  logout({ logoutParams: { returnTo: window.location.origin } });
}}
```

**Constitution Check**:
- ✅ Single responsibility (clear data before logout)
- ✅ No side effects

4. **Testing**

```bash
# Run tests
npm test

# Manual testing checklist:
# [ ] New user: Clear localStorage, login → see registration → redirect
# [ ] Returning user: Login → skip registration → fast redirect (< 1s)
# [ ] Logout: localStorage cleared
# [ ] Re-login: Treated as returning user (idempotent registration)
```

5. **Commit**

```bash
git add src/utils/userStorage.ts
git add src/components/pages/SignInRedirectPage.tsx
git add src/components/auth/AuthButton.tsx
git commit -m "feat(auth): add returning user detection via localStorage

- Create userStorage utility with type guards
- Skip registration for returning users
- Clear localStorage on logout
- Performance: < 1 second redirect for returning users

Constitution: Principle I (TypeScript interfaces), Principle IV (boundary validation)

Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>"
```

### Phase 2: Separate Auth Callback from Registration

**Goal**: Fast callback route for returning users

**Estimated Effort**: 4 files (2 new, 2 modified)

**Prerequisites**:
- Phase 1 complete and tested
- Auth0 dashboard access obtained

#### Steps:

1. **Create useAuthCallback hook**

```bash
mkdir -p src/hooks/auth
touch src/hooks/auth/useAuthCallback.ts
```

Implement according to `contracts/hook-contracts.ts`:
- Returns `AuthCallbackResult` with status and redirectTo
- Uses `useAuth0()` and `isReturningUser()`

2. **Create AuthCallbackPage**

```bash
touch src/components/pages/AuthCallbackPage.tsx
```

Features:
- Uses `useAuthCallback()` hook
- Shows loading/status messages
- Navigates based on status

**Constitution Check**:
- ✅ Material-UI sx props (not Tailwind)
- ✅ Proper TypeScript props interface
- ✅ Component composition

3. **Update Auth0 configuration**

In `src/main.tsx`:

```typescript
authorizationParams={{
  redirect_uri: `${window.location.origin}/auth/callback`, // Changed
  audience: "api://mtg-discovery",
  scope: "openid profile email offline_access"
}}
```

4. **Add route in App.tsx**

```typescript
// Add import
const AuthCallbackPage = lazy(() => import('./components/pages/AuthCallbackPage'));

// Add route BEFORE signin-redirect
<Route path="/auth/callback" element={
  <PageErrorBoundary name="AuthCallbackPage">
    <AuthCallbackPage />
  </PageErrorBoundary>
} />
```

5. **Update Auth0 Dashboard**

**IMPORTANT**: Do this BEFORE deploying!

1. Log into Auth0 Dashboard
2. Navigate to Applications → Your App → Settings
3. Add `https://yourdomain.com/auth/callback` to "Allowed Callback URLs"
4. Keep `/signin-redirect` during transition
5. Save changes

6. **Testing**

```bash
# Run tests
npm test

# Manual testing:
# [ ] /auth/callback routes returning users to /
# [ ] /auth/callback routes new users to /signin-redirect
# [ ] Direct navigation to /signin-redirect still works
```

7. **Commit**

```bash
git add src/hooks/auth/useAuthCallback.ts
git add src/components/pages/AuthCallbackPage.tsx
git add src/main.tsx
git add src/App.tsx
git commit -m "feat(auth): separate auth callback from registration

- Create useAuthCallback hook for routing logic
- Add AuthCallbackPage for fast callback processing
- Update Auth0 redirect URI to /auth/callback
- Route returning users directly to home

Constitution: Principle II (React layering), Principle VI (MUI sx props)

Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>"
```

### Phase 3: Simplify Token Readiness

**Goal**: Replace global subscription with component-local hooks

**Estimated Effort**: 3 files (1 new, 2 modified)

#### Steps:

1. **Create useTokenReady hook**

```bash
touch src/hooks/auth/useTokenReady.ts
```

Implement according to `contracts/hook-contracts.ts`.

2. **Simplify Auth0TokenProvider**

Remove lines 11-56 (subscription system), keep only:
- Token getter registration
- Error handling

**Constitution Check**:
- ✅ Removes complexity
- ✅ Component-local state
- ✅ No global subscriptions

3. **Update apollo-client.ts**

Remove subscription-related code, keep:
- `let getAuth0Token`
- `export const setAuth0TokenGetter`
- authLink usage (unchanged)

4. **Testing**

Test useTokenReady in components that need it.

5. **Commit**

```bash
git add src/hooks/auth/useTokenReady.ts
git add src/components/auth/Auth0TokenProvider.tsx
git add src/graphql/apollo-client.ts
git commit -m "feat(auth): simplify token management with component-local hooks

- Create useTokenReady hook for component-local verification
- Remove global BehaviorSubject subscription system
- Eliminate token subscription race conditions

Constitution: Simplification principle, component-local state

Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>"
```

### Phase 4: Improve User State Caching

**Goal**: Use Apollo cache-first to avoid redundant requests

**Estimated Effort**: 2 files (0 new, 2 modified)

#### Steps:

1. **Update useUserSync query**

```typescript
const { data, loading, error, refetch } = useQuery<UserInfoQueryData>(GET_USER_INFO, {
  skip: shouldQueryUserInfo === false,
  fetchPolicy: 'cache-first',  // NEW
  nextFetchPolicy: 'cache-and-network',  // NEW
  errorPolicy: 'all'
});
```

2. **Add cache type policy**

In `apollo-client.ts`:

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

3. **Testing**

Monitor cache hit rates in browser dev tools (Apollo tab).

4. **Commit**

```bash
git add src/hooks/user/useUserSync.ts
git add src/graphql/apollo-client.ts
git commit -m "perf(auth): optimize user info caching with cache-first policy

- Use cache-first fetch policy for GET_USER_INFO
- Add Apollo cache type policy for userInfo
- Reduce network requests for returning users

Constitution: Performance optimization, no behavioral changes

Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>"
```

### Phase 5: Clean Up Storage Keys

**Goal**: Centralize localStorage constants

**Estimated Effort**: 1 file (1 new) + updates

#### Steps:

1. **Create storageKeys.ts**

```bash
touch src/utils/storageKeys.ts
```

```typescript
export const STORAGE_KEYS = {
  USER_DATA: 'mtg-user-data',
  LANGUAGE: 'mtg-discovery-language',
  VISITED: 'mtg-discovery-visited',
  CARD_SIZE: 'mtg-card-size-preference',
} as const;

export type StorageKey = typeof STORAGE_KEYS[keyof typeof STORAGE_KEYS];
```

2. **Update references**

- `src/utils/userStorage.ts`
- `src/hooks/useCardSizePreference.ts`
- `src/hooks/useLanguageDetection.ts`

3. **Commit**

```bash
git add src/utils/storageKeys.ts
# ... other files
git commit -m "refactor(storage): centralize localStorage key constants

- Create STORAGE_KEYS constant object
- Update all localStorage access to use constants
- Type-safe storage key references

Constitution: Consistency, maintainability

Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>"
```

## Quality Gates

### Pre-Commit Checklist

Before each commit:
- [ ] All tests pass (`npm test`)
- [ ] Build succeeds (`npm run build`)
- [ ] No TypeScript errors (`npm run type-check` or tsc --noEmit)
- [ ] Linter passes (`npm run lint`)
- [ ] Constitution principles followed (review checklist)

### Pre-PR Checklist

Before creating PR:
- [ ] All phases complete and committed
- [ ] Manual testing completed (see Testing section)
- [ ] Constitution re-evaluation complete (see below)
- [ ] Azure DevOps work item created/linked
- [ ] PR template filled out

## Testing

### Unit Tests (Per Phase)

```bash
# Run tests for specific files
npm test -- userStorage.test.ts
npm test -- useAuthCallback.test.ts
npm test -- AuthCallbackPage.test.ts
```

### Integration Testing

```bash
# Full test suite
npm test

# With coverage
npm test -- --coverage
```

### Manual Testing Scenarios

**Scenario 1: New User**
1. Clear localStorage (`localStorage.clear()` in console)
2. Click login
3. Complete Auth0 login
4. Verify: See registration flow
5. Verify: Redirected to home
6. Verify: localStorage has user data

**Scenario 2: Returning User (Phase 1+)**
1. Have localStorage from Scenario 1
2. Click logout
3. Click login again
4. Verify: Skip registration, fast redirect (< 1s)
5. Verify: "Welcome back!" message shown

**Scenario 3: Different User**
1. Have localStorage for user A
2. Login with user B (different Auth0 account)
3. Verify: Registration flow runs
4. Verify: User A's data cleared, user B's data saved

**Scenario 4: Logout Clears Data**
1. Login as any user
2. Verify localStorage has data
3. Click logout
4. Verify localStorage cleared
5. Next login should be registration flow

**Scenario 5: Auth0 Callback Routing (Phase 2+)**
1. New user flow: `/auth/callback` → `/signin-redirect` → `/`
2. Returning user flow: `/auth/callback` → `/`
3. Direct navigation to `/signin-redirect` requires auth

## Constitution Re-Evaluation

After Phase 1 design complete, re-check:

### Principle I (MicroObjects → TypeScript)
- ✅ `StoredUserData` interface defined
- ✅ `AuthCallbackResult` interface defined
- ✅ `TokenReadyState` interface defined
- ✅ All types explicit (no `any`)

### Principle III (Test-First Development)
- ✅ Test framework confirmed (Vitest)
- ✅ Tests written for new utilities
- ✅ Tests written for new hooks
- ✅ Tests written for new components
- ✅ All tests pass

### Principle IV (Null Boundary Guards)
- ✅ Type guards for localStorage validation
- ✅ Runtime checks at Auth0 integration
- ✅ Interior code assumes validated data
- ✅ Clear error handling

### Principle VI (Code Style)
- ✅ Material-UI sx props (no Tailwind)
- ✅ TypeScript interfaces for all props
- ✅ Atomic design structure followed
- ✅ Generated GraphQL types used

**Final Gate**: All principles pass → Ready for implementation

## Rollout Strategy

### Week 1: Phase 1 Only
```bash
# Deploy Phase 1 to production
git push origin 001-auth-refactoring

# Monitor metrics:
# - Returning user redirect time (should be < 1s)
# - Error rates (should not increase)
# - localStorage usage (verify data persisting)
```

### Week 2: Phases 3-5
```bash
# Deploy optimizations
# Monitor cache hit rates
# Verify no token race conditions
```

### Week 3: Phase 2
```bash
# Coordinate with infrastructure team
# Update Auth0 dashboard FIRST
# Then deploy Phase 2
# Monitor callback routing
```

## Troubleshooting

### Issue: localStorage data not persisting
- Check browser settings (cookies/storage enabled)
- Verify JSON serialization is correct
- Check for quota exceeded errors in console

### Issue: Returning user still seeing registration
- Check sub mismatch (different Auth0 account)
- Verify localStorage data structure matches interface
- Check type guard validation logic

### Issue: Token ready hook not working
- Verify Auth0 SDK is loaded (`useAuth0().isLoading`)
- Check audience configuration matches
- Review error messages in console

### Issue: Apollo cache not hitting
- Verify `fetchPolicy: 'cache-first'` is set
- Check type policy is registered
- Review Apollo DevTools cache inspector

## Resources

- **Constitution**: `.specify/memory/constitution.md`
- **Auth0 React SDK**: https://auth0.com/docs/libraries/auth0-react
- **Apollo Client Caching**: https://www.apollographql.com/docs/react/caching/cache-configuration/
- **Material-UI**: https://mui.com/material-ui/getting-started/
- **Vitest**: https://vitest.dev/guide/
- **React Testing Library**: https://testing-library.com/docs/react-testing-library/intro/

## Next Steps

After `/speckit.plan` completion:
1. Run `/speckit.tasks` to generate task breakdown
2. Execute tasks following this quickstart guide
3. Create Azure DevOps work items
4. Submit PR when all phases complete
5. Run `/speckit.analyze` for consistency check

**Ready to implement!** 🚀
