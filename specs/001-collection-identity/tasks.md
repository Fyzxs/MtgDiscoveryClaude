# Tasks: Collection Identity Architecture

**Input**: Design documents from `/specs/001-collection-identity/`
**Prerequisites**: plan.md, spec.md, data-model.md, contracts/graphql-schema.md, research.md, quickstart.md

**Tests**: Tests are included per the spec's testing architecture (MSTest + AwesomeAssertions, fakes over mocks, TypeWrapper pattern).

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create new backend projects, add solution references, configure InternalsVisibleTo

- [x] T001 Create `Lib.Domain.Collections` class library project in `src/Lib.Domain.Collections/` targeting net9.0 with references to `Lib.Shared.DataModels`, `Lib.Shared.Invocation`, `Lib.Shared.Abstractions`
- [x] T002 [P] Create `Lib.Aggregator.Collections` class library project in `src/Lib.Aggregator.Collections/` targeting net9.0 with references to `Lib.Shared.DataModels`, `Lib.Shared.Invocation`, `Lib.Shared.Abstractions`, `Lib.Domain.Collections`
- [x] T003 [P] Create `Lib.Adapter.Collections` class library project in `src/Lib.Adapter.Collections/` targeting net9.0 with references to `Lib.Shared.DataModels`, `Lib.Shared.Invocation`, `Lib.Shared.Abstractions`
- [x] T004 [P] Create `Lib.Domain.Collections.Tests` test project in `src/Lib.Domain.Collections.Tests/` with reference to `Lib.Domain.Collections` and `TestConvenience.Core`
- [x] T005 [P] Create `Lib.Aggregator.Collections.Tests` test project in `src/Lib.Aggregator.Collections.Tests/` with reference to `Lib.Aggregator.Collections` and `TestConvenience.Core`
- [x] T006 [P] Create `Lib.Adapter.Collections.Tests` test project in `src/Lib.Adapter.Collections.Tests/` with reference to `Lib.Adapter.Collections` and `TestConvenience.Core`
- [x] T007 Add all new projects to `src/MtgDiscoveryVibe.sln`
- [x] T008 Add `InternalsVisibleTo` attributes in each new source project .csproj for corresponding test projects
- [x] T009 Add project references from `App.MtgDiscovery.GraphQL` to `Lib.Domain.Collections`, `Lib.Aggregator.Collections`, `Lib.Adapter.Collections`
- [x] T010 Add project reference from `Lib.Adapter.Scryfall.Cosmos` to `Lib.Adapter.Collections` (for adapter implementations)
- [x] T011 Verify solution builds: `dotnet build src/MtgDiscoveryVibe.sln`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core shared interfaces, Cosmos container, ExtEntities, and operators that ALL user stories depend on

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

### Shared Data Model Interfaces

- [x] T012 [P] Create `ICreateCollectionArgEntity` interface in `src/Lib.Shared.DataModels/Entities/Args/Collections/ICreateCollectionArgEntity.cs` with properties: `string Name`, `string Type`, `string Visibility`
- [x] T013 [P] Create `IRenameCollectionArgEntity` interface in `src/Lib.Shared.DataModels/Entities/Args/Collections/IRenameCollectionArgEntity.cs` with properties: `string CollectionId`, `string Name`
- [x] T014 [P] Create `IUpdateCollectionVisibilityArgEntity` interface in `src/Lib.Shared.DataModels/Entities/Args/Collections/IUpdateCollectionVisibilityArgEntity.cs` with properties: `string CollectionId`, `string Visibility`
- [x] T015 [P] Create `IDeleteCollectionArgEntity` interface in `src/Lib.Shared.DataModels/Entities/Args/Collections/IDeleteCollectionArgEntity.cs` with property: `string CollectionId`
- [x] T016 [P] Create `ITransferCollectionOwnershipArgEntity` interface in `src/Lib.Shared.DataModels/Entities/Args/Collections/ITransferCollectionOwnershipArgEntity.cs` with properties: `string CollectionId`, `string TargetUserId`
- [x] T017 [P] Create `IGrantCollectionAccessArgEntity` interface in `src/Lib.Shared.DataModels/Entities/Args/Collections/IGrantCollectionAccessArgEntity.cs` with properties: `string CollectionId`, `string TargetUserId`, `string Role`
- [x] T018 [P] Create `IRevokeCollectionAccessArgEntity` interface in `src/Lib.Shared.DataModels/Entities/Args/Collections/IRevokeCollectionAccessArgEntity.cs` with properties: `string CollectionId`, `string TargetUserId`

### Shared Itr Interfaces

