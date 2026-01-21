# Feature Specification: Authentication Flow Refactoring

**Feature Branch**: `001-auth-refactoring`
**Created**: 2026-01-17
**Status**: Draft
**Input**: User description: "use @.docs/auth-refactoring-plan.md"

## Clarifications

### Session 2026-01-17

- Q: What should the backend GraphQL verification endpoint be named and what exact fields should it return? → A: Query name: `verifyOrCreateUser`, Returns: `{ userId: String!, needsOnboarding: Boolean!, auth0Sub: String!, displayName: String, email: String, lastVerifiedAt: DateTime! }`
- Q: What observability requirements should be included for monitoring the authentication flow in production? → A: Minimal logging: Error logs only. Metrics: counter for authentication failures. No distributed tracing.
- Q: How long should cached localStorage data be considered fresh before requiring backend revalidation? → A: 24 hours - Cache valid for full day, reduces backend load significantly
- Q: What should users see during the backend verification loading state (when cache is stale or missing)? → A: Loading message: "Verifying your account..." with circular progress indicator (Material-UI CircularProgress)
- Q: How should the frontend handle persistent backend verification failures? → A: Optimistic with manual retry - Use cached data if available (even if stale), show non-blocking warning banner with manual retry button, block access only if no cache exists

## User Scenarios & Testing

### User Story 1 - Returning User Fast Authentication (Priority: P1)

As a returning user, I want to be instantly redirected to the home page after logging in, without being forced through the registration flow every time.

**Why this priority**: This is the core problem causing user frustration. Every returning user currently experiences the "first time every time" problem, making authentication feel slow and broken. Fixing this delivers immediate value to all existing users.

**Independent Test**: Can be fully tested by logging in as a user who has previously registered, verifying backend confirms user existence, measuring redirect time (should be < 1 second), and verifying no registration flow is shown. Delivers immediate value: faster authentication for returning users.

**Acceptance Scenarios**:

1. **Given** I am a returning user with fresh cache (< 24 hours), **When** I log in through Auth0, **Then** I see the home page instantly (0ms) without backend verification
2. **Given** I am a returning user with stale cache (> 24 hours), **When** I log in through Auth0, **Then** I see "Verifying your account..." with a progress indicator, then I am redirected to the home page in under 1 second
3. **Given** I am a returning user, **When** the backend confirms my user status, **Then** I see a "Welcome back!" message briefly before redirect
4. **Given** I have logged out, **When** I log back in, **Then** the backend verifies my user record and I get the fast path

---

### User Story 2 - New User Registration Flow (Priority: P1)

As a new user, I want a clear, one-time registration experience that saves my information for future logins.

**Why this priority**: Essential for onboarding new users. Must work correctly for the system to have any users. This is a prerequisite for the returning user fast path.

**Independent Test**: Can be fully tested by creating a new Auth0 account, logging in for the first time, verifying backend creates user record, and verifying the registration flow completes successfully. Delivers value: new users can successfully register.

**Acceptance Scenarios**:

1. **Given** I am a new user with no previous login history, **When** I log in through Auth0 for the first time, **Then** I see "Verifying your account..." with a progress indicator, the backend identifies me as a new user, and I am directed to the registration flow
2. **Given** I am completing registration, **When** the registration API call succeeds, **Then** the backend creates my user record and my user data is cached locally for faster future logins
3. **Given** I complete registration, **When** I am redirected to the home page, **Then** the backend has my user record and subsequent logins use the fast path
4. **Given** the backend user creation succeeds, **When** the response is received, **Then** localStorage is updated for optimized future UX

---

### User Story 3 - Session Termination and Data Cleanup (Priority: P2)

As a user, I want my local session data to be cleared when I log out, ensuring my privacy and allowing me to switch accounts cleanly.

**Why this priority**: Important for security and multi-account scenarios, but not blocking core functionality. Users can still authenticate without this, but it's needed for complete feature correctness.

**Independent Test**: Can be fully tested by logging in, verifying session data exists, logging out, and confirming session data is cleared. Delivers value: users can securely log out and switch accounts.

**Acceptance Scenarios**:

1. **Given** I am logged in and have session data stored, **When** I click the logout button, **Then** my local session data is cleared before logging out
2. **Given** I have logged out, **When** I try to log in again, **Then** the system treats me as a returning user (not a different user)
3. **Given** I log out and log in with a different Auth0 account, **When** the system detects a different user, **Then** the old user's data is cleared and replaced with the new user's data

---

### User Story 4 - Resilient Token Management (Priority: P3)

As a user, I want the system to handle authentication tokens reliably without race conditions or timing issues.

**Why this priority**: Improves reliability but doesn't directly impact user-visible functionality. Current system works most of the time; this removes edge cases.

