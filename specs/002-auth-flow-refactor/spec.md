# Feature Specification: Authentication Flow Refactoring

**Feature Branch**: `002-auth-flow-refactor`
**Created**: 2026-01-18
**Status**: Draft
**Input**: User description: "Refactor authentication flow to align with industry standards for Auth0 integration"

## Overview

This feature refactors the existing authentication flow in MtgDiscovery to follow industry best practices for OAuth2/Auth0 integration. The current implementation has several issues including client-side user state determination, PII stored in localStorage, complex token subscription systems, and a "first time every time" user experience for returning users.

The refactored flow will:
- Let the backend determine new vs returning user status
- Use a single idempotent sync operation
- Implement an explicit auth state machine
- Remove unnecessary complexity
- Provide appropriate welcome messages based on user status

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Returning User Quick Sign-In (Priority: P1)

A returning user who has previously registered with MtgDiscovery signs in using Auth0. The system recognizes them as a returning user and provides a seamless, fast authentication experience with a personalized welcome message.

**Why this priority**: This addresses the core "first time every time" problem. Returning users are the majority of sign-ins and their experience directly impacts user retention and satisfaction.

**Independent Test**: Can be fully tested by signing in with an existing user account and verifying the welcome message shows "Welcome back" and authentication completes quickly without unnecessary registration screens.

**Acceptance Scenarios**:

1. **Given** a user who has previously signed in to MtgDiscovery, **When** they click "Sign In" and complete Auth0 authentication, **Then** they see "Welcome back, [name]!" message and are redirected to their intended destination.

2. **Given** a returning user completing sign-in, **When** the authentication callback processes, **Then** no artificial delays occur and the user reaches their destination as fast as the network allows.

3. **Given** a returning user with an existing session, **When** they refresh the page, **Then** their authenticated state is preserved without requiring re-authentication.

---

### User Story 2 - New User Registration (Priority: P1)

A first-time user signs up for MtgDiscovery through Auth0. The system recognizes them as a new user and welcomes them appropriately while creating their account.

**Why this priority**: New user registration is equally critical as it's the entry point for all users. The experience must be smooth and welcoming.

**Independent Test**: Can be fully tested by signing in with a new Auth0 account and verifying the welcome message shows "Welcome to MtgDiscovery" and the account is created.

**Acceptance Scenarios**:

1. **Given** a user who has never signed in to MtgDiscovery, **When** they click "Sign In" and complete Auth0 authentication, **Then** they see "Welcome to MtgDiscovery!" message and their account is created.

2. **Given** a new user completing registration, **When** the system creates their account, **Then** their registration timestamp and last login time are recorded.

3. **Given** a new user who completes registration, **When** they sign in again later, **Then** they are treated as a returning user.

---

### User Story 3 - User Sign-Out (Priority: P2)

An authenticated user signs out of MtgDiscovery. The system clears all user data and returns them to an anonymous state, ensuring no stale data persists.

**Why this priority**: Clean sign-out is essential for security, shared devices, and switching accounts. It's less frequent than sign-in but critical for trust.

**Independent Test**: Can be fully tested by signing in, then signing out, and verifying all user-specific data is cleared and no traces remain.

**Acceptance Scenarios**:

1. **Given** an authenticated user, **When** they click "Sign Out", **Then** they are signed out of Auth0 and redirected to the home page.

2. **Given** a user who just signed out, **When** they sign back in (even as the same user), **Then** they receive a fresh session with no stale data from the previous session.

3. **Given** a user who signs out on a shared device, **When** another user signs in, **Then** no data from the previous user is visible or accessible.

---

### User Story 4 - Protected Resource Access (Priority: P2)

An unauthenticated user attempts to access a protected resource (like their collection). The system redirects them to sign in and returns them to their intended destination after authentication.

**Why this priority**: This ensures protected features remain protected while providing a smooth redirect flow. Important for user experience when accessing bookmarks or shared links.

**Independent Test**: Can be fully tested by navigating to a protected page while unauthenticated and verifying the redirect-to-login-and-back flow works correctly.

**Acceptance Scenarios**:

1. **Given** an unauthenticated user, **When** they navigate to a protected page, **Then** they are redirected to sign in with their intended destination preserved.

2. **Given** a user who was redirected to sign in, **When** they complete authentication, **Then** they are returned to their originally intended page.