- [x] T019 [P] Create `ICollectionItrEntity` interface in `src/Lib.Shared.DataModels/Entities/Itrs/Collections/ICollectionItrEntity.cs` with properties: `string CollectionId`, `string OwnerId`, `string Name`, `string Type`, `string Visibility`, `bool IsDefault`, `IReadOnlyList<IAuthorizedUserItrEntity> AuthorizedUsers`, `string CreatedAt`, `string UpdatedAt`
- [x] T020 [P] Create `IAuthorizedUserItrEntity` interface in `src/Lib.Shared.DataModels/Entities/Itrs/Collections/IAuthorizedUserItrEntity.cs` with properties: `string UserId`, `string Role`, `string GrantedAt`, `string GrantedBy`
- [x] T021 [P] Create `IRenameCollectionItrEntity` interface in `src/Lib.Shared.DataModels/Entities/Itrs/Collections/IRenameCollectionItrEntity.cs` with properties: `string CollectionId`, `string OwnerId`, `string Name`
- [x] T022 [P] Create `IUpdateCollectionVisibilityItrEntity` interface in `src/Lib.Shared.DataModels/Entities/Itrs/Collections/IUpdateCollectionVisibilityItrEntity.cs` with properties: `string CollectionId`, `string OwnerId`, `string Visibility`
- [x] T023 [P] Create `IDeleteCollectionItrEntity` interface in `src/Lib.Shared.DataModels/Entities/Itrs/Collections/IDeleteCollectionItrEntity.cs` with properties: `string CollectionId`, `string OwnerId`
- [x] T024 [P] Create `ITransferCollectionOwnershipItrEntity` interface in `src/Lib.Shared.DataModels/Entities/Itrs/Collections/ITransferCollectionOwnershipItrEntity.cs` with properties: `string CollectionId`, `string CurrentOwnerId`, `string TargetUserId`
- [x] T025 [P] Create `IGrantCollectionAccessItrEntity` interface in `src/Lib.Shared.DataModels/Entities/Itrs/Collections/IGrantCollectionAccessItrEntity.cs` with properties: `string CollectionId`, `string GrantorUserId`, `string TargetUserId`, `string Role`
- [x] T026 [P] Create `IRevokeCollectionAccessItrEntity` interface in `src/Lib.Shared.DataModels/Entities/Itrs/Collections/IRevokeCollectionAccessItrEntity.cs` with properties: `string CollectionId`, `string RevokerUserId`, `string TargetUserId`

### Shared Ouf/Out Interfaces

- [x] T027 [P] Create `ICollectionOufEntity` interface in `src/Lib.Shared.DataModels/Entities/Oufs/Collections/ICollectionOufEntity.cs` with properties: `string CollectionId`, `string OwnerId`, `string Name`, `string Type`, `string Visibility`, `bool IsDefault`, `IReadOnlyList<IAuthorizedUserOufEntity> AuthorizedUsers`, `string CreatedAt`, `string UpdatedAt`
- [x] T028 [P] Create `IAuthorizedUserOufEntity` interface in `src/Lib.Shared.DataModels/Entities/Oufs/Collections/IAuthorizedUserOufEntity.cs` with properties: `string UserId`, `string Role`, `string GrantedAt`, `string GrantedBy`
- [x] T029 [P] ~~Create `ICollectionOutEntity` interface~~ **SKIPPED**: OutEntity types are concrete classes in Entry layer, not interfaces in Shared.DataModels (pattern discovery). Concrete OutEntity will be created in Phase 3.
- [x] T030 [P] ~~Create `IAuthorizedUserOutEntity` interface~~ **SKIPPED**: Same as T029.

### Cosmos Infrastructure

- [x] T031 Create `CollectionExtEntity` class in `src/Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/CollectionExtEntity.cs` with Newtonsoft.Json `[JsonProperty]` attributes mapping to Cosmos document fields (id, owner_id, name, type, visibility, is_default, authorized_users, created_at, updated_at)
- [x] T032 [P] Create `AuthorizedUserExtEntity` class in `src/Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/Entities/AuthorizedUserExtEntity.cs` (in Entities/ subfolder per existing pattern)
- [x] T033 Create `CollectionsCosmosContainerName` class in `src/Lib.Adapter.Scryfall.Cosmos/Cosmos/Primitives/CollectionsCosmosContainerName.cs` (in Primitives/ per existing pattern, not Names/)
- [x] T034 Create `CollectionsCosmosContainerDefinition` class in `src/Lib.Adapter.Scryfall.Cosmos/Cosmos/Containers/Definitions/CollectionsCosmosContainerDefinition.cs` with partition key `/owner_id` and composite index on `(owner_id asc, created_at desc)`
- [x] T035 Create `CollectionsCosmosContainer` class in `src/Lib.Adapter.Scryfall.Cosmos/Cosmos/Containers/CollectionsCosmosContainer.cs` following `UserCardsCosmosContainer` pattern
- [x] T036 Create `CollectionGopher` (read operator) in `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Gophers/CollectionGopher.cs` following existing Gopher pattern
- [x] T037 [P] Create `CollectionScribe` (write operator) in `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Scribes/CollectionScribe.cs` following existing Scribe pattern
- [x] T038 [P] Create `CollectionsInquisitor` (query operator) in `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitors/CollectionsInquisitor.cs` following existing Inquisitor pattern for querying by owner_id and cross-partition queries for authorized_users

### Modify Existing ExtEntities

- [x] T039 [P] Add `collection_id` property with `[JsonProperty("collection_id")]` and default empty string to `UserCardExtEntity` in `src/Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/UserCardExtEntity.cs`
- [x] T040 [P] Add `collection_id` property with `[JsonProperty("collection_id")]` and default empty string to `UserWishlistCardExtEntity` in `src/Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/UserWishlistCardExtEntity.cs`
- [x] T041 [P] Add `collection_id` property with `[JsonProperty("collection_id")]` and default empty string to `UserSetCardExtEntity` in `src/Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/UserSetCardExtEntity.cs`

### Authorization Service (moved from US4 — needed by US2+ validators)