**Independent Test**: Can be tested by monitoring token verification in browser dev tools, checking for race conditions, and verifying no token-related errors. Delivers value: more reliable authentication without intermittent failures.

**Acceptance Scenarios**:

1. **Given** I am authenticating, **When** the system verifies my token, **Then** no race conditions occur between multiple components
2. **Given** token verification is in progress, **When** I navigate between pages, **Then** each component verifies the token independently without conflicts
3. **Given** token verification fails, **When** I see an error, **Then** I receive a clear error message and can retry

---

### User Story 5 - Optimized Data Fetching (Priority: P3)

As a user, I want my profile information to load quickly from cache instead of making redundant network requests.

**Why this priority**: Performance optimization that improves user experience but doesn't block core functionality. Nice-to-have improvement.

**Independent Test**: Can be tested by monitoring network requests in browser dev tools and verifying user info is cached. Delivers value: faster page loads for returning users.

**Acceptance Scenarios**:

1. **Given** I am a returning user, **When** I navigate to a page that needs my user info, **Then** the data loads from cache instead of making a network request
2. **Given** my user info is cached, **When** I refresh the page, **Then** I see my info immediately while it refreshes in the background
3. **Given** I am on multiple tabs, **When** my user info updates in one tab, **Then** other tabs eventually reflect the update

---

### Edge Cases

- What happens when a user clears their browser's localStorage manually?
  - Frontend shows loading state while calling backend verification
  - Backend confirms user exists and returns user data
  - localStorage is repopulated from backend response
  - User continues to home page without re-registration

- How does the system handle corrupted localStorage data?
  - Type guard validation detects corruption
  - Corrupted data is cleared automatically
  - Backend verification endpoint is called as fallback
  - Fresh data from backend repopulates localStorage

- What happens when a user logs out and logs in with a different Auth0 account?
  - Backend verification returns different userId
  - Old user's localStorage data is cleared
  - New user's data from backend replaces old data
  - Routing determined by backend's needsOnboarding flag

- How does the system handle Auth0 token errors?
  - Token verification catches errors
  - User sees clear error message
  - System redirects to login if token refresh fails

- What happens if backend verification API call fails?
  - If localStorage has cached data (any age): Use cached data to allow access, show non-blocking warning banner "Unable to verify your account. You may see outdated information." with manual retry button
  - If no cached data: Show error message with retry button, block access to app until successful verification
  - Manual retry button allows user to retry verification without refreshing page
  - System automatically retries verification on next login attempt
  - System gracefully degrades to cached data when backend is unavailable (optimistic approach)

- What happens if registration API call fails?
  - User sees error message with specific failure reason
  - User can retry registration
  - No localStorage data is saved until backend confirms success (maintains data integrity)

- How does the system handle concurrent tabs?
  - localStorage events sync state across tabs
  - Each tab independently verifies token
  - Each tab calls backend verification independently (backend is idempotent)
  - Last login timestamp updates across all tabs

- What happens if localStorage and backend disagree on user status?
  - **Backend is always the source of truth**
  - If localStorage says "returning user" but backend says "new user": Show registration flow
  - If localStorage says "new user" but backend says "returning user": Clear cache, use backend data, show home page
  - Background verification corrects localStorage to match backend state

- What happens when cached localStorage data is older than 24 hours?
  - Frontend checks lastVerifiedAt timestamp on every login
  - If cache is stale (> 24 hours old): Treat as if no cache exists, call backend verification immediately
  - User sees loading state briefly while backend verification runs
  - Fresh backend response updates localStorage with new lastVerifiedAt timestamp
  - Cache is now valid for next 24 hours

## Requirements

### Functional Requirements

#### Backend Requirements (New)

- **FR-001**: Backend MUST provide GraphQL query named `verifyOrCreateUser` that accepts Auth0 JWT token via HTTP Authorization header and returns user status
- **FR-002**: Backend `verifyOrCreateUser` query MUST return type `{ userId: String!, needsOnboarding: Boolean!, auth0Sub: String!, displayName: String, email: String, lastVerifiedAt: DateTime! }` for authenticated requests
- **FR-003**: Backend verification endpoint MUST be idempotent and safe to call multiple times for the same user (returns consistent userId for same auth0Sub)
- **FR-004**: Backend verification endpoint MUST create user record automatically if Auth0 user doesn't exist in database (triggered by first call with new auth0Sub)
- **FR-005**: Backend verification endpoint MUST return needsOnboarding: true for newly created users (user record just created in this request)
- **FR-006**: Backend verification endpoint MUST return needsOnboarding: false for existing users with complete profiles (user record already exists from prior registration)
- **FR-007**: Backend MUST validate Auth0 JWT token signature and claims before returning user data (returns authentication error if token invalid)