3. **Given** an authenticated user, **When** they navigate to a protected page, **Then** they can access it directly without interruption.

---

### User Story 5 - Session Expiry Handling (Priority: P3)

A user's authentication session expires during their visit. The system handles this gracefully, informing the user and providing easy re-authentication.

**Why this priority**: Session expiry is an edge case that happens less frequently but must be handled gracefully to avoid user confusion or data loss.

**Independent Test**: Can be tested by simulating a token expiry and verifying the user receives a clear message and can easily re-authenticate.

**Acceptance Scenarios**:

1. **Given** a user with an expired session, **When** they attempt an authenticated action, **Then** they see "Your session expired. Please sign in again." and can easily re-authenticate.

2. **Given** a user whose session is about to expire, **When** the system can refresh the token silently, **Then** the user experiences no interruption.

---

### Edge Cases

- What happens when Auth0 is unavailable? Users see a friendly error message and can retry.
- What happens when the backend sync fails? Users see an error with retry option, Auth0 session is preserved.
- What happens when network is lost during authentication? Users see connection error and can retry.
- What happens when a user has multiple browser tabs open? Each tab maintains consistent auth state.
- What happens when Auth0 returns an error during callback? Users see specific error message with guidance.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST determine new vs returning user status on the backend, not client-side
- **FR-002**: System MUST provide a single idempotent sync operation that creates users on first call and updates last login on subsequent calls
- **FR-003**: System MUST return an `isFirstLogin` flag from the backend to enable appropriate welcome messages
- **FR-004**: System MUST NOT store personally identifiable information (PII) in browser localStorage
- **FR-005**: System MUST display "Welcome to MtgDiscovery!" for first-time users
- **FR-006**: System MUST display "Welcome back, [name]!" for returning users
- **FR-007**: System MUST clear all cached user data when a user signs out
- **FR-008**: System MUST preserve the user's intended destination when redirecting to sign-in
- **FR-009**: System MUST track user registration timestamp (createdAt) and last login timestamp (lastLoginAt)
- **FR-010**: System MUST implement explicit authentication states: initializing, unauthenticated, authenticating, syncing, authenticated
- **FR-011**: System MUST handle token refresh automatically without user intervention
- **FR-012**: System MUST provide clear error messages for authentication failures
- **FR-013**: System MUST support protected routes that require authentication
- **FR-014**: System MUST handle session expiry gracefully with user notification

### Key Entities

- **User**: Represents a registered user with identity from Auth0
  - userId: Unique identifier generated from Auth0 subject
  - displayName: User's display name from Auth0 profile
  - email: User's email address
  - createdAt: Timestamp of first registration
  - lastLoginAt: Timestamp of most recent sign-in

- **Auth State**: Represents the current authentication status
  - status: Current state (initializing, unauthenticated, authenticating, syncing, authenticated)
  - user: Current user profile (if authenticated)
  - isFirstLogin: Whether this session is the user's first sign-in
  - error: Current error state (if any)

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Returning users complete sign-in and reach their destination without seeing registration screens
- **SC-002**: Users see appropriate welcome message (new vs returning) within 1 second of callback completion
- **SC-003**: No PII is stored in browser localStorage after implementation
- **SC-004**: Sign-out clears all user data - subsequent sign-in shows no stale data
- **SC-005**: Protected routes redirect unauthenticated users to sign-in and return them to intended destination after authentication
- **SC-006**: Session expiry is handled gracefully with clear messaging and easy re-authentication
- **SC-007**: Token refresh happens silently without interrupting user experience
- **SC-008**: Authentication state is consistent across page refreshes (authenticated users stay authenticated)

## Assumptions

- Auth0 is the identity provider and will remain so
- The backend uses idempotent UPSERT operations for user data
- Users have modern browsers that support standard OAuth2 flows
- Network connectivity is generally stable during authentication flows
- The existing Auth0 configuration (domain, client ID, audience) remains unchanged
- Material-UI is available for UI components (toast notifications, loading indicators)
- The application already has React Router for navigation
- Apollo Client is used for GraphQL operations and caching

## Out of Scope

- Changes to Auth0 configuration beyond adding new callback URLs
- User profile editing or preferences
- Multi-factor authentication configuration
- Social login configuration (handled by Auth0)
- Password reset flows (handled by Auth0)
- Account deletion
- User roles or permissions beyond authenticated/unauthenticated