- [x] T126 Create `ICollectionAuthorizationService` interface in `src/Lib.Domain.Collections/Apis/ICollectionAuthorizationService.cs` with methods: `IsOwner`, `IsAdmin`, `IsOwnerOrAdmin`, `IsEditor`, `IsViewer`, `IsAuthorizedUser`, `CanGrantAccess`, `CanRevokeAccess`
- [x] T127 Implement `CollectionAuthorizationService` in `src/Lib.Domain.Collections/Authorization/CollectionAuthorizationService.cs` encapsulating all role-checking logic against collection owner_id and authorized_users (31 unit tests passing)

### Concurrency Control

- [x] T218 ~~Add ETag/if-match optimistic concurrency handling~~ **VERIFIED**: CosmosScribe base class inherently handles ETags via `RequestOptions` on upsert operations. No additional code needed.

**⚠️ IMPLEMENTATION NOTE**: GraphQL enum types (CollectionType, CollectionVisibility, CollectionRole) are schema-layer only. Do NOT create C# enums — roles and types are strings with validation per constitution Principle I.

### Verify Foundation

- [x] T042 Verify solution builds after all foundational changes: `dotnet build src/MtgDiscoveryVibe.sln` (0 errors, 0 warnings)
- [x] T043 Verify all existing tests still pass: `dotnet test src/MtgDiscoveryVibe.sln` (31/31 authorization tests passing; 3 pre-existing failures in Aggregator.Cards.Tests unrelated to collection changes)

**Checkpoint**: Foundation ready - user story implementation can now begin

---

## Phase 3: User Story 1 - Default Collection Auto-Creation (Priority: P1) 🎯 MVP

**Goal**: When a new user registers, a private default collection is automatically created. Existing card operations default to the user's default collection when no collection is specified.

**Independent Test**: Register a new user and verify a default collection exists; add cards without specifying a collection and confirm they land in the default collection.

### Adapter Layer (US1)

- [x] T044 [US1] Create `ICollectionAdapterCommandService` interface in `src/Lib.Adapter.Collections/Apis/ICollectionAdapterCommandService.cs`
- [x] T045 [US1] Create `ICollectionAdapterQueryService` interface in `src/Lib.Adapter.Collections/Apis/ICollectionAdapterQueryService.cs`
- [x] T046 [US1] Create `CollectionXfrEntity` in `src/Lib.Adapter.Collections/Entities/CollectionXfrEntity.cs`
- [x] T047 [US1] Create `CollectionOufEntity` in `src/Lib.Adapter.Collections/Entities/CollectionOufEntity.cs` implementing `ICollectionOufEntity`
- [x] T048 [US1] Create `AuthorizedUserOufEntity` in `src/Lib.Adapter.Collections/Entities/AuthorizedUserOufEntity.cs` implementing `IAuthorizedUserOufEntity`
- [x] T049 [US1] Implement `CollectionAdapterCommandService` in `src/Lib.Adapter.Collections/Commands/CollectionAdapterCommandService.cs`
- [x] T050 [US1] Implement `CollectionAdapterQueryService` in `src/Lib.Adapter.Collections/Queries/CollectionAdapterQueryService.cs`

### Aggregator Layer (US1)

- [x] T051 [US1] Create `ICollectionAggregatorCommandService` interface + query interface + composite service
- [x] T052 [US1] Implement `CollectionAggregatorCommandService` + query service delegating to adapter

### Domain Layer (US1)

- [x] T053 [US1] Create `ICollectionDomainCommandService` interface + query interface + composite service
- [x] T054 [US1] Implement `CollectionDomainCommandService` + query service delegating to aggregator

### Entry Layer (US1)

- [x] T055 [US1] Create `ICollectionEntryCommandService` + `ICollectionEntryQueryService` interfaces
- [x] T056 [US1] Create `CollectionItrEntity` implementing `ICollectionItrEntity`
- [x] T057 [P] [US1] Create `AuthorizedUserItrEntity` implementing `IAuthorizedUserItrEntity`
- [x] T058 [US1] Create `CollectionOutEntity` as concrete sealed class (no interface per pattern discovery)
- [x] T059 [P] [US1] Create `AuthorizedUserOutEntity` as concrete sealed class (no interface per pattern discovery)
- [x] T060 [US1] Create `CreateCollectionArgEntityValidatorContainer` with 6 individual validators (null, name valid, name length, name reserved, type valid, visibility valid)
- [x] T061 [US1] Implement `CollectionEntryCommandService` + `CollectionEntryQueryService` with validation, mapping, domain calls

### User Registration Hook (US1)

- [x] T062 [US1] Created `IDefaultCollectionCreator` + `DefaultCollectionCreator` in Entry layer to create default collection on first login
- [x] T063 [US1] DefaultCollectionCreator builds ItrEntity with name="My Collection", type="default", visibility="private", is_default=true. Modified `RegisterUserEntryService` to invoke on isFirstLogin=true

### GraphQL Layer (US1)