#### Frontend Requirements

- **FR-008**: Frontend MUST call backend verification endpoint immediately after Auth0 callback
- **FR-009**: Frontend MUST use backend response to determine routing (home vs registration flow)
- **FR-010**: Frontend MUST redirect to home page for returning users (needsOnboarding: false) in under 1 second total
- **FR-011**: Frontend MUST display "Welcome back!" message to returning users during redirect
- **FR-012**: Frontend MUST redirect to registration flow for new users (needsOnboarding: true)
- **FR-013**: Frontend MUST cache backend verification response in localStorage for optimized UX (including lastVerifiedAt timestamp)
- **FR-014**: Frontend MUST use cached localStorage data for instant UI if cache is less than 24 hours old (based on lastVerifiedAt timestamp)
- **FR-015**: Frontend MUST call backend verification in background if cached data is older than 24 hours and update localStorage after verification completes
- **FR-016**: Frontend MUST treat backend as source of truth when localStorage and backend disagree on user status (clear stale cache and use backend response)
- **FR-017**: Frontend MUST clear old user localStorage data if backend returns different userId (account switch scenario)
- **FR-018**: Frontend MUST clear all stored session data when user clicks logout
- **FR-019**: Frontend MUST validate localStorage data structure and clear corrupted data automatically
- **FR-020**: Frontend MUST verify authentication tokens independently per component without global state races
- **FR-021**: Frontend MUST cache user profile information from GET_USER_INFO query and serve from cache on subsequent requests
- **FR-022**: Frontend MUST refresh cached user profile data in the background to keep it current
- **FR-023**: Frontend MUST gracefully degrade to cached localStorage data if backend verification fails temporarily
- **FR-024**: Frontend MUST display "Verifying your account..." message with circular progress indicator (Material-UI CircularProgress) when backend verification is in progress (cache stale or missing)
- **FR-025**: Frontend MUST use cached localStorage data (regardless of age) when backend verification fails and cache exists, displaying non-blocking warning banner: "Unable to verify your account. You may see outdated information."
- **FR-026**: Frontend MUST provide manual retry button in warning banner when backend verification fails with cached data fallback
- **FR-027**: Frontend MUST block access and show error with retry button when backend verification fails and no cache exists
- **FR-028**: Frontend MUST retry backend verification automatically on next login attempt after a failure

### Key Entities

- **User Verification Response** (Backend): Backend GraphQL response from `verifyOrCreateUser` query containing user status information `{ userId: String!, needsOnboarding: Boolean!, auth0Sub: String!, displayName: String, email: String, lastVerifiedAt: DateTime! }`. Source of truth for user existence and onboarding status. All fields except displayName and email are required/non-null.

- **User Session Data** (Frontend Cache): Stored in browser localStorage, contains cached copy of backend verification response including lastVerifiedAt timestamp. Cache is valid for 24 hours from lastVerifiedAt. Used for instant UI on login if fresh (< 24 hours old), otherwise triggers background verification. **Backend is always source of truth if conflict occurs.**

- **Authentication Status** (Frontend): Represents current state of authentication flow (processing, returning user, new user, error, backend-verification-pending). Determines routing and user experience during login.

- **Token Readiness** (Frontend): Represents component-local token verification status (ready, waiting, error). Ensures components can safely make authenticated requests without global state races.

## Success Criteria

### Measurable Outcomes

- **SC-001**: Returning users complete login and reach home page in under 1 second (measured from Auth0 callback to home page visible, including backend verification call)
- **SC-002**: Backend verification endpoint responds in under 200ms for p95 of requests
- **SC-003**: Cached localStorage data enables instant home page UI (0ms perceived delay) while backend verification runs in background
- **SC-004**: New users see clear status messages during registration and successfully complete first-time setup
- **SC-005**: Backend correctly identifies 100% of returning users via verification endpoint
- **SC-006**: Zero user record duplication errors (backend idempotency working correctly)
- **SC-007**: Logout reliably clears session data 100% of the time (verified via localStorage inspection)
- **SC-008**: User profile data cache hit rate exceeds 80% for returning users on subsequent page loads
- **SC-009**: Zero token subscription race condition errors (measured via error logging)
- **SC-010**: Frontend gracefully degrades to cached data if backend is temporarily unavailable (measured via network failure simulation)
- **SC-011**: Authentication flow works identically across all major browsers (Chrome, Firefox, Safari, Edge)

## Out of Scope

The following are explicitly not included in this feature:

