# Tasks: Authentication Flow Refactoring

**Input**: Design documents from `/specs/002-auth-flow-refactor/`
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/

**Tests**: Backend tests using MSTest with AwesomeAssertions (per CLAUDE.md). Frontend tests not specified - manual testing per quickstart.md.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3, or US1/US2 for combined stories)
- Include exact file paths in descriptions

## Path Conventions

- **Backend**: `src/` (C# .NET 9.0 solution)
- **Frontend**: `client/src/` (React 19 TypeScript)
- **GraphQL**: `src/App.MtgDiscovery.GraphQL/` for mutations/queries

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and branch setup

- [x] T001 Create feature branch `002-auth-flow-refactor` from main
- [x] T002 [P] Create auth type definitions in client/src/types/auth.ts

---

## Phase 2: Foundational (Backend - Blocking Prerequisites)

**Purpose**: Backend changes that MUST be complete before ANY frontend user story can be implemented

**Why Foundational**: The frontend cannot determine new vs returning user without the backend `isFirstLogin` flag. All user stories depend on this backend capability.

### Backend Entity Updates

- [x] T003 [P] Add `CreatedAt` and `LastLoginAt` properties to src/Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/UserInfoExtEntity.cs
- [x] T004 [P] Create IUserSyncOufEntity interface in src/Lib.Shared.DataModels/Entities/Oufs/IUserSyncOufEntity.cs

### Backend Adapter Layer

- [x] T005 Update UserInfoScribe to implement isFirstLogin logic in src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Scribes/UserInfoScribe.cs
- [x] T006 Update IRegisterUserAdapter response type in src/Lib.Adapter.User/Apis/Commands/IRegisterUserAdapter.cs
- [x] T007 Update RegisterUserAdapter implementation in src/Lib.Adapter.User/Commands/RegisterUserAdapter.cs

### Backend Aggregator Layer

- [x] T008 Update IRegisterUserAggregatorService response type in src/Lib.Aggregator.User/Apis/Commands/IRegisterUserAggregatorService.cs
- [x] T009 Update RegisterUserAggregatorService implementation in src/Lib.Aggregator.User/Commands/RegisterUserAggregatorService.cs

### Backend Domain Layer

- [x] T010 Update IRegisterUserDomainService response type in src/Lib.Domain.User/Apis/Commands/IRegisterUserDomainService.cs
- [x] T011 Update RegisterUserDomainService implementation in src/Lib.Domain.User/Commands/RegisterUserDomainService.cs

### Backend Entry Layer

- [x] T012 Create UserSyncOutEntity in src/Lib.MtgDiscovery.Entry/Entities/Outs/User/UserSyncOutEntity.cs
- [x] T013 Update IRegisterUserEntryService response type in src/Lib.MtgDiscovery.Entry/Apis/Queries/User/IRegisterUserEntryService.cs
- [x] T014 Update RegisterUserEntryService implementation in src/Lib.MtgDiscovery.Entry/Queries/User/RegisterUserEntryService.cs

### Backend GraphQL Layer

- [x] T015 Create UserSyncOutEntityType GraphQL type in src/App.MtgDiscovery.GraphQL/Entities/Types/User/UserSyncOutEntityType.cs
- [x] T016 Update UserRegistrationSuccessDataResponseModelType in src/App.MtgDiscovery.GraphQL/Entities/Types/ResponseModels/UserRegistrationSuccessDataResponseModelType.cs
- [x] T017 Update UserMutationMethods to return extended response in src/App.MtgDiscovery.GraphQL/Mutations/UserMutationMethods.cs

### Backend Tests

- [ ] T018 [P] Add unit tests for UserInfoScribe isFirstLogin logic in src/Lib.Adapter.Scryfall.Cosmos.Tests/
- [x] T019 Build and verify backend compiles with `dotnet build src/MtgDiscoveryVibe.sln`
- [x] T020 Run backend tests with `dotnet test src/MtgDiscoveryVibe.sln` (user adapter tests pass; other failures are pre-existing)

**Checkpoint**: Backend ready - frontend implementation can now begin. The `registerUserInfo` mutation now returns `isFirstLogin` flag.

---

## Phase 3: User Story 1 & 2 - Returning User / New User (Priority: P1) MVP

**Goal**: Implement the auth state machine and callback flow that determines new vs returning user and shows appropriate welcome message.

**Note**: User Stories 1 and 2 are combined in this phase because they share the same implementation (the `isFirstLogin` flag determines which path). They cannot be meaningfully separated.

**Independent Test**: Sign in with new account - see "Welcome to MtgDiscovery". Sign out and sign in again - see "Welcome back".

### Frontend Auth Types & State Machine

- [x] T021 [P] [US1/US2] Create AuthStatus enum and state types in client/src/types/auth.ts
- [x] T022 [P] [US1/US2] Create ToastContext for notifications in client/src/contexts/ToastContext.tsx
- [x] T023 [US1/US2] Create AuthStateContext with state machine in client/src/contexts/AuthStateContext.tsx
- [x] T024 [US1/US2] Create useAuthState hook in client/src/hooks/auth/useAuthState.ts (exported from AuthStateContext)

### Frontend GraphQL Integration

- [x] T025 [US1/US2] Create SYNC_USER mutation in client/src/graphql/mutations/user.ts
- [x] T026 [US1/US2] Run GraphQL codegen to generate TypeScript types with `npm run codegen` in client/ (manual update - backend not running)

### Frontend Callback & Token Management

- [x] T027 [US1/US2] Create useAuthCallback hook with error handling for auth failures (FR-012) in client/src/hooks/auth/useAuthCallback.ts
- [x] T028 [US1/US2] Create AuthCallbackPage component in client/src/components/pages/AuthCallbackPage.tsx
- [x] T029 [US1/US2] Simplify Auth0TokenProvider - remove subscription system, verify useRefreshTokens enabled (FR-011) in client/src/components/auth/Auth0TokenProvider.tsx
- [x] T030 [US1/US2] Create useToast hook in client/src/hooks/useToast.ts (exported from ToastContext)

### Frontend App Integration

- [x] T031 [US1/US2] Add /auth/callback route and wrap app with providers in client/src/App.tsx
- [x] T032 [US1/US2] Update Auth0Provider redirectUri in client/src/main.tsx

**Checkpoint**: At this point, new and returning users see appropriate welcome messages. Test with quickstart.md Test 1 and Test 2.

---

## Phase 4: User Story 3 - User Sign-Out (Priority: P2)

**Goal**: Clean sign-out that clears all user data and returns user to anonymous state.

**Independent Test**: Sign in, then sign out. Sign in again and verify no stale data from previous session.

### Frontend Sign-Out Implementation

- [x] T033 [US3] Update AuthButton logout to clear Apollo cache in client/src/components/auth/AuthButton.tsx
- [x] T034 [US3] Add sign-out state transition in AuthStateContext in client/src/contexts/AuthStateContext.tsx (already implemented in Phase 3)

**Checkpoint**: Sign-out clears all data. Test with quickstart.md Test 4.

---

## Phase 5: User Story 4 - Protected Resource Access (Priority: P2)

**Goal**: Unauthenticated users are redirected to sign in and returned to their intended destination.

**Independent Test**: Navigate to /collection while unauthenticated. Complete sign-in. Verify redirect back to /collection.

### Frontend Protected Routes

- [x] T035 [US4] Create ProtectedRoute component in client/src/components/auth/ProtectedRoute.tsx
- [x] T036 [US4] Apply ProtectedRoute to collection routes in client/src/App.tsx (BinderPage, WishlistPage)

**Checkpoint**: Protected routes work with return-to flow. Test with quickstart.md Test 5.

---

## Phase 6: User Story 5 - Session Expiry Handling (Priority: P3)

**Goal**: Graceful handling when user session expires, with clear messaging and easy re-authentication.

**Independent Test**: Simulate token expiry (clear Auth0 cookies). Attempt authenticated action. Verify error message and re-auth flow.

### Frontend Session Expiry

- [x] T037 [US5] Add session expiry detection in useAuthCallback in client/src/hooks/auth/useAuthCallback.ts
- [x] T038 [US5] Add expiry error state and message in AuthStateContext in client/src/contexts/AuthStateContext.tsx (already implemented in Phase 3)

**Checkpoint**: Session expiry shows clear message. User can re-authenticate.

---

## Phase 7: Polish & Cleanup

**Purpose**: Remove legacy code and verify all acceptance criteria

### Cleanup Tasks

- [x] T039 [P] Remove localStorage PII storage from any remaining locations (resolved by T041 - SignInRedirectPage no longer used)
- [~] T040 [P] Remove old token subscription system remnants - PARTIAL: Auth0TokenProvider simplified; full removal blocked by useUserSync dependency
- [x] T041 [P] Remove old /signin-redirect route - now redirects to /auth/callback for backwards compatibility

### Validation

- [ ] T042 Run full quickstart.md validation (all 5 tests)
- [ ] T043 Verify no PII in localStorage (SC-003)
- [ ] T044 Verify welcome message timing < 1 second (SC-002)
- [ ] T045 Verify token refresh works silently without user intervention (FR-011, SC-007)
- [ ] T046 Verify clear error messages display for auth failures (FR-012)

### Edge Case Validation

- [ ] T047 [P] Verify Auth0 unavailable shows friendly error message with retry option
- [ ] T048 [P] Verify backend sync failure shows error with retry, Auth0 session preserved
- [ ] T049 [P] Verify network loss during auth shows connection error with retry
- [ ] T050 [P] Verify multiple browser tabs maintain consistent auth state
- [ ] T051 [P] Verify Auth0 callback error shows specific error message with guidance

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup - BLOCKS all frontend user stories
- **User Stories 1&2 (Phase 3)**: Depends on Foundational phase completion
- **User Story 3 (Phase 4)**: Depends on Phase 3 (needs AuthStateContext)
- **User Story 4 (Phase 5)**: Depends on Phase 3 (needs AuthStateContext)
- **User Story 5 (Phase 6)**: Depends on Phase 3 (needs useAuthCallback)
- **Polish (Phase 7)**: Depends on all user stories being complete

### User Story Dependencies

- **User Story 1 & 2 (P1)**: Combined - share implementation. MUST complete first.
- **User Story 3 (P2)**: Can run in parallel with US4 after US1&2 complete
- **User Story 4 (P2)**: Can run in parallel with US3 after US1&2 complete
- **User Story 5 (P3)**: Can start after US1&2 complete (independent of US3/US4)

### Within Each Phase

- Backend: Entity → Adapter → Aggregator → Domain → Entry → GraphQL (layer flow)
- Frontend: Types → Context → Hooks → Components → App integration
- Tests before implementation where specified

### Parallel Opportunities

**Phase 2 (Backend)**:
- T003 and T004 can run in parallel (different files)
- T018 can run in parallel with T015-T017

**Phase 3 (Frontend)**:
- T021 and T022 can run in parallel (different files)

**Phase 4-6**:
- User Stories 3, 4, and 5 can run in parallel after Phase 3 completes

**Phase 7 (Cleanup)**:
- T039, T040, T041 can all run in parallel

**Phase 7 (Edge Case Validation)**:
- T047, T048, T049, T050, T051 can all run in parallel

---

## Parallel Example: Phase 2 Backend

```bash
# Launch entity updates together:
Task: "Add CreatedAt/LastLoginAt to UserInfoExtEntity"
Task: "Create IUserSyncOufEntity interface"

# After entities complete, launch tests in parallel with GraphQL layer:
Task: "Add unit tests for UserInfoScribe"
Task: "Create UserSyncDataType GraphQL type"
```

---

## Implementation Strategy

### MVP First (Phase 1-3 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Backend foundation (CRITICAL - blocks frontend)
3. Complete Phase 3: User Stories 1 & 2 (new/returning user flow)
4. **STOP and VALIDATE**: Test with quickstart.md Tests 1 & 2
5. Deploy/demo if ready - core value delivered

### Incremental Delivery

1. Setup + Backend Foundation → Backend ready
2. Add US1&2 → Test independently → Deploy (MVP: users can sign in with correct welcome)
3. Add US3 → Test independently → Deploy (clean sign-out)
4. Add US4 → Test independently → Deploy (protected routes)
5. Add US5 → Test independently → Deploy (session expiry handling)
6. Cleanup → Final validation → Complete

### Single Developer Strategy

1. Complete Phase 1-2 sequentially (backend must be done first)
2. Complete Phase 3 (core auth flow)
3. Complete Phases 4-6 in priority order (US3 → US4 → US5)
4. Complete Phase 7 (cleanup)

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Backend uses MicroObjects layered architecture (data flows: App → Entry → Domain → Aggregator → Adapter)
- Frontend uses React Context for auth state, MUI Snackbar for toasts
- No frontend test framework specified - use manual testing per quickstart.md
- Backend tests use MSTest with AwesomeAssertions per project conventions
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