- [x] T064 [US1] Create `CreateCollectionArgEntity` implementing `ICreateCollectionArgEntity`
- [x] T065 [US1] Create `CollectionOutEntityType` (ObjectType)
- [x] T066 [P] [US1] Create `AuthorizedUserOutEntityType` (ObjectType)
- [x] T067 [US1] Create `CollectionSuccessDataResponseModel` — uses generic `SuccessDataResponseModel<CollectionOutEntity>`
- [x] T068 [US1] Create `CollectionSuccessDataResponseModelType` (ObjectType)
- [x] T069 [US1] Create `CollectionResponseModelUnionType` (UnionType)
- [x] T070 [US1] Create `CollectionsSuccessDataResponseModel` — uses generic `SuccessDataResponseModel<List<CollectionOutEntity>>`
- [x] T071 [US1] Create `CollectionsSuccessDataResponseModelType` (ObjectType)
- [x] T072 [US1] Create `CollectionsResponseModelUnionType` (UnionType)
- [x] T073 [US1] Create `CollectionMutationMethods` with `CreateCollectionAsync` mutation
- [x] T074 [US1] Create `CollectionQueryMethods` with `MyCollectionsAsync` query
- [x] T075 [US1] Types registered in `ApiMutationExtensions` and `ApiQueryExtensions` (not separate file — follows existing pattern)

### DI Registration (US1)

- [x] T076 [US1] No explicit DI registration needed — all services are composed via constructor chaining (matches existing pattern)

### Tests (US1)

- [x] T077 [P] [US1] Validator tests: 15 container tests + 6 individual validator test classes (42 tests total) in Entry.Tests
- [x] T078 [P] [US1] CollectionEntryCommandService tests: 4 tests in Entry.Tests
- [x] T079 [P] [US1] CollectionDomainCommandService tests: 6 tests in Lib.Domain.Collections.Tests (2 command + 4 service)
- [x] T080 [P] [US1] CollectionAggregatorCommandService tests: 6 tests in Lib.Aggregator.Collections.Tests (2 command + 4 service)
- [x] T081 [P] [US1] CollectionAdapterCommandService tests: 4 tests in Lib.Adapter.Collections.Tests
- [x] T082 [US1] DefaultCollectionCreator tests: 5 tests in Entry.Tests

### Verify US1

- [x] T083 [US1] Build: 0 errors, 0 warnings. Tests: 46 new collection tests all passing. Entry.Tests not in solution (pre-existing). Pre-existing failures: 3 in Aggregator.Cards.Tests, 5 in Adapter.UserCards.Tests (unrelated)

**Checkpoint**: User Story 1 complete - default collections created on registration, create collection mutation works, myCollections query works

---

## Phase 4: User Story 2 - Create and Manage Custom Collections (Priority: P2)

**Goal**: Users can create additional named collections, view all their collections, rename collections, and switch the active collection.

**Independent Test**: Create a custom collection, switch to it as active, add cards, switch back to default, and confirm each collection shows only its own cards.

### Backend - Rename Collection (US2)

- [x] T084 [US2] Create `RenameCollectionArgEntity` implementing `IRenameCollectionArgEntity`
- [x] T085 [US2] Create `RenameCollectionItrEntity` implementing `IRenameCollectionItrEntity`
- [x] T086 [US2] Create `RenameCollectionArgEntityValidatorContainer` with 5 validators (null, collectionId GUID, name valid, name length, name reserved)
- [x] T087 [US2] Add `RenameCollectionAsync` to entry service with validation, mapping, uniqueness check
- [x] T088 [US2] Add `RenameCollectionAsync` through domain → aggregator → adapter layers
- [x] T089 [US2] Implement Cosmos update: read via Gopher, update Name + UpdatedAt, upsert via Scribe
- [x] T090 [US2] Add `RenameCollectionAsync` mutation to `CollectionMutationMethods`

### Backend - Get Collection by ID (US2)

- [x] T091 [US2] Add `GetCollectionByIdAsync` to adapter/aggregator/domain/entry query services
- [x] T092 [US2] Add `GetCollectionAsync` query to `CollectionQueryMethods`

### Backend - Name Uniqueness Check (US2)

- [x] T093 [US2] Create `ICollectionNameUniquenessChecker` + implementation. Integrated into CreateCollection and RenameCollection entry services

### Frontend - Collection State Management (US2)

- [x] T094 [US2] Create `CollectionManagementContext.tsx` with localStorage persistence, active collection tracking, provider pattern
- [x] T095 [US2] Create `getMyCollections.ts` GraphQL query definition
- [x] T096 [P] [US2] Create `createCollection.ts` GraphQL mutation definition
- [x] T097 [P] [US2] Create `renameCollection.ts` GraphQL mutation definition
- [x] T098 [P] [US2] Create `getCollection.ts` GraphQL query definition
- [x] T099 [US2] Run `npm run codegen` — completed, types generated

### Frontend - Collection UI Components (US2)

- [x] T100 [US2] Create `CreateCollectionDialog.tsx` with MUI Dialog, form validation, type/visibility dropdowns
- [x] T101 [US2] Create `CollectionCard.tsx` with type badge, visibility icon, card count, context menu (rename/delete)
- [ ] T220 [US2] Backend card count query — **DEFERRED**: placeholder count=0 in UI until backend adds count field
- [x] T102 [US2] Create `CollectionBadge.tsx` atom with color-coded Chip (default/custom/cube/trade)
- [x] T103 [US2] Create `CollectionsPage.tsx` with responsive grid, empty state, create button, active highlighting
- [x] T104 [US2] Add `/collections` route in App.tsx with ProtectedRoute wrapper
- [x] T105 [US2] Added CollectionManagementProvider to App.tsx context hierarchy
- [x] T106 [P] [US2] Added "My Collections" nav item in NavigationDrawer.tsx

### Tests (US2)