- **Database schema changes** (user table already exists, no new fields required)
- **Auth0 provider configuration changes** (except adding new callback URL if needed)
- **User profile management features** (editing name, email, preferences)
- **Password reset or account recovery flows**
- **Multi-factor authentication**
- **Social login providers** beyond existing Auth0 configuration
- **Session timeout or idle detection**
- **"Remember me" checkbox functionality**
- **Cross-device session sync**
- **Offline authentication** (requires network for backend verification)

## Assumptions

- Auth0 is already configured and working for basic authentication
- Backend has existing GraphQL API infrastructure (HotChocolate)
- Backend already has user table/collection in database (Cosmos DB)
- Backend can add a new GraphQL query for user verification
- Backend REGISTER_USER mutation is idempotent (safe to call multiple times)
- Backend can auto-create user records if they don't exist (upsert pattern)
- Backend user ID generation is consistent and stable
- Backend can respond to verification requests in under 200ms (p95)
- Users have modern browsers with localStorage support
- Users have network connectivity for backend verification (graceful degradation if offline)
- Auth0 dashboard access will be available if callback URL update is needed

## Dependencies

### External Dependencies
- **Auth0 React SDK** (@auth0/auth0-react) - Authentication provider integration
- **Browser localStorage API** - Client-side caching (with fallback to memory if disabled)

### Internal Dependencies
- **Backend GraphQL API** - NEW: User verification endpoint required (GraphQL query name: `verifyOrCreateUser` per FR-001)
- **Backend User Service** - Existing: REGISTER_USER mutation (must remain idempotent)
- **Apollo Client** - Existing: GraphQL client for frontend-backend communication
- **React Router DOM** - Existing: Client-side routing
- **Material-UI** - Existing: UI component library

## Non-Functional Requirements

### Performance
- **Frontend**: Returning user redirect completes in under 1 second (total, including backend call if cache is stale)
- **Frontend**: Fresh cached localStorage (< 24 hours old) enables instant UI (0ms perceived delay, no backend call required)
- **Frontend**: Stale cache (> 24 hours old) requires backend verification with loading state shown to user
- **Frontend**: localStorage read/write operations complete synchronously (< 1ms)
- **Frontend**: Token verification completes without blocking UI rendering
- **Frontend**: 24-hour cache validity reduces backend verification calls by ~95% for daily active users
- **Backend**: User verification endpoint responds in under 200ms (p95)
- **Backend**: User verification endpoint handles concurrent requests efficiently (no database locking)

### Reliability
- **Frontend**: Type guard validation ensures data integrity at localStorage boundary
- **Frontend**: Corrupted data is automatically detected and cleared
- **Frontend**: Auth0 token errors are caught and handled gracefully
- **Frontend**: Graceful degradation to cached data if backend temporarily unavailable (optimistic approach with warning banner)
- **Frontend**: Manual retry mechanism for backend verification failures (non-blocking UI with retry button)
- **Frontend**: Access blocked only when no cached data exists and backend verification fails (prevents data-less state)
- **Frontend**: Automatic retry on next login attempt after backend verification failure
- **Backend**: Idempotent user creation prevents duplicate user records
- **Backend**: User verification endpoint returns consistent results for same input
- **Backend**: Handles Auth0 token validation failures with clear error messages

### Security
- **Frontend**: No sensitive tokens stored in localStorage (only user metadata and IDs)
- **Frontend**: Type validation prevents localStorage injection attacks
- **Frontend**: Session data cleared on logout to prevent unauthorized access
- **Backend**: Validates Auth0 JWT token signature before returning user data
- **Backend**: Auth0 sub validation prevents unauthorized access to different user's data
- **Backend**: User verification endpoint requires valid authentication (no anonymous access)

### Compatibility
- **Frontend**: Works across all modern browsers (Chrome, Firefox, Safari, Edge)
- **Frontend**: Graceful degradation if localStorage is disabled (uses in-memory cache)
- **Frontend**: Works with or without backend verification (cached data as fallback)
- **Backend**: GraphQL schema changes are additive only (no breaking changes to existing queries)
- **Backend**: Compatible with existing REGISTER_USER mutation behavior

### Maintainability
- **Frontend**: Centralized storage key constants for consistency
- **Frontend**: Clear separation of concerns (utilities, hooks, components)
- **Frontend**: TypeScript interfaces for all data structures
- **Frontend**: Component-local state eliminates global state complexity
- **Backend**: New endpoint follows existing GraphQL patterns and conventions
- **Backend**: User verification logic reuses existing user service components

### Observability
- **Logging**: Error logs only for authentication failures (minimal logging approach)
- **Metrics**: Counter for authentication failures tracked and monitored
- **Tracing**: No distributed tracing required
- **Monitoring**: Basic error rate monitoring sufficient for production
