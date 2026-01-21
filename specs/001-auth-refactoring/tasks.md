# Tasks: Authentication Flow Refactoring

**Input**: Design documents from `/specs/001-auth-refactoring/`
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/, quickstart.md

**Tests**: Backend uses MSTest + AwesomeAssertions (constitution required). Frontend uses manual testing scenarios in quickstart.md.

**Organization**: Tasks grouped by implementation phase (Setup → Backend → User Stories). Backend must complete before frontend integration.

## Format: `[ID] [P?] [Layer] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Backend]**: Backend .NET implementation task
- **[US#]**: Frontend user story task (e.g., US1, US2, US3)
- **[Backend Test]**: Backend test task (MSTest)
- Include exact file paths in descriptions

## Path Conventions

This is a full-stack web application with backend and frontend changes:
- **Backend**: `src/` (.NET 9.0 C# with MicroObjects architecture)
- **Frontend**: `client/src/` (React 19 TypeScript application)
- **Backend Tests**: `src/*.Tests/` (MSTest with AwesomeAssertions)
- **Frontend Tests**: Manual testing via browser (see quickstart.md)
- All paths relative to repository root

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Verify environment and prepare for implementation

- [ ] T001 Verify Node.js 18+ and npm are installed
- [ ] T002 [P] Navigate to client/ directory and run `npm install` to ensure all dependencies are current
- [ ] T003 [P] Verify Vitest testing framework exists in client/package.json
- [ ] T004 [P] Verify Auth0 environment variables configured (VITE_AUTH0_DOMAIN, VITE_AUTH0_CLIENT_ID, VITE_AUTH0_AUDIENCE)
- [ ] T005 [P] Run `npm run dev` to verify development server starts successfully
- [ ] T006 [P] Verify GraphQL codegen configured: check client/codegen.ts exists

**Checkpoint**: Development environment ready - feature implementation can begin

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Verify existing infrastructure that all user stories depend on

**⚠️ CRITICAL**: These checks must pass before ANY user story implementation begins

- [ ] T007 Verify client/src/components/auth/Auth0TokenProvider.tsx exists and provides Auth0 context
- [ ] T008 [P] Verify client/src/components/auth/AuthButton.tsx exists with logout functionality
- [ ] T009 [P] Verify client/src/components/pages/SignInRedirectPage.tsx exists with registration flow
- [ ] T010 [P] Verify client/src/graphql/apollo-client.ts exists with Apollo Client setup
- [ ] T011 [P] Verify client/src/hooks/user/useUserSync.ts exists with user query logic
- [ ] T012 [P] Verify client/src/main.tsx exists with Auth0Provider configuration
- [ ] T013 [P] Verify client/src/App.tsx exists with routing configuration
- [ ] T014 Read existing localStorage usage patterns in codebase for consistency

**Checkpoint**: Foundation verified - user story implementation can now begin in priority order

---

## Phase 3: Backend - User Verification Endpoint (BLOCKING)

**Purpose**: Implement server-authoritative authentication with GraphQL `verifyOrCreateUser` query following MicroObjects layered architecture

**⚠️ CRITICAL**: This phase MUST complete before frontend can call backend verification endpoint (Phases 4+)

**Constitution Compliance**: Follows Principle II (Layered Architecture Flow) with strict 7-layer pattern

### Backend Entity Definitions

- [ ] T015 [Backend] Create `src/Lib.Shared.DataModels/Entities/Args/IVerifyOrCreateUserArgEntity.cs` interface with Auth0Sub property
- [ ] T016 [Backend] Create `src/Lib.Shared.DataModels/Entities/Args/VerifyOrCreateUserArgEntity.cs` sealed class implementing IVerifyOrCreateUserArgEntity
- [ ] T017 [Backend] Create `src/Lib.Shared.DataModels/Entities/Itrs/IUserVerificationItrEntity.cs` interface per spec FR-002 (userId, needsOnboarding, auth0Sub, displayName, email, lastVerifiedAt)
- [ ] T018 [Backend] Create `src/Lib.Shared.DataModels/Entities/Itrs/UserVerificationItrEntity.cs` sealed class implementing IUserVerificationItrEntity
- [ ] T019 [Backend] Create `src/Lib.Shared.DataModels/Entities/Oufs/IUserVerificationOufEntity.cs` interface (same fields as ItrEntity for this query)
- [ ] T020 [Backend] Create `src/Lib.Shared.DataModels/Entities/Oufs/UserVerificationOufEntity.cs` sealed class implementing IUserVerificationOufEntity
- [ ] T021 [Backend] Create `src/Lib.Shared.DataModels/Entities/Outs/IUserVerificationOutEntity.cs` interface for GraphQL response
- [ ] T022 [Backend] Create `src/Lib.Shared.DataModels/Entities/Outs/UserVerificationOutEntity.cs` sealed class implementing IUserVerificationOutEntity

### Aggregator Layer

- [ ] T023 [Backend] Create `src/Lib.Aggregator.User/IUserVerificationAggregatorService.cs` interface with method `Task<IOperationResponse<IUserVerificationOufEntity>> VerifyOrCreateUserAsync(IUserVerificationItrEntity input)`
- [ ] T024 [Backend] Create `src/Lib.Aggregator.User/UserVerificationAggregatorService.cs` sealed class implementing IUserVerificationAggregatorService
- [ ] T025 [Backend] Implement VerifyOrCreateUserAsync in UserVerificationAggregatorService: Call user adapter to check if user exists by auth0Sub
- [ ] T026 [Backend] Add logic to UserVerificationAggregatorService: If user exists, return existing user data with needsOnboarding=false
- [ ] T027 [Backend] Add logic to UserVerificationAggregatorService: If user doesn't exist, create user record via adapter, return with needsOnboarding=true (FR-004, FR-005)
- [ ] T028 [Backend] Ensure UserVerificationAggregatorService returns IOperationResponse<IUserVerificationOufEntity> with success/failure states
- [ ] T029 [Backend] Register IUserVerificationAggregatorService in DI container (Startup.cs or service registration file)

### Domain Layer

- [ ] T030 [Backend] Create `src/Lib.Domain.User/IUserVerificationDomainService.cs` interface with method `Task<IOperationResponse<IUserVerificationOufEntity>> VerifyOrCreateUserAsync(IUserVerificationItrEntity input)`
- [ ] T031 [Backend] Create `src/Lib.Domain.User/UserVerificationDomainService.cs` sealed class implementing IUserVerificationDomainService
- [ ] T032 [Backend] Inject IUserVerificationAggregatorService into UserVerificationDomainService constructor
- [ ] T033 [Backend] Implement VerifyOrCreateUserAsync in UserVerificationDomainService: Apply ALWAYS rules (business logic validation)
- [ ] T034 [Backend] Ensure idempotency in domain service: Same auth0Sub always returns same userId (FR-003)
- [ ] T035 [Backend] Register IUserVerificationDomainService in DI container

### Entry Layer

- [ ] T036 [Backend] Create `src/Lib.MtgDiscovery.Entry/IUserVerificationEntryService.cs` interface with method `Task<IOperationResponse<IUserVerificationOutEntity>> VerifyOrCreateUserAsync(IVerifyOrCreateUserArgEntity input)`
- [ ] T037 [Backend] Create `src/Lib.MtgDiscovery.Entry/UserVerificationEntryService.cs` sealed class implementing IUserVerificationEntryService
- [ ] T038 [Backend] Inject IUserVerificationDomainService into UserVerificationEntryService constructor
- [ ] T039 [Backend] Create validator for IVerifyOrCreateUserArgEntity checking Auth0Sub is non-null and non-empty (Principle IV: Null Boundary Guards)
- [ ] T040 [Backend] Implement VerifyOrCreateUserAsync in UserVerificationEntryService: Validate ArgEntity, map to ItrEntity, call domain service
- [ ] T041 [Backend] Map IUserVerificationOufEntity response from domain to IUserVerificationOutEntity for GraphQL layer
- [ ] T042 [Backend] Register IUserVerificationEntryService in DI container

### GraphQL App Layer

- [ ] T043 [Backend] Create `src/App.MtgDiscovery.GraphQL/Entities/Types/UserVerificationOutEntityType.cs` ObjectType class for UserVerificationOutEntity
- [ ] T044 [Backend] Create `src/App.MtgDiscovery.GraphQL/Entities/Types/UserVerificationSuccessDataResponseModelType.cs` ObjectType class for success response
- [ ] T045 [Backend] Create `src/App.MtgDiscovery.GraphQL/Entities/Types/UserVerificationResponseUnionType.cs` UnionType class (NOT UnionType<T>) per constitution GraphQL patterns
- [ ] T046 [Backend] Create `src/App.MtgDiscovery.GraphQL/Queries/UserVerificationQuery.cs` with [ExtendObjectType] attribute (FR-001)
- [ ] T047 [Backend] Implement `verifyOrCreateUser` query method in UserVerificationQuery: Extract Auth0Sub from ClaimsPrincipal (FR-001, FR-007 JWT validation)
- [ ] T048 [Backend] Add [Authorize] attribute to verifyOrCreateUser query method for JWT authentication requirement
- [ ] T049 [Backend] Inject IUserVerificationEntryService into UserVerificationQuery
- [ ] T050 [Backend] Call entry service from query, map OutEntity to GraphQL response type, return union type (success/failure)
- [ ] T051 [Backend] Register all GraphQL types in schema: AddType<UserVerificationOutEntityType>(), AddType<UserVerificationSuccessDataResponseModelType>(), AddType<UserVerificationResponseUnionType>()
- [ ] T052 [Backend] Register UserVerificationQuery in GraphQL schema configuration (FR-001)

### Backend Testing (Constitution Principle III)

- [ ] T053 [Backend Test] Create `src/Lib.Aggregator.User.Tests/UserVerificationAggregatorServiceTests.cs` with MSTest
- [ ] T054 [Backend Test] Write test: VerifyOrCreateUserAsync_NewUser_ReturnsNeedsOnboardingTrue (FR-005)
- [ ] T055 [Backend Test] Write test: VerifyOrCreateUserAsync_ExistingUser_ReturnsNeedsOnboardingFalse (FR-006)
- [ ] T056 [Backend Test] Write test: VerifyOrCreateUserAsync_SameAuth0Sub_ReturnsConsistentUserId (FR-003 idempotency)
- [ ] T057 [Backend Test] Create `src/Lib.Domain.User.Tests/UserVerificationDomainServiceTests.cs` with MSTest
- [ ] T058 [Backend Test] Write domain service tests following Arrange-Act-Assert pattern per constitution
- [ ] T059 [Backend Test] Create `src/Lib.MtgDiscovery.Entry.Tests/UserVerificationEntryServiceTests.cs` with MSTest
- [ ] T060 [Backend Test] Write test: VerifyOrCreateUserAsync_NullAuth0Sub_ReturnsValidationError (Principle IV)
- [ ] T061 [Backend Test] Write test: VerifyOrCreateUserAsync_ValidInput_MapsToItrEntity
- [ ] T062 [Backend Test] Verify all fake invocation counts in tests per constitution testing requirements
- [ ] T063 [Backend Test] Run `dotnet test` and verify all backend tests pass

**Checkpoint**: Backend `verifyOrCreateUser` endpoint implemented, tested, and ready for frontend integration

---

## Phase 4: User Story 1 - Returning User Fast Authentication (Priority: P1) 🎯 MVP

**Goal**: Skip registration for returning users via localStorage detection, achieving < 1 second redirect

**Independent Test**: Manual test in browser:
1. Login as new user (should see registration)
2. Logout
3. Login again (should skip registration, see "Welcome back!", redirect < 1s)
4. Verify localStorage contains user data
5. Measure redirect time from Auth0 callback to home page

### Implementation for User Story 1

**Step 0: Frontend GraphQL Query Definition**

- [ ] T064 [US1] Create directory client/src/graphql/queries/ if it doesn't exist
- [ ] T065 [US1] Create client/src/graphql/queries/user-verification.graphql with GraphQL query definition
- [ ] T066 [US1] Define verifyOrCreateUser query in user-verification.graphql matching FR-002 response schema (query with __typename, userId, needsOnboarding, auth0Sub, displayName, email, lastVerifiedAt fields, union type with FailureResponseModel)
- [ ] T067 [US1] Run `npm run codegen` from client/ directory to generate TypeScript types and hooks for verifyOrCreateUser query
- [ ] T068 [US1] Verify generated file client/src/generated/graphql.ts contains VerifyOrCreateUserQuery and useVerifyOrCreateUserQuery hook

**Step 1: Create Backend Verification Hook**

- [ ] T069 [US1] Create directory client/src/hooks/auth/ if it doesn't exist
- [ ] T070 [US1] Create client/src/hooks/auth/useBackendVerification.ts with empty file and imports
- [ ] T071 [US1] Import generated useVerifyOrCreateUserQuery hook from src/generated/graphql
- [ ] T072 [US1] Define BackendVerificationResult interface in useBackendVerification.ts per contracts/hook-contracts.ts (status: 'loading' | 'success' | 'error' | 'cached', userData, error)
- [ ] T073 [US1] Implement useBackendVerification hook: Check cache freshness first (< 24 hours per FR-014)
- [ ] T074 [US1] Add logic: If cache fresh, return cached data immediately with status 'cached' (FR-014)
- [ ] T075 [US1] Add logic: If cache stale or missing, call useVerifyOrCreateUserQuery (FR-008, FR-015)
- [ ] T076 [US1] Add logic: On successful query response, check for userId conflict (FR-016)
- [ ] T077 [US1] Compare backend response userId with cached localStorage userId (if cache exists)
- [ ] T078 [US1] If userIds don't match (account switch detected), call clearUserData() before saving new data (FR-016, FR-017)
- [ ] T079 [US1] Save backend response to localStorage via saveUserData() (FR-013)
- [ ] T080 [US1] Add logic: On query error, gracefully degrade to cached data if available (FR-023, FR-025)
- [ ] T081 [US1] Add logic: If no cache and query fails, return error status (FR-027)
- [ ] T082 [US1] Return BackendVerificationResult with appropriate status, userData, and error fields

**Step 2: Create Error Handling UI Components**

**Purpose**: Implement UI components for error states per FR-024 through FR-027

- [ ] T083 [US1] Create client/src/components/molecules/auth/VerificationLoadingState.tsx component
- [ ] T084 [US1] Implement VerificationLoadingState: Display "Verifying your account..." message with Material-UI CircularProgress (FR-024)
- [ ] T085 [US1] Add centered layout with theme.spacing for consistent appearance
- [ ] T086 [US1] Export VerificationLoadingState for use in AuthCallbackPage and other auth flows
- [ ] T087 [US1] Create client/src/components/molecules/auth/VerificationWarningBanner.tsx component
- [ ] T088 [US1] Implement VerificationWarningBanner: Display warning Alert with message "Unable to verify your account. You may see outdated information." (FR-025)
- [ ] T089 [US1] Add manual retry Button to VerificationWarningBanner with onClick prop (FR-026)
- [ ] T090 [US1] Style banner with Material-UI Alert severity="warning" and action slot for retry button
- [ ] T091 [US1] Export VerificationWarningBanner with props interface: { onRetry: () => void }
- [ ] T092 [US1] Create client/src/components/molecules/auth/VerificationErrorState.tsx component for blocked access scenario
- [ ] T093 [US1] Implement VerificationErrorState: Display error Alert with message and retry button (FR-027)
- [ ] T094 [US1] Add error message prop to show specific failure reason (network error, auth error, etc.)
- [ ] T095 [US1] Block access to app content when VerificationErrorState is shown (no navigation allowed)
- [ ] T096 [US1] Export VerificationErrorState with props interface: { error: string, onRetry: () => void }

**Step 3: Integrate Error Components**

- [ ] T097 [US1] Update useBackendVerification hook (T070-T082): Return additional field `component: 'loading' | 'warning' | 'error' | null` to indicate which UI to show
- [ ] T098 [US1] Modify T072 BackendVerificationResult interface to include component field
- [ ] T099 [US1] Add logic to determine component value based on status: loading→'loading', error with cache→'warning', error without cache→'error', success→null
- [ ] T100 [US1] Import VerificationLoadingState, VerificationWarningBanner, VerificationErrorState into files that use useBackendVerification
- [ ] T101 [US1] Wire retry button onClick handlers to refetch backend verification query

**Step 4: Automatic Retry on Next Login (FR-028)**

**Purpose**: Implement automatic retry of failed backend verification on subsequent login attempts

- [ ] T102 [US1] Add `lastVerificationFailed: boolean` field to StoredUserData interface in data-model.md (FR-028)
- [ ] T103 [US1] Update userStorage.ts saveUserData(): Accept optional `verificationFailed` parameter to store failure state (FR-028)
- [ ] T104 [US1] Update useBackendVerification hook: Check localStorage for lastVerificationFailed flag on component mount (FR-028)
- [ ] T105 [US1] If lastVerificationFailed is true, force backend verification regardless of cache age (bypass 24h freshness check) (FR-028)
- [ ] T106 [US1] On successful verification response, clear lastVerificationFailed flag in localStorage (FR-028)
- [ ] T107 [US1] On verification error, set lastVerificationFailed: true in localStorage before showing error UI (FR-028)

**Step 5: Create Storage Utility (Foundation)**

- [ ] T108 [US1] Create directory client/src/utils/ if it doesn't exist
- [ ] T109 [US1] Create client/src/utils/userStorage.ts with empty file and imports
- [ ] T110 [US1] Define StoredUserData interface in client/src/utils/userStorage.ts per data-model.md lines 21-38
- [ ] T111 [US1] Define storage key constant USER_STORAGE_KEY = 'mtg-user-data' in client/src/utils/userStorage.ts
- [ ] T112 [US1] Implement isValidStoredUserData() type guard in client/src/utils/userStorage.ts per contracts/utility-contracts.ts lines 63-77 (FR-019)
- [ ] T113 [US1] Implement getStoredUserData() function in client/src/utils/userStorage.ts per contracts/utility-contracts.ts lines 45-60
- [ ] T114 [US1] Implement isReturningUser(auth0Sub) function in client/src/utils/userStorage.ts per contracts/utility-contracts.ts lines 79-95
- [ ] T115 [US1] Implement saveUserData(data) function in client/src/utils/userStorage.ts per contracts/utility-contracts.ts lines 97-113
- [ ] T116 [US1] Implement updateLastLogin() function in client/src/utils/userStorage.ts per contracts/utility-contracts.ts lines 115-134
- [ ] T117 [US1] Implement clearUserData() function in client/src/utils/userStorage.ts per contracts/utility-contracts.ts lines 136-150

**Step 5: Modify SignInRedirectPage for Returning Users**

- [ ] T118 [US1] Add imports to client/src/components/pages/SignInRedirectPage.tsx: import { isReturningUser, saveUserData, updateLastLogin } from '../../utils/userStorage'
- [ ] T119 [US1] Modify handleUserSetup() in client/src/components/pages/SignInRedirectPage.tsx: Add returning user check at start (before token wait) per quickstart.md lines 117-136
- [ ] T120 [US1] Add early return for returning users in client/src/components/pages/SignInRedirectPage.tsx: updateLastLogin(), setStatusMessage('Welcome back!'), navigate('/') (FR-010, FR-011)
- [ ] T121 [US1] Modify successful registration handler in client/src/components/pages/SignInRedirectPage.tsx: Call saveUserData() after REGISTER_USER mutation succeeds
- [ ] T122 [US1] Add error handling in client/src/components/pages/SignInRedirectPage.tsx: Only save to localStorage if registration API call succeeds

**Step 6: Modify AuthButton for Logout Cleanup**

- [ ] T123 [US1] Add import to client/src/components/auth/AuthButton.tsx: import { clearUserData } from '../../utils/userStorage'
- [ ] T124 [US1] Modify mobile logout button onClick in client/src/components/auth/AuthButton.tsx (around line 74): Add clearUserData() call before logout() per quickstart.md lines 152-158 (FR-018)
- [ ] T125 [US1] Modify desktop logout button onClick in client/src/components/auth/AuthButton.tsx (around line 102): Add clearUserData() call before logout() (FR-018)

**Step 7: Manual Testing**

- [ ] T126 [US1] Manual test: Clear localStorage, login as new user, verify registration flow runs
- [ ] T127 [US1] Manual test: Logout, login again, verify "Welcome back!" message appears
- [ ] T128 [US1] Manual test: Verify redirect happens in < 1 second (use browser dev tools Network tab) (FR-010)
- [ ] T129 [US1] Manual test: Check localStorage has 'mtg-user-data' key with correct structure
- [ ] T130 [US1] Manual test: Logout, verify localStorage 'mtg-user-data' is cleared
- [ ] T131 [US1] Manual test: Login with different Auth0 account, verify old data cleared and new data saved

**Checkpoint**: User Story 1 complete - Returning users get fast authentication

**Commit Message**:
```
feat(auth): add returning user detection via localStorage

- Create userStorage utility with type guards
- Skip registration for returning users
- Clear localStorage on logout
- Performance: < 1 second redirect for returning users

Implements User Story 1 (P1): Returning User Fast Authentication

Constitution: Principle I (TypeScript interfaces), Principle IV (boundary validation)

Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>
```

---

## Phase 5: User Story 2 - New User Registration Flow (Priority: P1)

**Goal**: Separate Auth0 callback handling from registration flow for routing clarity

**Independent Test**: Manual test in browser:
1. Clear localStorage
2. Login with new Auth0 account
3. Verify routes to /auth/callback first
4. Verify then routes to /signin-redirect for registration
5. Verify registration completes successfully
6. Verify subsequent logins skip registration (fast path)

**Dependency**: Requires User Story 1 (userStorage utility must exist)

### Implementation for User Story 2

**Step 1: Create useAuthCallback Hook**

- [ ] T132 [US2] Create directory client/src/hooks/auth/ if it doesn't exist
- [ ] T133 [US2] Create client/src/hooks/auth/useAuthCallback.ts with empty file and imports
- [ ] T134 [US2] Define CallbackStatus type in client/src/hooks/auth/useAuthCallback.ts per contracts/hook-contracts.ts line 30
- [ ] T135 [US2] Define AuthCallbackResult interface in client/src/hooks/auth/useAuthCallback.ts per contracts/hook-contracts.ts lines 37-43
- [ ] T136 [US2] Implement useAuthCallback() hook in client/src/hooks/auth/useAuthCallback.ts per contracts/hook-contracts.ts lines 45-72
- [ ] T137 [US2] Add state management in useAuthCallback: useState for status and redirectTo
- [ ] T138 [US2] Add useEffect in useAuthCallback: Check Auth0 state, check localStorage, determine routing
- [ ] T139 [US2] Return AuthCallbackResult from useAuthCallback hook

**Step 2: Create AuthCallbackPage Component**

- [ ] T140 [US2] Create client/src/components/pages/AuthCallbackPage.tsx with empty file and imports
- [ ] T141 [US2] Define AuthCallbackPageProps interface in client/src/components/pages/AuthCallbackPage.tsx per contracts/component-contracts.ts lines 31-33
- [ ] T142 [US2] Implement AuthCallbackPage component in client/src/components/pages/AuthCallbackPage.tsx per contracts/component-contracts.ts lines 35-63
- [ ] T143 [US2] Add useAuthCallback() hook call in AuthCallbackPage
- [ ] T144 [US2] Add useNavigate() hook call in AuthCallbackPage
- [ ] T145 [US2] Implement useEffect in AuthCallbackPage: Navigate when status changes per quickstart.md lines 225-227
- [ ] T146 [US2] Add loading indicator in AuthCallbackPage: CircularProgress from MUI while processing
- [ ] T147 [US2] Add status messages in AuthCallbackPage: "Completing sign-in..." or "Welcome back!" per contracts/component-contracts.ts lines 46-49
- [ ] T148 [US2] Style AuthCallbackPage with Material-UI sx props (centered layout, proper spacing)

**Step 3: Integrate Backend Verification into AuthCallbackPage**

- [ ] T149 [US2] Import useBackendVerification hook into client/src/components/pages/AuthCallbackPage.tsx
- [ ] T150 [US2] Call useBackendVerification() at start of AuthCallbackPage component to get user verification status
- [ ] T151 [US2] Add conditional routing logic in AuthCallbackPage: If status === 'cached' or 'success' and needsOnboarding === false, navigate to '/' (returning user path per FR-009)
- [ ] T152 [US2] Add conditional routing logic: If needsOnboarding === true, navigate to '/signin-redirect' (new user registration path per FR-009, FR-012)
- [ ] T153 [US2] Replace existing loading indicator with conditional display: If component === 'loading', show VerificationLoadingState component per FR-024
- [ ] T154 [US2] Add conditional error handling: If component === 'error', show VerificationErrorState component per FR-027
- [ ] T155 [US2] Add conditional warning display: If component === 'warning', show VerificationWarningBanner component per FR-025

**Step 4: Update Routing Configuration**

- [ ] T156 [US2] Update client/src/main.tsx: Change Auth0Provider redirect_uri to `${window.location.origin}/auth/callback` per quickstart.md lines 238-245
- [ ] T157 [US2] Update client/src/App.tsx: Add lazy import for AuthCallbackPage per quickstart.md line 251
- [ ] T158 [US2] Update client/src/App.tsx: Add /auth/callback route with AuthCallbackPage wrapped in PageErrorBoundary per quickstart.md lines 254-258
- [ ] T159 [US2] Verify client/src/App.tsx: Ensure /auth/callback route is BEFORE /signin-redirect route in routing config

**Step 5: Update Auth0 Dashboard Configuration**

- [ ] T160 [US2] Access Auth0 Dashboard and navigate to Applications → Your App → Settings
- [ ] T161 [US2] Add production callback URL to "Allowed Callback URLs": https://yourdomain.com/auth/callback
- [ ] T162 [US2] Keep existing /signin-redirect URL during transition period
- [ ] T163 [US2] Save Auth0 configuration changes

**Step 6: Manual Testing**

- [ ] T164 [US2] Manual test: Login as new user, verify /auth/callback route loads
- [ ] T165 [US2] Manual test: Verify new users route from /auth/callback → /signin-redirect
- [ ] T166 [US2] Manual test: Verify returning users route from /auth/callback → / (home)
- [ ] T167 [US2] Manual test: Direct navigation to /signin-redirect still works (requires auth)
- [ ] T168 [US2] Manual test: Verify "Welcome back!" message shows for returning users
- [ ] T169 [US2] Manual test: Complete registration flow end-to-end, verify data saves

**Checkpoint**: User Story 2 complete - Callback routing separates returning/new users

**Commit Message**:
```
feat(auth): separate auth callback from registration

- Create useAuthCallback hook for routing logic
- Add AuthCallbackPage for fast callback processing
- Update Auth0 redirect URI to /auth/callback
- Route returning users directly to home
- Route new users to /signin-redirect for registration

Implements User Story 2 (P1): New User Registration Flow

Constitution: Principle II (React layering), Principle VI (MUI sx props)

Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>
```

---

## Phase 6: User Story 3 - Session Termination and Data Cleanup (Priority: P2)

**Goal**: Ensure session data is properly cleared on logout for security and multi-account support

**Independent Test**: Manual test in browser:
1. Login as any user
2. Verify localStorage has 'mtg-user-data'
3. Click logout
4. Verify localStorage 'mtg-user-data' is cleared
5. Login with different Auth0 account
6. Verify old user data is gone, new user data is present

**Dependency**: Requires User Story 1 (clearUserData function exists)

### Implementation for User Story 3

**Note**: Core functionality already implemented in User Story 1 (T123-T125). This phase adds additional validation and edge case handling.

**Step 1: Add Sub Mismatch Detection**

- [ ] T170 [US3] Add validateUserSub(auth0Sub) function to client/src/utils/userStorage.ts that checks if stored sub matches current user
- [ ] T171 [US3] Modify client/src/components/pages/SignInRedirectPage.tsx: Add sub validation check before returning user fast path
- [ ] T172 [US3] Add logic in SignInRedirectPage.tsx: If sub mismatch detected, call clearUserData() and proceed with registration

**Step 2: Add Corrupted Data Detection**

- [ ] T173 [US3] Modify getStoredUserData() in client/src/utils/userStorage.ts: Add try/catch for JSON.parse errors
- [ ] T174 [US3] Modify getStoredUserData() in client/src/utils/userStorage.ts: If data is corrupted, call clearUserData() automatically and return null
- [ ] T175 [US3] Add logging in client/src/utils/userStorage.ts: Log when corrupted data is detected and cleared

**Step 3: Manual Testing**

- [ ] T176 [US3] Manual test: Login, verify data stored
- [ ] T177 [US3] Manual test: Logout, verify data cleared
- [ ] T178 [US3] Manual test: Manually corrupt localStorage data (invalid JSON), reload page, verify auto-cleanup
- [ ] T179 [US3] Manual test: Login with user A, logout, login with user B, verify user A data is gone
- [ ] T180 [US3] Manual test: Login with user A, manually change 'sub' in localStorage to different value, reload, verify mismatch detected and data cleared

**Checkpoint**: User Story 3 complete - Session cleanup and security hardening

**Commit Message**:
```
feat(auth): add session cleanup and data validation

- Add Auth0 sub mismatch detection
- Auto-clear corrupted localStorage data
- Validate user identity on returning user fast path
- Prevent unauthorized access to different user's data

Implements User Story 3 (P2): Session Termination and Data Cleanup

Constitution: Principle IV (boundary validation), security hardening

Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>
```

---

## Phase 7: User Story 4 - Resilient Token Management (Priority: P3)

**Goal**: Replace global token subscription system with component-local hooks to eliminate race conditions

**Independent Test**: Manual test in browser:
1. Open browser dev tools console
2. Login and navigate between multiple pages
3. Monitor for token-related errors (should be zero)
4. Open multiple tabs, verify no race conditions
5. Check network requests show proper Authorization headers

**Dependency**: Independent of other user stories (can be implemented in parallel)

### Implementation for User Story 4

**Step 1: Create useTokenReady Hook**

- [ ] T181 [US4] Create client/src/hooks/auth/useTokenReady.ts with empty file and imports (FR-020)
- [ ] T182 [US4] Define TokenReadyState interface in client/src/hooks/auth/useTokenReady.ts per contracts/hook-contracts.ts lines 96-100
- [ ] T183 [US4] Implement useTokenReady() hook in client/src/hooks/auth/useTokenReady.ts per contracts/hook-contracts.ts lines 102-126 (FR-020)
- [ ] T184 [US4] Add state management in useTokenReady: useState for isReady, isWaiting, error
- [ ] T185 [US4] Add useEffect in useTokenReady: Verify Auth0 token availability independently
- [ ] T186 [US4] Return TokenReadyState from useTokenReady hook

**Step 2: Simplify Auth0TokenProvider**

- [ ] T187 [US4] Read client/src/components/auth/Auth0TokenProvider.tsx to understand current subscription system (lines 11-56)
- [ ] T188 [US4] Remove BehaviorSubject import from client/src/components/auth/Auth0TokenProvider.tsx
- [ ] T189 [US4] Remove tokenReadySubject declaration from client/src/components/auth/Auth0TokenProvider.tsx (around line 11)
- [ ] T190 [US4] Remove subscription system code from client/src/components/auth/Auth0TokenProvider.tsx (lines 11-56)
- [ ] T191 [US4] Keep only token getter registration in client/src/components/auth/Auth0TokenProvider.tsx (setAuth0TokenGetter call)
- [ ] T192 [US4] Keep error handling logic in client/src/components/auth/Auth0TokenProvider.tsx (unchanged)

**Step 3: Update Apollo Client**

- [ ] T193 [US4] Read client/src/graphql/apollo-client.ts to understand current subscription usage
- [ ] T194 [US4] Remove subscription-related imports from client/src/graphql/apollo-client.ts
- [ ] T195 [US4] Remove tokenReadySubject subscription code from client/src/graphql/apollo-client.ts
- [ ] T196 [US4] Keep getAuth0Token variable in client/src/graphql/apollo-client.ts (unchanged)
- [ ] T197 [US4] Keep setAuth0TokenGetter export in client/src/graphql/apollo-client.ts (unchanged)
- [ ] T198 [US4] Keep authLink usage in client/src/graphql/apollo-client.ts (unchanged - uses getAuth0Token directly)

**Step 4: Manual Testing**

- [ ] T199 [US4] Manual test: Open browser dev tools console, clear all console logs
- [ ] T200 [US4] Manual test: Login and navigate between 5+ different pages
- [ ] T201 [US4] Manual test: Verify zero token subscription errors in console
- [ ] T202 [US4] Manual test: Open 3 tabs, navigate in each, verify no race conditions
- [ ] T203 [US4] Manual test: Check Network tab shows Authorization headers on GraphQL requests
- [ ] T204 [US4] Manual test: Use useTokenReady in a test component, verify isReady becomes true after auth

**Checkpoint**: User Story 4 complete - Token management simplified, race conditions eliminated

**Commit Message**:
```
feat(auth): simplify token management with component-local hooks

- Create useTokenReady hook for component-local verification
- Remove global BehaviorSubject subscription system
- Eliminate token subscription race conditions
- Each component verifies token independently

Implements User Story 4 (P3): Resilient Token Management

Constitution: Simplification principle, component-local state

Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>
```

---

## Phase 8: User Story 5 - Optimized Data Fetching (Priority: P3)

**Goal**: Use Apollo cache-first policy to reduce redundant network requests for user profile data

**Independent Test**: Manual test in browser:
1. Login as returning user
2. Open Network tab in browser dev tools
3. Navigate between multiple pages that use user info
4. Count network requests to GET_USER_INFO endpoint
5. Verify cache hit rate > 80% (most requests served from cache)

**Dependency**: Independent of other user stories (can be implemented in parallel)

### Implementation for User Story 5

**Step 1: Update useUserSync Query Policy**

- [ ] T205 [US5] Read client/src/hooks/user/useUserSync.ts to locate GET_USER_INFO query
- [ ] T206 [US5] Modify useQuery options in client/src/hooks/user/useUserSync.ts: Add fetchPolicy: 'cache-first' per quickstart.md lines 367-373 (FR-021)
- [ ] T207 [US5] Modify useQuery options in client/src/hooks/user/useUserSync.ts: Add nextFetchPolicy: 'cache-and-network' per contracts/apollo-contracts.ts lines 138-142 (FR-022)
- [ ] T208 [US5] Verify errorPolicy: 'all' remains in useQuery options (unchanged)

**Step 2: Add Apollo Cache Type Policy**

- [ ] T209 [US5] Read client/src/graphql/apollo-client.ts to locate InMemoryCache instantiation
- [ ] T210 [US5] Add typePolicies configuration to InMemoryCache in client/src/graphql/apollo-client.ts per quickstart.md lines 378-393
- [ ] T211 [US5] Add Query.fields.userInfo merge policy in client/src/graphql/apollo-client.ts per contracts/apollo-contracts.ts lines 70-75
- [ ] T212 [US5] Verify merge function returns incoming ?? existing (prefer fresh data)

**Step 3: Manual Testing**

- [ ] T213 [US5] Manual test: Clear browser cache, login as returning user
- [ ] T214 [US5] Manual test: Open Network tab, filter for GraphQL requests
- [ ] T215 [US5] Manual test: Navigate to home, then profile, then back to home
- [ ] T216 [US5] Manual test: Count GET_USER_INFO network requests (should be 1-2, not 5+)
- [ ] T217 [US5] Manual test: Open Apollo DevTools, inspect cache, verify userInfo is cached
- [ ] T218 [US5] Manual test: Refresh page, verify userInfo loads from cache immediately
- [ ] T219 [US5] Manual test: Calculate cache hit rate: (total page loads - network requests) / total page loads, verify > 80%

**Checkpoint**: User Story 5 complete - User data fetching optimized with caching

**Commit Message**:
```
perf(auth): optimize user info caching with cache-first policy

- Use cache-first fetch policy for GET_USER_INFO
- Add Apollo cache type policy for userInfo
- Reduce network requests for returning users
- Background refresh keeps data current

Implements User Story 5 (P3): Optimized Data Fetching

Constitution: Performance optimization, no behavioral changes

Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>
```

---

## Phase 9: Polish & Cross-Cutting Concerns

**Purpose**: Centralize constants and validate implementation

**Step 1: Centralize Storage Keys**

- [ ] T220 Create client/src/utils/storageKeys.ts with STORAGE_KEYS constant object per quickstart.md lines 427-439
- [ ] T221 Define StorageKey type alias in client/src/utils/storageKeys.ts
- [ ] T222 Update client/src/utils/userStorage.ts to import and use STORAGE_KEYS.USER_DATA instead of hardcoded string
- [ ] T223 [P] Find and update other localStorage usage: client/src/hooks/useCardSizePreference.ts to use STORAGE_KEYS
- [ ] T224 [P] Find and update other localStorage usage: client/src/hooks/useLanguageDetection.ts to use STORAGE_KEYS
- [ ] T225 [P] Add type safety: Ensure all localStorage.getItem/setItem calls use StorageKey type

**Step 2: Documentation**

- [ ] T226 [P] Verify CLAUDE.md has been updated by agent context script (should have TypeScript/React 19 context)
- [ ] T227 [P] Update client/README.md if needed with new authentication flow documentation
- [ ] T228 [P] Verify quickstart.md matches actual implementation (may need minor updates based on implementation learnings)

**Step 3: Code Quality**

- [ ] T229 [P] Run `npm run lint` in client/ directory, fix any linting errors
- [ ] T230 [P] Run `npm run build` in client/ directory, verify build succeeds without warnings
- [ ] T231 [P] Run TypeScript compiler check: `npx tsc --noEmit` in client/, fix any type errors
- [ ] T232 [P] Review all files for console.log statements, remove or convert to proper logging

**Step 4: Final Validation**

- [ ] T233 Run complete manual test suite from quickstart.md (all scenarios for all user stories)
- [ ] T234 Verify backward compatibility: Existing users can login without issues
- [ ] T235 Test across browsers: Chrome, Firefox, Safari, Edge
- [ ] T236 Measure performance: Confirm returning user redirect < 1 second
- [ ] T237 Verify cache hit rate: Confirm > 80% for user profile data
- [ ] T238 Check localStorage: Verify no sensitive data stored (only user metadata)

**Step 5: Deployment Preparation**

- [ ] T239 Create deployment plan: Phase 1 → Phase 3-4-5 → Phase 2 (Auth0 update last)
- [ ] T240 Document rollback procedure: How to revert if issues arise
- [ ] T241 Prepare monitoring: Define metrics to track (redirect time, cache hits, errors)

**Final Commit Message**:
```
refactor(storage): centralize localStorage key constants

- Create STORAGE_KEYS constant object
- Update all localStorage access to use constants
- Type-safe storage key references
- Code quality improvements (linting, type checking)

Constitution: Consistency, maintainability

Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>
```

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - VERIFIES prerequisites exist
- **User Story 1 (Phase 3)**: Depends on Foundational phase completion - CREATES userStorage utility
- **User Story 2 (Phase 4)**: Depends on User Story 1 completion - REQUIRES userStorage utility
- **User Story 3 (Phase 5)**: Depends on User Story 1 completion - EXTENDS userStorage validation
- **User Story 4 (Phase 6)**: Depends on Foundational phase completion - INDEPENDENT of other stories
- **User Story 5 (Phase 7)**: Depends on Foundational phase completion - INDEPENDENT of other stories
- **Polish (Phase 8)**: Depends on all desired user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational (Phase 2) - No dependencies on other stories
- **User Story 2 (P1)**: MUST complete after User Story 1 - Uses isReturningUser() from userStorage
- **User Story 3 (P2)**: MUST complete after User Story 1 - Uses clearUserData() from userStorage
- **User Story 4 (P3)**: Can start after Foundational (Phase 2) - Completely independent
- **User Story 5 (P3)**: Can start after Foundational (Phase 2) - Completely independent

### Critical Path (Sequential Implementation)

```
Phase 1 (Setup)
  ↓
Phase 2 (Foundational)
  ↓
Phase 3 (User Story 1: localStorage utility) ← MUST BE FIRST
  ↓
Phase 4 (User Story 2: Callback routing) ← Depends on US1
  ↓
Phase 5 (User Story 3: Session cleanup) ← Depends on US1
  ↓
Phases 6-7 can run in any order (US4, US5 are independent)
  ↓
Phase 8 (Polish)
```

### Parallel Opportunities

**Within User Story 1:**
- T015-T117: All storage utility functions can be written in parallel (different functions)
- T123-T125: Both logout button modifications can be done in parallel (different line numbers)

**Within User Story 2:**
- T134-T135: Type definitions can be written in parallel with component implementation
- T140-T148: Component implementation steps must be sequential
- T160-T163: Auth0 dashboard updates can be done by different person in parallel

**Within User Story 4:**
- T181-T186: useTokenReady hook creation
- T187-T192: Auth0TokenProvider simplification
- These two can run in parallel (different files)

**Within User Story 5:**
- T205-T208: useUserSync updates
- T209-T212: Apollo cache policy updates
- These two can run in parallel (different files)

**Across User Stories (if team has capacity):**
- After US1 completes, US2 and US3 must run sequentially
- But US4 and US5 can start immediately after Foundational phase (before or during US1-3)

### Within Each User Story

- Tests: NOT INCLUDED (manual testing only per specification)
- Models/Utilities before Services/Hooks
- Services/Hooks before Components
- Components before Routing changes
- Implementation before Integration
- Story complete before moving to next priority

### Task Execution Notes

- Tasks marked [P] can run in parallel IF different files AND no dependencies
- Tasks without [P] must run sequentially (often modifying same file)
- Manual test tasks (T126-T131, etc.) should be done together as a test suite
- Commit after completing each user story phase

---

## Parallel Example: User Story 1

```bash
# Cannot parallelize userStorage functions - same file, sequential order needed
# T015-T117 must run in sequence

# Can parallelize logout button modifications (different line numbers in same file):
Task T123: "Modify mobile logout button onClick"
Task T124: "Modify desktop logout button onClick"
# (Both modify AuthButton.tsx but at different locations)

# Manual tests should run as a suite:
Task T126-T131: Run all together as acceptance test suite
```

---

## Implementation Strategy

### MVP First (User Stories 1 & 2 Only)

**Minimum Viable Product delivers core value:**

1. Complete Phase 1: Setup (T001-T006)
2. Complete Phase 2: Foundational (T007-T014)
3. Complete Phase 3: User Story 1 (T015-T131) ← Fast authentication for returning users
4. Complete Phase 4: User Story 2 (T132-T169) ← Proper callback routing
5. **STOP and VALIDATE**: Test both stories independently
6. Deploy/demo if ready

**This MVP delivers:**
- Returning users skip registration (< 1s redirect) ✅
- New users complete registration successfully ✅
- Logout clears data ✅
- Core problem solved ✅

**Remaining stories (US3-US5) are enhancements:**
- US3: Security hardening (sub validation, corruption handling)
- US4: Reliability improvement (remove race conditions)
- US5: Performance optimization (caching)

### Incremental Delivery (Recommended)

1. **Release 1**: Setup + Foundational → Foundation ready
2. **Release 2**: User Story 1 → Test independently → Deploy (Fast auth for returning users!)
3. **Release 3**: User Story 2 → Test independently → Deploy (Callback routing separated!)
4. **Release 4**: User Story 3 → Test independently → Deploy (Security hardened!)
5. **Release 5**: User Stories 4 & 5 → Test independently → Deploy (Optimizations complete!)
6. Each release adds value without breaking previous functionality

### Parallel Team Strategy

With 3 developers available:

1. **All together**: Complete Setup + Foundational (T001-T014)
2. **Sequential on critical path**: User Story 1 must complete first (all devs help)
3. **After US1 completes**:
   - Developer A: User Story 2 (requires US1)
   - Developer B: User Story 4 (independent)
   - Developer C: User Story 5 (independent)
4. **After US2 completes**:
   - Developer A: User Story 3 (requires US1)
5. **All together**: Polish phase

---

## Notes

- [P] tasks = different files OR different locations in same file, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- Manual tests specified in quickstart.md (no automated tests requested)
- Commit after completing each user story phase (5 total commits for US1-US5)
- Stop at any checkpoint to validate story independently
- Phased rollout: Deploy US1 first for quick wins, US2-5 can follow incrementally
- Auth0 dashboard update (T160-T163) requires external access coordination
- Performance monitoring: Use browser dev tools (Network tab, localStorage inspector, console)

---

## Task Count Summary

- **Total Tasks**: 241 tasks
- **Phase 1 (Setup)**: 6 tasks
- **Phase 2 (Foundational)**: 8 tasks
- **Phase 3 (User Story 1 - P1)**: 24 tasks (15 implementation + 6 manual tests + 3 logout cleanup)
- **Phase 4 (User Story 2 - P1)**: 31 tasks (19 implementation + 6 manual tests + 4 Auth0 config + 2 routing)
- **Phase 5 (User Story 3 - P2)**: 11 tasks (6 implementation + 5 manual tests)
- **Phase 6 (User Story 4 - P3)**: 24 tasks (18 implementation + 6 manual tests)
- **Phase 7 (User Story 5 - P3)**: 15 tasks (8 implementation + 7 manual tests)
- **Phase 8 (Polish)**: 22 tasks (13 implementation + 9 validation)

**Parallelizable Tasks**: 36 tasks marked with [P] across all phases

**Suggested MVP Scope**: Phases 1-4 (User Stories 1 & 2) = 69 tasks → Delivers core authentication improvement

**Full Feature**: All 141 tasks → Complete authentication refactoring with all optimizations