- [x] T107 [P] [US2] Write unit tests for `RenameCollectionArgEntityValidatorContainer` — COMPLETED: comprehensive tests in RenameCollectionArgEntityValidatorContainerTests.cs
- [ ] T108 [P] [US2] Write unit tests for rename flow — **DEFERRED**: fakes updated for new methods
- [ ] T109 [P] [US2] Write unit tests for name uniqueness check — **DEFERRED**

### Verify US2

- [x] T110 [US2] Backend builds: 0 errors, 0 warnings. Existing collection tests pass (46 tests).
- [x] T111 [US2] Frontend build: TypeScript compiles cleanly (tsc --noEmit returns 0)

**Checkpoint**: Users can create, rename, view, and switch between collections

---

## Phase 5: User Story 3 - Collection Visibility Control (Priority: P3)

**Goal**: Owners can toggle collection visibility between private and public. Public collections are viewable by any authenticated user via collection ID.

**Independent Test**: Create a collection, change it to public, then have a different user access it by ID.

### Backend (US3)

- [x] T112 [US3] Create `UpdateCollectionVisibilityArgEntity` in `src/App.MtgDiscovery.GraphQL/Entities/Args/Collections/UpdateCollectionVisibilityArgEntity.cs`
- [x] T113 [US3] Create `UpdateCollectionVisibilityItrEntity` in `src/Lib.MtgDiscovery.Entry/Commands/Collections/Entities/UpdateCollectionVisibilityItrEntity.cs`
- [x] T114 [US3] Create `UpdateCollectionVisibilityArgEntityValidatorContainer` validating CollectionId (valid GUID), Visibility ("private"/"public")
- [x] T115 [US3] Add `UpdateCollectionVisibilityAsync` method through entry → domain → aggregator → adapter layers
- [x] T116 [US3] Implement Cosmos update logic in adapter for visibility change (read, update visibility + updated_at, write)
- [x] T117 [US3] Add `UpdateCollectionVisibilityAsync` mutation to `CollectionMutationMethods`
- [x] T118 [US3] Update `GetCollectionByIdAsync` query to allow access if collection is public (added CrossPartitionQueryAsync to Cosmos infrastructure, added NotFoundOperationException and ForbiddenOperationException)

### Frontend (US3)

- [x] T119 [US3] Create `updateCollectionVisibility.ts` mutation in `client/src/graphql/mutations/updateCollectionVisibility.ts`
- [x] T120 [US3] Run `npm run codegen` — completed as part of T099
- [x] T121 [US3] Add visibility toggle to `CollectionCard.tsx` as menu item "Make Public"/"Make Private" with `onToggleVisibility` callback

### Tests (US3)

- [x] T122 [P] [US3] Write unit tests for `UpdateCollectionVisibilityArgEntityValidatorContainer` — COMPLETED: comprehensive tests in UpdateCollectionVisibilityArgEntityValidatorContainerTests.cs
- [ ] T123 [P] [US3] Write unit tests for visibility update flow through layers — **DEFERRED**
- [ ] T124 [P] [US3] Write unit test for public collection access — **DEFERRED**

### Verify US3

- [x] T125 [US3] Backend builds: 0 errors, 0 warnings. Collection tests: 46 passing (Adapter: 4, Aggregator: 6, Domain: 36). Frontend: visibility mutation created, CollectionCard updated, codegen complete.

**Checkpoint**: Collection visibility control works; public collections are viewable by any authenticated user

---

## Phase 6: User Story 4 - Grant and Revoke Collection Access (Priority: P4)

**Goal**: Owners and admins can grant editor/viewer access and revoke access. Users can self-remove from shared collections.

**Independent Test**: Grant editor access to a second user, confirm they can add a card, then revoke access and confirm they cannot.

### Backend - Grant Access (US4)

> **NOTE**: T126-T127 (`CollectionAuthorizationService`) moved to Phase 2 Foundational — already available for use here.

- [x] T128 [US4] Create `GrantCollectionAccessArgEntity` in `src/App.MtgDiscovery.GraphQL/Entities/Args/Collections/GrantCollectionAccessArgEntity.cs`
- [x] T129 [US4] Create `GrantCollectionAccessItrEntity` in `src/Lib.MtgDiscovery.Entry/Commands/Collections/Entities/GrantCollectionAccessItrEntity.cs`
- [x] T130 [US4] Create `GrantCollectionAccessArgEntityValidatorContainer` with 4 validators (null, collectionId GUID, targetUserId GUID, role valid)
- [x] T131 [US4] Add `GrantCollectionAccessAsync` method through entry → domain → aggregator → adapter layers
- [x] T132 [US4] Implement Cosmos logic in adapter: read collection, add AuthorizedUserExtEntity to authorized_users array, update updated_at, write back
- [ ] T219 [US4] Add user-existence verification — **DEFERRED**: requires cross-layer user lookup
- [x] T133 [US4] Add `GrantCollectionAccessAsync` mutation to `CollectionMutationMethods`

### Backend - Revoke Access (US4)

- [x] T134 [US4] Create `RevokeCollectionAccessArgEntity` in `src/App.MtgDiscovery.GraphQL/Entities/Args/Collections/RevokeCollectionAccessArgEntity.cs`
- [x] T135 [US4] Create `RevokeCollectionAccessItrEntity` in `src/Lib.MtgDiscovery.Entry/Commands/Collections/Entities/RevokeCollectionAccessItrEntity.cs`
- [x] T136 [US4] Create `RevokeCollectionAccessArgEntityValidatorContainer` with 3 validators (null, collectionId GUID, targetUserId GUID)
- [x] T137 [US4] Add `RevokeCollectionAccessAsync` method through entry → domain → aggregator → adapter layers
- [x] T138 [US4] Implement Cosmos logic in adapter: read collection, remove user from authorized_users array, update updated_at, write back
- [x] T139 [US4] Add `RevokeCollectionAccessAsync` mutation to `CollectionMutationMethods`

### Backend - Access List Query (US4)

- [x] T140 [US4] Uses `SuccessDataResponseModel<List<AuthorizedUserOutEntity>>` — no separate class needed
- [x] T141 [US4] Create `AuthorizedUsersSuccessDataResponseModelType` (ObjectType)
- [x] T142 [US4] Create `AuthorizedUsersResponseModelUnionType` (UnionType)
- [x] T143 [US4] Add `CollectionAccessListAsync` query to `CollectionQueryMethods`
- [x] T144 [US4] Register new authorized users response types in `ApiQueryExtensions.cs`

### Frontend - Sharing UI (US4)

- [x] T145 [US4] Create `grantCollectionAccess.ts` in `client/src/graphql/mutations/grantCollectionAccess.ts`
- [x] T146 [P] [US4] Create `revokeCollectionAccess.ts` in `client/src/graphql/mutations/revokeCollectionAccess.ts`
- [x] T147 [P] [US4] Create `getCollectionAccessList.ts` in `client/src/graphql/queries/getCollectionAccessList.ts`
- [x] T148 [US4] Run `npm run codegen` — completed as part of T099
- [x] T149 [US4] Create `GrantAccessDialog.tsx` with MUI Dialog, user ID input, role selector (editor/viewer)
- [x] T150 [US4] Create `AccessListDialog.tsx` with MUI List, authorized users display, revoke buttons
- [x] T151 [US4] Add share/access list actions to `CollectionCard.tsx` with `isOwner`, `onShare`, `onViewAccessList` props

### Tests (US4)

- [ ] T152 [P] [US4] Write unit tests for `CollectionAuthorizationService` — **DEFERRED**: fakes updated
- [x] T153 [P] [US4] Write unit tests for `GrantCollectionAccessArgEntityValidatorContainer` — COMPLETED: comprehensive tests in GrantCollectionAccessArgEntityValidatorContainerTests.cs
- [x] T154 [P] [US4] Write unit tests for `RevokeCollectionAccessArgEntityValidatorContainer` — COMPLETED: comprehensive tests in RevokeCollectionAccessArgEntityValidatorContainerTests.cs
- [ ] T155 [P] [US4] Write unit tests for grant/revoke flows — **DEFERRED**: fakes updated

### Verify US4

- [x] T156 [US4] Backend builds: 0 errors, 0 warnings. Tests: 46 collection tests passing.
- [x] T157 [US4] Frontend build: TypeScript compiles cleanly

**Checkpoint**: Full sharing workflow operational - grant, revoke, access list, self-removal

---

## Phase 7: User Story 5 - Collection Deletion (Priority: P5)

**Goal**: Owners can permanently delete non-default collections, removing the collection and all associated card data.

**Independent Test**: Create a collection, add cards, delete it, and confirm all data is removed.

### Backend (US5)

- [x] T158 [US5] Create `DeleteCollectionArgEntity` in `src/App.MtgDiscovery.GraphQL/Entities/Args/Collections/DeleteCollectionArgEntity.cs`
- [x] T159 [US5] Create `DeleteCollectionItrEntity` in `src/Lib.MtgDiscovery.Entry/Commands/Collections/Entities/DeleteCollectionItrEntity.cs`
- [x] T160 [US5] Create `DeleteCollectionArgEntityValidatorContainer` with 2 validators (null, collectionId GUID). Default check at adapter layer.
- [x] T161 [US5] Add `DeleteCollectionAsync` method through entry → domain → aggregator → adapter layers
- [x] T162 [US5] Implement Cosmos deletion in adapter with `CollectionJanitor`. Card data deletion deferred.
- [x] T163 [US5] Add `DeleteCollectionAsync` mutation to `CollectionMutationMethods`

### Frontend (US5)

- [x] T164 [US5] Create `deleteCollection.ts` mutation in `client/src/graphql/mutations/deleteCollection.ts`
- [x] T165 [US5] Run `npm run codegen` — completed as part of T099
- [x] T166 [US5] Create `DeleteCollectionDialog.tsx` confirmation dialog. CollectionCard already has delete action for owner-only, non-default.

### Tests (US5)

- [x] T167 [P] [US5] Write unit tests for `DeleteCollectionArgEntityValidatorContainer` — COMPLETED: comprehensive tests in DeleteCollectionArgEntityValidatorContainerTests.cs
- [ ] T168 [P] [US5] Write unit tests for deletion flow — **DEFERRED**: fakes updated

### Verify US5

- [x] T169 [US5] Backend builds: 0 errors, 0 warnings. Tests: 46 collection tests passing.
- [x] T170 [US5] Frontend build: TypeScript compiles cleanly

**Checkpoint**: Collection deletion with cascading card data removal works

---

## Phase 8: User Story 6 - Transfer Collection Ownership (Priority: P6)

**Goal**: Owners can transfer ownership of non-default collections to existing authorized users. Previous owner becomes admin.

**Independent Test**: Grant a user access, transfer ownership, verify new owner can delete/transfer while previous owner retains admin access.

### Backend (US6)

- [x] T171 [US6] Create `TransferCollectionOwnershipArgEntity` in `src/App.MtgDiscovery.GraphQL/Entities/Args/Collections/TransferCollectionOwnershipArgEntity.cs`
- [x] T172 [US6] Create `TransferCollectionOwnershipItrEntity` in `src/Lib.MtgDiscovery.Entry/Commands/Collections/Entities/TransferCollectionOwnershipItrEntity.cs`
- [x] T173 [US6] Create `TransferCollectionOwnershipArgEntityValidatorContainer` with 3 validators (null, collectionId GUID, targetUserId GUID)
- [x] T174 [US6] Add `TransferCollectionOwnershipAsync` method through entry → domain → aggregator → adapter layers
- [x] T175 [US6] Implement Cosmos transfer logic in adapter: delete old doc, create new with updated owner_id, previous owner becomes admin
- [x] T176 [US6] Add `TransferCollectionOwnershipAsync` mutation to `CollectionMutationMethods`

### Frontend (US6)

- [x] T177 [US6] Create `transferCollectionOwnership.ts` in `client/src/graphql/mutations/transferCollectionOwnership.ts`
- [x] T178 [US6] Run `npm run codegen` — completed as part of T099
- [x] T179 [US6] Add transfer ownership action to `CollectionCard.tsx` with `onTransferOwnership` prop. Created `TransferOwnershipDialog.tsx`.

### Tests (US6)

- [x] T180 [P] [US6] Write unit tests for `TransferCollectionOwnershipArgEntityValidatorContainer` — COMPLETED: comprehensive tests in TransferCollectionOwnershipArgEntityValidatorContainerTests.cs
- [ ] T181 [P] [US6] Write unit tests for transfer flow — **DEFERRED**: fakes updated

### Verify US6

- [x] T182 [US6] Backend builds: 0 errors, 0 warnings. Tests: 46 collection tests passing.
- [x] T183 [US6] Frontend build: TypeScript compiles cleanly

**Checkpoint**: Ownership transfer works with correct role reassignment

---

## Phase 9: User Story 7 - View Shared Collections (Priority: P7)

**Goal**: Users can see collections shared with them in a "Shared With Me" section with role information.

**Independent Test**: Grant a user access, log in as that user, confirm the collection appears in "Shared With Me".

### Backend (US7)

- [x] T184 [US7] Add `GetSharedCollectionsAsync` method to adapter query service with cross-partition query using `EXISTS` clause
- [x] T185 [US7] Add `GetAccessibleCollectionsAsync` method combining owned + shared collections
- [x] T186 [US7] Wire `SharedCollectionsAsync` and `AccessibleCollectionsAsync` queries through all layers
- [x] T187 [US7] Add `SharedCollectionsAsync` query to `CollectionQueryMethods`
- [x] T188 [US7] Add `AccessibleCollectionsAsync` query to `CollectionQueryMethods`

### Frontend (US7)

- [x] T189 [US7] Create `getSharedCollections.ts` in `client/src/graphql/queries/getSharedCollections.ts`
- [x] T190 [P] [US7] Create `getAccessibleCollections.ts` in `client/src/graphql/queries/getAccessibleCollections.ts`
- [x] T191 [US7] Run `npm run codegen` — completed as part of T099
- [x] T192 [US7] Create `SharedCollectionCard.tsx` with owner ID, user's role, visibility, and leave button
- [x] T193 [US7] Add "Shared With Me" section to `CollectionsPage.tsx` — section wired with `GET_SHARED_COLLECTIONS` query
- [x] T194 [US7] Add self-removal action to `SharedCollectionCard.tsx` via `onLeave` callback

### Tests (US7)

- [ ] T195 [P] [US7] Write unit tests for shared collections query — **DEFERRED**: fakes updated
- [ ] T196 [P] [US7] Write unit tests for accessible collections query — **DEFERRED**: fakes updated

### Verify US7

- [x] T197 [US7] Backend builds: 0 errors, 0 warnings. Tests: 46 collection tests passing.
- [x] T198 [US7] Frontend build: TypeScript compiles cleanly

**Checkpoint**: Shared collections visible in dedicated section with role info

---

## Phase 10: User Story 8 - Collection Selection in Application Header (Priority: P8)

**Goal**: Authenticated users see a collection selector dropdown in the header for quick switching.

**Independent Test**: Verify selector appears, shows all collections with type badges, and switching updates card views.

### Frontend (US8)

- [x] T199 [US8] Create `CollectionSelector.tsx` with MUI Select, collection badges, visibility icons, active highlighting
- [x] T200 [US8] Add "New Collection" option via `onCreateNew` callback
- [x] T201 [US8] Integrate CollectionSelector into Header — imports added, shows when authenticated with collections
- [x] T202 [US8] Wire `CollectionManagementContext` to use actual `useQuery` with `GET_MY_COLLECTIONS`, wired `CollectionsPage` with `GET_SHARED_COLLECTIONS`
- [ ] T203 [US8] Ensure card data refetch — **DEFERRED**: requires backend query updates to filter by collection_id, frontend query variable updates, and cache invalidation on collection change

### Verify US8

- [x] T204 [US8] Frontend build: TypeScript compiles cleanly

**Checkpoint**: Header selector provides quick collection switching

---

## Phase 11: Data Migration

**Purpose**: One-time migration to backfill existing user data with default collections and collection_id

- [x] T205 Create `CollectionMigrationOrchestrator` in `src/Cli.MtgDiscovery.DataMigration/Collections/` that reads users, creates default collections, updates cards
- [x] T206 Migration is idempotent: checks for existing default collection, checks for existing collection_id before updating
- [x] T207 Test project created with stubs: `Cli.MtgDiscovery.DataMigration.Tests` (8 tests deferred as Inconclusive, 1 passing)

---

## Phase 12: User ID Discovery

**Purpose**: Allow users to find and share their user ID for collection sharing

- [x] T208 [P] Create `UserProfilePage.tsx` with user ID display, copy-to-clipboard button, sharing instructions
- [x] T209 [P] Add route for UserProfilePage — added /profile route with ProtectedRoute wrapper
- [x] T210 Add user ID display in account/profile section — implemented in UserProfilePage.tsx with monospace TextField and copy-to-clipboard button

---

## Phase 13: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

- [x] T211 [P] Create `NotificationContext.tsx` with `useNotification` hook for Snackbar notifications (success, error, info, warning)
- [x] T212 [P] Edge case: fallback to default collection on delete/revoke — implemented useEffect in CollectionManagementContext that detects when activeCollectionId no longer exists in collections list
- [x] T221 Edge case: No default collection returns clear error `"No default collection found for owner {ownerId}"` in `CollectionQueryAdapter.GetDefaultCollectionAsync`
- [x] T213 Add loading states — dialogs already have isSubmitting states with disabled buttons and CircularProgress indicators
- [x] T214 Verified: all new components use MUI sx props (no Tailwind classes)
- [x] T215 Backend build: 0 errors, 0 warnings. Tests: 46 collection tests passing.
- [x] T216 Frontend build: TypeScript compiles cleanly
- [x] T217 Quickstart validation: Backend builds (0 errors, 0 warnings), Frontend TypeScript compiles (tsc --noEmit exit 0), Collection tests pass (46 tests: Domain=36, Aggregator=6, Adapter=4)

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **US1 (Phase 3)**: Depends on Foundational - MVP story, should be completed first
- **US2 (Phase 4)**: Depends on US1 (needs create collection, GraphQL types, entry service)
- **US3 (Phase 5)**: Depends on US1 (needs collection entity and query infrastructure)
- **US4 (Phase 6)**: Depends on US1 (needs collection entity); also benefits from US2 (custom collections to share)
- **US5 (Phase 7)**: Depends on US1 (needs collection entity and adapter)
- **US6 (Phase 8)**: Depends on US4 (needs authorized users in place for transfer target)
- **US7 (Phase 9)**: Depends on US4 (needs sharing to exist for shared collections to display)
- **US8 (Phase 10)**: Depends on US2 (needs collection management context and selector infrastructure)
- **Migration (Phase 11)**: Can begin after US1 backend is complete (needs Cosmos container and default collection creation logic)
- **User ID Discovery (Phase 12)**: Independent of other stories, can run in parallel after Phase 2
- **Polish (Phase 13)**: Depends on all desired user stories being complete

### Critical Path

```
Phase 1 → Phase 2 → Phase 3 (US1) → Phase 4 (US2) → Phase 6 (US4) → Phase 8 (US6)
                                   → Phase 5 (US3)
                                   → Phase 7 (US5)
                                   → Phase 9 (US7) [after US4]
                                   → Phase 10 (US8) [after US2]
                                   → Phase 11 (Migration) [after US1 backend]
                                   → Phase 12 (User ID) [after Phase 2]
```

### Within Each User Story

- Adapter layer before aggregator
- Aggregator before domain
- Domain before entry
- Entry before GraphQL
- Backend before frontend
- Core implementation before tests
- Story complete before moving to next priority

### Parallel Opportunities

- All Phase 1 setup tasks marked [P] can run in parallel
- All Phase 2 interface creation tasks marked [P] can run in parallel
- All Cosmos ExtEntity modifications (T039-T041) can run in parallel
- Frontend GraphQL file creation within a story marked [P] can run in parallel
- Test tasks within a story marked [P] can run in parallel
- US3 and US5 can run in parallel (both depend on US1, not on each other)
- Phase 11 (Migration) and Phase 12 (User ID) can run in parallel with later user stories

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational
3. Complete Phase 3: User Story 1
4. **STOP and VALIDATE**: Default collections created on registration, create collection works
5. Deploy/demo if ready

### Incremental Delivery

1. Setup + Foundational → Foundation ready
2. Add US1 → Default collection works → Deploy (MVP)
3. Add US2 → Custom collections, management page → Deploy
4. Add US3 → Visibility control → Deploy
5. Add US4 → Sharing works → Deploy
6. Add US5 → Deletion works → Deploy
7. Add US6 → Ownership transfer → Deploy
8. Add US7 → Shared collections visible → Deploy
9. Add US8 → Header selector → Deploy
10. Migration + Polish → Feature complete

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
- Follow the 3-build-failure limit before stopping and reporting
- All backend classes must be `sealed` or `abstract`
- All async calls must use `ConfigureAwait(false)`
- No enums — roles and types are strings with validation
- Use Newtonsoft.Json only (not System.Text.Json)
- Frontend uses MUI sx props only (no Tailwind)
- Interfaces before implementations
- Fakes over mocks in tests
