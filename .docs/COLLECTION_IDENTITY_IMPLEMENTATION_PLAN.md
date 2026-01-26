# Collection Identity Architecture - Implementation Plan

## Overview

This document details the implementation plan for transitioning from the current 1:1 User:Collection model to a first-class Collection entity model. The work is broken down into three epics, each with features and tasks.

---

## Epic 1: Core Infrastructure - Collection Entity Foundation

**Goal**: Establish the database, backend architecture, and authorization model to support collections as first-class entities. Users always have access to their "default" collection (matching their userId). Users can create additional collections and be granted permissions on other users' collections. At this stage, users cannot change their active collection - it's always assumed to be their default collection.

### Feature 1.1: Collection Entity Model

**Description**: Create the Collection entity with supporting data structures across all layers.

#### Tasks:

##### Task 1.1.1: Define Collection Cosmos Schema
- **File**: Create `src/Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/CollectionExtEntity.cs`
- **Schema**:
  ```csharp
  {
    id: string,                    // Collection ID (GUID)
    owner_id: string,              // User ID of collection owner
    name: string,                  // Collection display name
    type: string,                  // "default" | "custom" | "cube" | "trade"
    visibility: string,            // "private" | "public"
    is_default: bool,              // True for user's default collection
    authorized_users: [            // Array of AuthorizedUserExtEntity
      {
        user_id: string,
        role: string,              // "owner" | "editor" | "viewer"
        granted_at: string,        // ISO timestamp
        granted_by: string         // User ID who granted access
      }
    ],
    created_at: string,            // ISO timestamp
    updated_at: string             // ISO timestamp
  }
  ```
- **Partition Key**: `owner_id`
- **Container Name**: `Collections`
- **Dependencies**: None
- **Testing**: Unit tests for ExtEntity structure

##### Task 1.1.2: Create Collection Entity Interfaces (Shared Layer)
- **Files to Create**:
  - `src/Lib.Shared.DataModels/Entities/Args/ICollectionArgEntity.cs` - GraphQL input
  - `src/Lib.Shared.DataModels/Entities/Args/IAuthorizedUserArgEntity.cs` - Authorization input
  - `src/Lib.Shared.DataModels/Entities/Args/IUpdateCollectionVisibilityArgEntity.cs` - Visibility update input (`CollectionId`, `Visibility`)
  - `src/Lib.Shared.DataModels/Entities/Itrs/ICollectionItrEntity.cs` - Internal transfer
  - `src/Lib.Shared.DataModels/Entities/Itrs/IAuthorizedUserItrEntity.cs` - Authorization transfer
  - `src/Lib.Shared.DataModels/Entities/Itrs/IUpdateCollectionVisibilityItrEntity.cs` - Visibility update transfer
  - `src/Lib.Shared.DataModels/Entities/Outs/ICollectionOutEntity.cs` - GraphQL output
  - `src/Lib.Shared.DataModels/Entities/Outs/IAuthorizedUserOutEntity.cs` - Authorization output
- **Properties** (align with Cosmos schema):
  - `CollectionId`, `OwnerId`, `Name`, `Type`, `Visibility`, `IsDefault`, `AuthorizedUsers`, `CreatedAt`, `UpdatedAt`
- **Dependencies**: Task 1.1.1
- **Testing**: Interface contracts verified by implementations

##### Task 1.1.3: Implement Collection Entity Classes
- **Files to Create**:
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Entities/CollectionArgEntity.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Entities/AuthorizedUserArgEntity.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Entities/CollectionItrEntity.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Entities/AuthorizedUserItrEntity.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Entities/CollectionOutEntity.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Entities/AuthorizedUserOutEntity.cs`
- **Pattern**: Follow existing entity implementation patterns (sealed classes with private readonly fields)
- **Dependencies**: Task 1.1.2
- **Testing**: Unit tests for each entity class

##### Task 1.1.4: Create Collection Cosmos Container
- **File**: Create `src/Lib.Adapter.Scryfall.Cosmos/Apis/Containers/CollectionsCosmosContainer.cs`
- **Pattern**: Extend `CosmosContainer<CollectionExtEntity>`
- **Configuration**:
  - Container name: "Collections"
  - Partition key: "/owner_id"
  - Throughput: 400 RU/s (manual, adjust based on usage)
- **Dependencies**: Task 1.1.1
- **Testing**: Integration tests for container creation/access

##### Task 1.1.5: Create Collection Operators (Gopher, Scribe, Inquisitor)
- **Files to Create**:
  - `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Gophers/CollectionGopher.cs` - Read single collection
  - `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Scribes/CollectionScribe.cs` - Write collection
  - `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitors/CollectionsInquisitor.cs` - Query collections
- **Responsibilities**:
  - CollectionGopher: Get collection by ID
  - CollectionScribe: Create, update, delete collections
  - CollectionsInquisitor: Query collections by owner, query collections by authorized user
- **Dependencies**: Task 1.1.4
- **Testing**: Unit tests with fake Cosmos client

---

### Feature 1.2: Default Collection Creation

**Description**: Automatically create a default collection for each user during registration.

#### Tasks:

##### Task 1.2.1: Extend User Registration Flow - Entry Layer
- **File**: Modify `src/Lib.MtgDiscovery.Entry/Commands/User/RegisterUserEntryService.cs`
- **Changes**:
  - After user registration succeeds, trigger default collection creation
  - Add `ICollectionEntryService` dependency
  - Call `CreateDefaultCollectionAsync(userId, userNickname)`
- **Dependencies**: Feature 1.3 (Collection Entry Service)
- **Testing**: Unit tests verifying default collection creation

##### Task 1.2.2: Extend User Registration Flow - Domain Layer
- **File**: Modify `src/Lib.Domain.User/Commands/RegisterUserDomainService.cs`
- **Changes**: Passthrough to aggregator (align with existing pattern)
- **Dependencies**: Task 1.2.1
- **Testing**: Unit tests

##### Task 1.2.3: Extend User Registration Flow - Aggregator Layer
- **File**: Modify `src/Lib.Aggregator.User/Commands/RegisterUserAggregatorService.cs`
- **Changes**:
  - After user creation, create default collection
  - Collection properties:
    - `id`: New GUID
    - `owner_id`: `userId`
    - `name`: "Default Collection" (or user's nickname + "'s Collection")
    - `type`: "default"
    - `visibility`: "private"
    - `is_default`: true
    - `authorized_users`: [{ user_id: userId, role: "owner", granted_at: now, granted_by: userId }]
- **Dependencies**: Feature 1.4 (Collection Aggregator Service)
- **Testing**: Unit tests with fake adapters

---

### Feature 1.3: Collection Entry Service

**Description**: Entry service layer for collection operations with validation.

#### Tasks:

##### Task 1.3.1: Create Collection Entry Service Interface
- **File**: Create `src/Lib.MtgDiscovery.Entry/Commands/Collections/ICollectionEntryService.cs`
- **Methods**:
  - `Task<IOperationResponse<ICollectionOutEntity>> CreateCollectionAsync(ICreateCollectionArgEntity args)`
  - `Task<IOperationResponse<ICollectionOutEntity>> CreateDefaultCollectionAsync(string userId, string userNickname)`
  - `Task<IOperationResponse<ICollectionOutEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityArgEntity args)`
  - `Task<IOperationResponse<ICollectionOutEntity>> DeleteCollectionAsync(IDeleteCollectionArgEntity args)`
  - `Task<IOperationResponse<ICollectionOutEntity>> GetCollectionAsync(string collectionId)`
  - `Task<IOperationResponse<ICollectionOutEntity[]>> GetUserCollectionsAsync(string userId)`
  - `Task<IOperationResponse<ICollectionOutEntity[]>> GetAccessibleCollectionsAsync(string userId)`
- **Dependencies**: Task 1.1.2
- **Testing**: Interface verified by implementation

##### Task 1.3.2: Create Collection Validators
- **Files to Create**:
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/CreateCollectionArgEntityValidatorContainer.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/CreateCollectionArgEntityValidator_HasValidUserId.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/CreateCollectionArgEntityValidator_HasValidName.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/CreateCollectionArgEntityValidator_HasValidType.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/CreateCollectionArgEntityValidator_HasValidVisibility.cs`
- **Validation Rules**:
  - UserId: Not null, not empty, valid GUID format
  - Name: Not null, not empty, max 100 characters, no reserved names ("default")
  - Type: Valid enum value ("custom" | "cube" | "trade")
  - Visibility: Valid value ("private" | "public"), defaults to "private" if not provided
- **Dependencies**: Task 1.1.2
- **Testing**: Unit tests for each validator

##### Task 1.3.3: Implement Collection Entry Service
- **File**: Create `src/Lib.MtgDiscovery.Entry/Commands/Collections/CreateCollectionEntryService.cs`
- **Responsibilities**:
  - Validate input using validator container
  - Map ArgEntity → ItrEntity
  - Delegate to `ICollectionDomainService`
  - Map OufEntity → OutEntity
  - Handle operation responses
- **Dependencies**: Tasks 1.3.1, 1.3.2, Feature 1.4
- **Testing**: Unit tests with fake domain service

##### Task 1.3.4: Implement Get Collection Entry Services
- **Files to Create**:
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/GetCollectionEntryService.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/GetUserCollectionsEntryService.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/GetAccessibleCollectionsEntryService.cs`
- **Pattern**: Follow existing query entry service patterns
- **Dependencies**: Task 1.3.1, Feature 1.4
- **Testing**: Unit tests with fake domain service

---

### Feature 1.4: Collection Domain Service

**Description**: Domain service layer for collection business logic.

#### Tasks:

##### Task 1.4.1: Create Collection Domain Service Interface
- **File**: Create `src/Lib.Domain.Collections/Commands/ICollectionDomainService.cs`
- **Methods**: Match `ICollectionEntryService` signatures (ItrEntity instead of ArgEntity)
  - `Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(ICollectionItrEntity collection)`
  - `Task<IOperationResponse<ICollectionOufEntity>> CreateDefaultCollectionAsync(string userId, string userNickname)`
  - `Task<IOperationResponse<ICollectionOufEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityItrEntity args)`
  - `Task<IOperationResponse<ICollectionOufEntity>> DeleteCollectionAsync(IDeleteCollectionItrEntity args)`
  - `Task<IOperationResponse<ICollectionOufEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipItrEntity args)`
  - `Task<IOperationResponse<ICollectionOufEntity>> GetCollectionAsync(string collectionId)`
  - `Task<IOperationResponse<ICollectionOufEntity[]>> GetUserCollectionsAsync(string userId)`
  - `Task<IOperationResponse<ICollectionOufEntity[]>> GetAccessibleCollectionsAsync(string userId)`
- **Dependencies**: Task 1.1.2
- **Testing**: Interface verified by implementation

##### Task 1.4.2: Implement Collection Domain Service
- **File**: Create `src/Lib.Domain.Collections/Commands/CreateCollectionDomainService.cs` (and others for Get operations)
- **Business Rules**:
  - Users can only have ONE default collection
  - Collection names must be unique per user
  - Owner is automatically added to `authorized_users` with "owner" role
- **Pattern**: Passthrough to aggregator (align with existing domain services)
- **Dependencies**: Task 1.4.1, Feature 1.5
- **Testing**: Unit tests with fake aggregator service

---

### Feature 1.5: Collection Aggregator Service

**Description**: Aggregator service layer for collection data orchestration.

#### Tasks:

##### Task 1.5.1: Create Collection Aggregator Service Interface
- **File**: Create `src/Lib.Aggregator.Collections/Commands/ICollectionAggregatorService.cs`
- **Methods**: Match domain service (using ItrEntity)
- **Dependencies**: Task 1.1.2
- **Testing**: Interface verified by implementation

##### Task 1.5.2: Implement Collection Aggregator Service
- **Files to Create**:
  - `src/Lib.Aggregator.Collections/Commands/CreateCollectionAggregatorService.cs`
  - `src/Lib.Aggregator.Collections/Commands/GetCollectionAggregatorService.cs`
  - `src/Lib.Aggregator.Collections/Commands/GetUserCollectionsAggregatorService.cs`
  - `src/Lib.Aggregator.Collections/Commands/GetAccessibleCollectionsAggregatorService.cs`
- **Responsibilities**:
  - Map ItrEntity → XfrEntity
  - Call adapter services
  - Map ExtEntity → OufEntity
  - Handle operation responses
- **Dependencies**: Task 1.5.1, Feature 1.6
- **Testing**: Unit tests with fake adapters

---

### Feature 1.6: Collection Adapter Service

**Description**: Adapter service layer for collection external system integration.

#### Tasks:

##### Task 1.6.1: Create Collection Adapter Service Interfaces
- **Files to Create**:
  - `src/Lib.Adapter.Collections/Commands/ICollectionCommandAdapter.cs`
  - `src/Lib.Adapter.Collections/Queries/ICollectionQueryAdapter.cs`
- **Methods**:
  - Command: `CreateCollectionAsync`, `UpdateCollectionAsync`, `UpdateCollectionVisibilityAsync`, `DeleteCollectionAsync`, `TransferCollectionOwnershipAsync`
  - Query: `GetCollectionAsync`, `GetUserCollectionsAsync`, `GetAccessibleCollectionsAsync`
- **Dependencies**: Task 1.1.2
- **Testing**: Interface verified by implementation

##### Task 1.6.2: Implement Collection Adapter Services
- **Files to Create**:
  - `src/Lib.Adapter.Collections/Commands/CollectionCommandAdapter.cs`
  - `src/Lib.Adapter.Collections/Queries/CollectionQueryAdapter.cs`
- **Responsibilities**:
  - Map XfrEntity → ExtEntity
  - Call Cosmos operators (Gopher, Scribe, Inquisitor)
  - Map ExtEntity → OufEntity
  - Return `IOperationResponse<T>`
- **Dependencies**: Task 1.6.1, Task 1.1.5
- **Testing**: Unit tests with fake operators

---

### Feature 1.7: Collection Authorization Logic

**Description**: Authorization validation for collection access.

#### Tasks:

##### Task 1.7.1: Create Authorization Helper Service
- **File**: Create `src/Lib.Domain.Collections/Authorization/ICollectionAuthorizationService.cs`
- **Methods**:
  - `Task<bool> CanUserAccessCollectionAsync(string userId, string collectionId, string requiredRole)`
  - `Task<bool> IsUserPrimaryOwnerAsync(string userId, string collectionId)` — checks `owner_id` field
  - `Task<bool> IsUserCollectionOwnerAsync(string userId, string collectionId)` — checks `authorized_users` for "owner" role
  - `Task<string> GetUserRoleInCollectionAsync(string userId, string collectionId)`
  - `Task<bool> CanUserViewCollectionAsync(string userId, string collectionId)`
- **Roles Hierarchy**: owner > editor > viewer
- **Primary Owner vs Co-Owner**:
  - Primary owner (`owner_id`): Can transfer, delete, change visibility
  - Co-owner (role "owner" in `authorized_users`): Full CRUD + sharing, but cannot transfer, delete, or change visibility
- **Dependencies**: Feature 1.5
- **Testing**: Unit tests with various authorization scenarios

##### Task 1.7.2: Implement Authorization Helper Service
- **File**: Create `src/Lib.Domain.Collections/Authorization/CollectionAuthorizationService.cs`
- **Logic**:
  - Fetch collection by ID
  - For view access: If collection visibility is `public`, any authenticated user can view. If `private`, check `authorized_users` for viewer+ role.
  - For edit/owner access: Always check `authorized_users` array regardless of visibility
  - Validate role meets requirement
- **Dependencies**: Task 1.7.1
- **Testing**: Unit tests covering:
  - Owner can perform all operations
  - Editor can add/remove cards
  - Viewer can only read
  - Non-authorized user cannot access private collection
  - Any authenticated user can view public collection
  - Non-authorized user cannot edit public collection

##### Task 1.7.3: Create Collection Authorization Validators
- **Files to Create**:
  - `src/Lib.MtgDiscovery.Entry/Commands/Validators/AuthUserCanAccessCollectionValidator.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Validators/AuthUserIsCollectionOwnerValidator.cs` — checks "owner" role in `authorized_users`
  - `src/Lib.MtgDiscovery.Entry/Commands/Validators/AuthUserIsPrimaryCollectionOwnerValidator.cs` — checks `owner_id` field (for delete, transfer, visibility)
- **Usage**: Add to validator containers for collection mutations
- **Dependencies**: Task 1.7.2
- **Testing**: Unit tests with fake authorization service

---

### Feature 1.8: Migrate UserCards Schema

**Description**: Add `collection_id` field to UserCards, UserWishlistCards, and UserSetCards containers.

#### Tasks:

##### Task 1.8.1: Update UserCards ExtEntity Schema
- **File**: Modify `src/Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/UserCardExtEntity.cs`
- **Add Property**: `public string CollectionId { get; init; } = string.Empty;`
- **Migration Consideration**: Existing documents will not have this field (handled in Task 1.8.5)
- **Dependencies**: None
- **Testing**: Unit tests for updated schema

##### Task 1.8.2: Update UserWishlistCards ExtEntity Schema
- **File**: Modify `src/Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/UserWishlistCardExtEntity.cs`
- **Add Property**: `public string CollectionId { get; init; } = string.Empty;`
- **Dependencies**: None
- **Testing**: Unit tests for updated schema

##### Task 1.8.3: Update UserSetCards ExtEntity Schema
- **File**: Modify `src/Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/UserSetCardExtEntity.cs`
- **Add Property**: `public string CollectionId { get; init; } = string.Empty;`
- **Dependencies**: None
- **Testing**: Unit tests for updated schema

##### Task 1.8.4: Update Entity Interfaces (Arg, Itr, Xfr, Out)
- **Files to Modify** (add `CollectionId` property):
  - `src/Lib.Shared.DataModels/Entities/Args/IAddUserCardArgEntity.cs`
  - `src/Lib.Shared.DataModels/Entities/Itrs/IUserCardItrEntity.cs`
  - `src/Lib.Shared.DataModels/Entities/Xfrs/IAddUserCardXfrEntity.cs`
  - `src/Lib.Shared.DataModels/Entities/Outs/IUserCardOutEntity.cs`
  - (Repeat for UserWishlistCards and UserSetCards)
- **Dependencies**: Tasks 1.8.1, 1.8.2, 1.8.3
- **Testing**: Interface contracts verified by implementations

##### Task 1.8.5: Create Data Migration Script
- **File**: Create `src/Example.Cosmos.Migration/MigrateUserCardsToCollections.cs`
- **Migration Steps**:
  1. Query all UserInfo documents
  2. For each user:
     - Create default collection (if not exists) with `visibility: "private"`
     - Query all UserCards documents (partition key = user_id)
     - Update each document with `collection_id = default_collection_id`
     - Repeat for UserWishlistCards
     - Repeat for UserSetCards
  3. Log migration progress and errors
- **Run Once**: Manual execution, idempotent
- **Dependencies**: Feature 1.1, Feature 1.2
- **Testing**: Integration tests with test database

##### Task 1.8.6: Update Collection Mutation Validators
- **Files to Modify**:
  - `src/Lib.MtgDiscovery.Entry/Commands/UserCards/Validators/AddCardToCollectionArgEntityValidatorContainer.cs`
  - Add: `AddCardToCollectionArgEntityValidator_HasValidCollectionId.cs`
  - Add: `AddCardToCollectionArgEntityValidator_AuthUserCanAccessCollection.cs`
- **Validation Rules**:
  - CollectionId: Not null, not empty, valid GUID format
  - Authorization: User has "editor" or "owner" role in collection
- **Dependencies**: Task 1.8.4, Feature 1.7
- **Testing**: Unit tests for validators

---

### Feature 1.9: GraphQL Collection Schema

**Description**: Add GraphQL types, queries, and mutations for collections.

#### Tasks:

##### Task 1.9.1: Create Collection GraphQL Types
- **Files to Create**:
  - `src/App.MtgDiscovery.GraphQL/Entities/Types/CollectionType.cs`
  - `src/App.MtgDiscovery.GraphQL/Entities/Types/AuthorizedUserType.cs`
  - `src/App.MtgDiscovery.GraphQL/Entities/CollectionResponseModels.cs`
- **Types**:
  - `Collection`: Maps to `ICollectionOutEntity`
  - `AuthorizedUser`: Maps to `IAuthorizedUserOutEntity`
  - Response unions: `CollectionSuccessResponse`, `CollectionsSuccessResponse`, `FailureResponse`
- **Dependencies**: Feature 1.3
- **Testing**: Schema validation tests

##### Task 1.9.2: Create Collection Query Methods
- **File**: Create `src/App.MtgDiscovery.GraphQL/Queries/CollectionQueryMethods.cs`
- **Queries**:
  - `getCollection(collectionId: ID!): CollectionResponse!`
  - `myCollections: CollectionsResponse!` (authenticated)
  - `accessibleCollections: CollectionsResponse!` (authenticated, includes shared collections)
- **Pattern**: `[ExtendObjectType("Query")]` with `[Authorize]` where needed
- **Dependencies**: Task 1.9.1, Feature 1.3
- **Testing**: Integration tests with test database

##### Task 1.9.3: Create Collection Mutation Methods
- **File**: Create `src/App.MtgDiscovery.GraphQL/Mutations/CollectionMutationMethods.cs`
- **Mutations**:
  - `createCollection(args: CreateCollectionInput!): CollectionResponse!` (authenticated)
  - `updateCollectionVisibility(args: UpdateCollectionVisibilityInput!): CollectionResponse!` (authenticated, primary owner only)
- **Pattern**: `[ExtendObjectType("Mutation")]` with `[Authorize]` and `ClaimsPrincipal`
- **Dependencies**: Task 1.9.1, Feature 1.3
- **Testing**: Integration tests

##### Task 1.9.4: Update UserCards Mutations to Include CollectionId
- **File**: Modify `src/App.MtgDiscovery.GraphQL/Mutations/UserCardsMutationMethods.cs`
- **Changes**:
  - Add `collectionId` parameter to `AddCardToCollectionInput`
  - Default to user's default collection if not provided (backward compatibility)
  - Update response models
- **Dependencies**: Task 1.8.4
- **Testing**: Integration tests verifying collection-scoped operations

---

### Feature 1.10: Backend Service Registration

**Description**: Register new services in dependency injection container.

#### Tasks:

##### Task 1.10.1: Register Collection Services in DI
- **File**: Modify `src/App.MtgDiscovery.GraphQL/Startup.cs`
- **Services to Register**:
  - `ICollectionEntryService → CreateCollectionEntryService` (and Get variants)
  - `ICollectionDomainService → CreateCollectionDomainService` (and Get variants)
  - `ICollectionAggregatorService → CreateCollectionAggregatorService` (and Get variants)
  - `ICollectionCommandAdapter → CollectionCommandAdapter`
  - `ICollectionQueryAdapter → CollectionQueryAdapter`
  - `ICollectionAuthorizationService → CollectionAuthorizationService`
  - Cosmos operators: `CollectionGopher`, `CollectionScribe`, `CollectionsInquisitor`
- **Dependencies**: All previous features
- **Testing**: Startup tests verifying service resolution

---

### Feature 1.11: Backend Testing

**Description**: Comprehensive unit and integration tests for collection infrastructure.

#### Tasks:

##### Task 1.11.1: Create Collection Entry Service Tests
- **File**: Create `src/Lib.MtgDiscovery.Entry.Tests/Commands/Collections/CreateCollectionEntryServiceTests.cs`
- **Test Cases**:
  - Successful collection creation
  - Validation failures (invalid userId, invalid name, invalid type)
  - Domain service errors propagate correctly
- **Pattern**: MSTest with AwesomeAssertions, fake domain service
- **Dependencies**: Feature 1.3
- **Testing**: All tests pass

##### Task 1.11.2: Create Collection Domain Service Tests
- **File**: Create `src/Lib.Domain.Collections.Tests/Commands/CreateCollectionDomainServiceTests.cs`
- **Test Cases**:
  - Passthrough to aggregator
  - Error handling
- **Dependencies**: Feature 1.4
- **Testing**: All tests pass

##### Task 1.11.3: Create Collection Aggregator Service Tests
- **File**: Create `src/Lib.Aggregator.Collections.Tests/Commands/CreateCollectionAggregatorServiceTests.cs`
- **Test Cases**:
  - Successful collection creation
  - Adapter failures handled correctly
  - Entity mapping correctness
- **Dependencies**: Feature 1.5
- **Testing**: All tests pass

##### Task 1.11.4: Create Collection Adapter Service Tests
- **File**: Create `src/Lib.Adapter.Collections.Tests/Commands/CollectionCommandAdapterTests.cs`
- **Test Cases**:
  - Successful Cosmos operations
  - Cosmos failures return OperationException
  - Entity mapping correctness
- **Dependencies**: Feature 1.6
- **Testing**: All tests pass

##### Task 1.11.5: Create Collection Authorization Tests
- **File**: Create `src/Lib.Domain.Collections.Tests/Authorization/CollectionAuthorizationServiceTests.cs`
- **Test Cases**:
  - Owner can perform all operations
  - Editor can add/remove cards
  - Viewer can only read
  - Non-authorized user cannot access
  - Role hierarchy validation
- **Dependencies**: Feature 1.7
- **Testing**: All tests pass

##### Task 1.11.6: Create GraphQL Integration Tests
- **File**: Create `src/App.MtgDiscovery.GraphQL.Tests/Mutations/CollectionMutationMethodsTests.cs`
- **Test Cases**:
  - Create collection via GraphQL
  - Query user collections
  - Authorization enforcement
  - UserCards mutations with collectionId
- **Dependencies**: Feature 1.9
- **Testing**: All tests pass

---

### Feature 1.12: Collection Deletion

**Description**: Allow primary owners to delete non-default collections, removing the collection and all associated card data.

#### Tasks:

##### Task 1.12.1: Create Delete Collection Entities
- **Files to Create**:
  - `src/Lib.Shared.DataModels/Entities/Args/IDeleteCollectionArgEntity.cs` — Properties: `CollectionId`
  - `src/Lib.Shared.DataModels/Entities/Itrs/IDeleteCollectionItrEntity.cs` — Properties: `CollectionId`, `UserId`
- **Dependencies**: Task 1.1.2
- **Testing**: Interface contracts

##### Task 1.12.2: Create Delete Collection Validators
- **Files to Create**:
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/DeleteCollectionArgEntityValidatorContainer.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/DeleteCollectionArgEntityValidator_HasValidCollectionId.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/DeleteCollectionArgEntityValidator_AuthUserIsPrimaryOwner.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/DeleteCollectionArgEntityValidator_IsNotDefaultCollection.cs`
- **Validation Rules**:
  - CollectionId: Valid GUID
  - Authorization: Authenticated user is the primary owner (`owner_id`)
  - Cannot delete default collection (`is_default` is false)
- **Dependencies**: Task 1.12.1, Feature 1.7
- **Testing**: Unit tests for each validator

##### Task 1.12.3: Implement Delete Collection Service Layers
- **Files to Create**:
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/DeleteCollectionEntryService.cs`
  - `src/Lib.Domain.Collections/Commands/DeleteCollectionDomainService.cs`
  - `src/Lib.Aggregator.Collections/Commands/DeleteCollectionAggregatorService.cs`
- **Business Logic**:
  - Validate request (primary owner, not default)
  - Delete all UserCards documents with matching `collection_id`
  - Delete all UserWishlistCards documents with matching `collection_id`
  - Delete all UserSetCards documents with matching `collection_id`
  - Delete the Collection document
- **Dependencies**: Tasks 1.12.1, 1.12.2, Features 1.3-1.6
- **Testing**: Unit tests for each layer

##### Task 1.12.4: Add Delete to GraphQL Mutations
- **File**: Modify `src/App.MtgDiscovery.GraphQL/Mutations/CollectionMutationMethods.cs`
- **New Mutation**: `deleteCollection(collectionId: ID!): CollectionResponse!` (authenticated, primary owner only)
- **Dependencies**: Task 1.12.3
- **Testing**: Integration tests

##### Task 1.12.5: Create Delete Collection Tests
- **Files to Create**:
  - `src/Lib.MtgDiscovery.Entry.Tests/Commands/Collections/DeleteCollectionEntryServiceTests.cs`
  - `src/Lib.Domain.Collections.Tests/Commands/DeleteCollectionDomainServiceTests.cs`
  - `src/Lib.Aggregator.Collections.Tests/Commands/DeleteCollectionAggregatorServiceTests.cs`
- **Test Cases**:
  - Successful deletion removes Collection + all associated card records
  - Non-primary-owner cannot delete
  - Default collection cannot be deleted
  - Non-existent collection returns error
- **Dependencies**: Task 1.12.3
- **Testing**: All tests pass

---

### Epic 1 Completion Criteria

- [ ] All Cosmos containers created (Collections)
- [ ] All entity interfaces and implementations created
- [ ] All service layers implemented (Entry, Domain, Aggregator, Adapter)
- [ ] Authorization logic implemented and tested
- [ ] UserCards schema migrated to include `collection_id`
- [ ] GraphQL schema includes collection types, queries, and mutations
- [ ] Data migration script created and tested
- [ ] All unit tests pass (100+ tests expected)
- [ ] Integration tests pass
- [ ] Users can create collections via GraphQL
- [ ] Users' default collection is auto-created on registration
- [ ] UserCards mutations accept `collectionId` parameter
- [ ] Authorization validates collection access (respects visibility: private vs public)
- [ ] Visibility defaults to private for new and migrated collections
- [ ] Only primary owner can change collection visibility
- [ ] Primary owners can delete non-default collections
- [ ] Deletion removes Collection + all associated UserCards/UserWishlistCards/UserSetCards

---

## Epic 2: Collection Selection UI

**Goal**: Enable users to select which collection they are viewing/managing. Users can create additional collections from the UI. Collections have names for easy identification. "Default" collection is always named "Default Collection".

### Feature 2.1: Frontend Collection State Management

**Description**: Manage active collection state in React context.

#### Tasks:

##### Task 2.1.1: Create Collection Context
- **File**: Create `client/src/contexts/CollectionManagementContext.tsx`
- **State**:
  - `collections: Collection[]` - All accessible collections (owned + shared)
  - `activeCollection: Collection | null` - Currently selected collection
  - `defaultCollection: Collection | null` - User's default collection
  - `loading: boolean`
  - `error: Error | null`
- **Methods**:
  - `setActiveCollection(collectionId: string)`
  - `createCollection(name: string, type: CollectionType)`
  - `refreshCollections()`
- **Dependencies**: Epic 1 (GraphQL schema)
- **Testing**: Context tests with mock Apollo

##### Task 2.1.2: Update CollectionContext to Use Active Collection
- **File**: Modify `client/src/contexts/CollectionContext.tsx`
- **Changes**:
  - Import `CollectionManagementContext`
  - Use `activeCollection.id` instead of `userProfile.id` in mutation variables
  - Update optimistic cache updates to use `collectionId`
- **Dependencies**: Task 2.1.1
- **Testing**: Context tests verifying collection-scoped operations

##### Task 2.1.3: Update WishlistContext to Use Active Collection
- **File**: Modify `client/src/contexts/WishlistContext.tsx`
- **Changes**: Same as CollectionContext
- **Dependencies**: Task 2.1.1
- **Testing**: Context tests

---

### Feature 2.2: Collection GraphQL Queries (Frontend)

**Description**: Generate TypeScript types and hooks for collection operations.

#### Tasks:

##### Task 2.2.1: Define Collection GraphQL Queries
- **Files to Create**:
  - `client/src/graphql/queries/getMyCollections.graphql`
  - `client/src/graphql/queries/getAccessibleCollections.graphql`
  - `client/src/graphql/queries/getCollection.graphql`
- **Example Query**:
  ```graphql
  query GetMyCollections {
    myCollections {
      __typename
      ... on CollectionsSuccessResponse {
        data {
          id
          ownerId
          name
          type
          visibility
          isDefault
          authorizedUsers {
            userId
            role
            grantedAt
            grantedBy
          }
          createdAt
          updatedAt
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
  ```
- **Dependencies**: Epic 1 Feature 1.9
- **Testing**: Query validation via codegen

##### Task 2.2.2: Define Collection GraphQL Mutations
- **Files to Create**:
  - `client/src/graphql/mutations/createCollection.graphql`
- **Example Mutation**:
  ```graphql
  mutation CreateCollection($args: CreateCollectionInput!) {
    createCollection(args: $args) {
      __typename
      ... on CollectionSuccessResponse {
        data {
          id
          ownerId
          name
          type
          visibility
          isDefault
          createdAt
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
  ```
- **Dependencies**: Epic 1 Feature 1.9
- **Testing**: Mutation validation via codegen

##### Task 2.2.3: Generate TypeScript Types
- **Command**: `npm run codegen`
- **Files Generated**:
  - `client/src/generated/graphql.ts` (updated with collection types)
  - `client/src/generated/operations.ts` (updated with hooks)
- **Hooks Available**:
  - `useGetMyCollectionsQuery()`
  - `useGetAccessibleCollectionsQuery()`
  - `useGetCollectionQuery()`
  - `useCreateCollectionMutation()`
- **Dependencies**: Tasks 2.2.1, 2.2.2
- **Testing**: TypeScript compilation

---

### Feature 2.3: Collection Selector Component

**Description**: UI component for selecting active collection.

#### Tasks:

##### Task 2.3.1: Create CollectionSelector Atom
- **File**: Create `client/src/components/atoms/shared/CollectionSelector.tsx`
- **Component Type**: Material-UI Select dropdown
- **Props**:
  - `collections: Collection[]`
  - `activeCollectionId: string | null`
  - `onCollectionChange: (collectionId: string) => void`
- **UI Features**:
  - Display collection name + type badge
  - Highlight default collection
  - Show shared collections with different icon
  - Loading skeleton while fetching
- **Styling**: MUI `sx` props (no Tailwind)
- **Dependencies**: Task 2.1.1, Task 2.2.3
- **Testing**: Component tests with mocked collections

##### Task 2.3.2: Create CollectionBadge Atom
- **File**: Create `client/src/components/atoms/shared/CollectionBadge.tsx`
- **Component Type**: Material-UI Chip
- **Props**:
  - `type: CollectionType`
  - `size?: "small" | "medium"`
- **Badge Colors** (using theme):
  - `default`: Blue (`theme.palette.primary.main`)
  - `custom`: Green (`theme.palette.success.main`)
  - `cube`: Orange (`theme.palette.warning.main`)
  - `trade`: Teal (`theme.palette.info.main`)
- **Dependencies**: None
- **Testing**: Component tests

##### Task 2.3.3: Integrate CollectionSelector in Header
- **File**: Modify `client/src/components/organisms/Header.tsx`
- **Changes**:
  - Import `CollectionManagementContext`
  - Add `CollectionSelector` component to header (authenticated users only)
  - Position: Between navigation and user menu
- **Dependencies**: Tasks 2.3.1, 2.3.2
- **Testing**: Visual regression tests

---

### Feature 2.4: Create Collection Dialog

**Description**: Modal dialog for creating new collections.

#### Tasks:

##### Task 2.4.1: Create CreateCollectionDialog Organism
- **File**: Create `client/src/components/organisms/CreateCollectionDialog.tsx`
- **Component Type**: Material-UI Dialog
- **Props**:
  - `open: boolean`
  - `onClose: () => void`
  - `onCollectionCreated: (collection: Collection) => void`
- **Form Fields**:
  - Name: TextField (required, max 100 chars)
  - Type: Select dropdown (custom | cube | trade)
  - Visibility: Toggle switch (private | public), defaults to private
    - Helper text for private: "Only you and people you share with can see this collection"
    - Helper text for public: "Anyone can view this collection"
- **Validation**:
  - Name required
  - Name cannot be "default" (case-insensitive)
  - Type required
- **Submit**:
  - Call `useCreateCollectionMutation()`
  - Show loading state
  - Show error toast on failure
  - Close dialog on success
- **Dependencies**: Task 2.2.3
- **Testing**: Component tests with mock mutation

##### Task 2.4.2: Create "New Collection" Button
- **File**: Modify `client/src/components/atoms/shared/CollectionSelector.tsx`
- **Changes**:
  - Add "+ New Collection" button at bottom of dropdown menu
  - Opens `CreateCollectionDialog` on click
- **Dependencies**: Task 2.4.1
- **Testing**: Component tests

---

### Feature 2.5: Collections Management Page

**Description**: Dedicated page for viewing and managing all collections.

#### Tasks:

##### Task 2.5.1: Create CollectionsPage Component
- **File**: Create `client/src/pages/CollectionsPage.tsx`
- **Sections**:
  1. **My Collections**: Grid of collection cards (owned)
  2. **Shared With Me**: Grid of shared collection cards
- **Collection Card** (molecule):
  - Collection name
  - Type badge
  - Card count (if available)
  - Owner name (for shared collections)
  - "Select" button → Sets as active collection
  - "View Details" button → Navigate to collection details page
- **Dependencies**: Task 2.2.3, Feature 2.3
- **Testing**: Page tests with mock data

##### Task 2.5.2: Create CollectionCard Molecule
- **File**: Create `client/src/components/molecules/shared/CollectionCard.tsx`
- **Component Type**: Material-UI Card
- **Props**:
  - `collection: Collection`
  - `isActive: boolean`
  - `onSelect: () => void`
  - `onViewDetails: () => void`
- **UI Elements**:
  - Collection icon (based on type)
  - Name (Typography variant="h6")
  - Type badge
  - Visibility indicator (lock icon for private, globe icon for public)
  - Stats (e.g., "127 cards")
  - Action buttons (Select, View Details)
  - Active indicator (checkmark or highlight)
  - Visibility toggle (primary owner only): Switch to change between private/public
  - Delete button (primary owner only, non-default): Opens confirmation dialog, calls `deleteCollection` mutation
- **Styling**: Card elevation, hover effects
- **Dependencies**: Task 2.3.2
- **Testing**: Component tests

##### Task 2.5.3: Add CollectionsPage Route
- **File**: Modify `client/src/App.tsx`
- **Route**: `/collections`
- **Protected**: Requires authentication
- **Navigation**: Add link in user menu dropdown
- **Dependencies**: Task 2.5.1
- **Testing**: Routing tests

---

### Feature 2.6: Collection Persistence

**Description**: Persist active collection selection in localStorage.

#### Tasks:

##### Task 2.6.1: Implement Collection Persistence Logic
- **File**: Modify `client/src/contexts/CollectionManagementContext.tsx`
- **LocalStorage Key**: `activeCollectionId`
- **Logic**:
  - On mount: Read `activeCollectionId` from localStorage
  - If stored ID exists in user's collections, set as active
  - If stored ID not found, default to user's default collection
  - On `setActiveCollection`: Write to localStorage
- **Dependencies**: Task 2.1.1
- **Testing**: Context tests with localStorage mocks

---

### Feature 2.7: Update Card Operations

**Description**: Update all card operation flows to use active collection.

#### Tasks:

##### Task 2.7.1: Update AddCardToCollection Mutation Variables
- **File**: Modify `client/src/contexts/CollectionContext.tsx`
- **Changes**:
  - Read `activeCollection.id` from `CollectionManagementContext`
  - Pass `collectionId` in mutation variables instead of `userId`
- **Dependencies**: Task 2.1.2
- **Testing**: Context tests

##### Task 2.7.2: Update Wishlist Mutation Variables
- **File**: Modify `client/src/contexts/WishlistContext.tsx`
- **Changes**: Same as Task 2.7.1
- **Dependencies**: Task 2.1.3
- **Testing**: Context tests

##### Task 2.7.3: Update User Card Queries
- **Files to Modify**:
  - `client/src/graphql/queries/getUserCardsBySet.graphql`
  - `client/src/graphql/queries/getUserCardsByIds.graphql`
- **Changes**: Add `collectionId` filter parameter (optional, defaults to default collection)
- **Dependencies**: Epic 1 Feature 1.9
- **Testing**: Query tests

---

### Epic 2 Completion Criteria

- [ ] CollectionManagementContext implemented and tested
- [ ] CollectionSelector component in header
- [ ] CreateCollectionDialog functional
- [ ] CollectionsPage displays all collections
- [ ] Active collection persisted in localStorage
- [ ] All card operations use active collection
- [ ] Users can switch between collections seamlessly
- [ ] Collection type badges displayed consistently
- [ ] Default collection always available and clearly marked
- [ ] Visibility toggle available for collection owners
- [ ] Visibility indicator (lock/globe) displayed on collection cards

---

## Epic 3: Collection Sharing

**Goal**: Enable users to grant other users access to their collections. Grantees can remove themselves from access. Grantors can see who has access to which collections. User search by userId (no search UI initially).

### Feature 3.1: Backend Collection Sharing

**Description**: Backend logic for granting/revoking collection access.

#### Tasks:

##### Task 3.1.1: Create Collection Sharing Entities
- **Files to Create**:
  - `src/Lib.Shared.DataModels/Entities/Args/IGrantCollectionAccessArgEntity.cs`
  - `src/Lib.Shared.DataModels/Entities/Args/IRevokeCollectionAccessArgEntity.cs`
  - `src/Lib.Shared.DataModels/Entities/Itrs/IGrantCollectionAccessItrEntity.cs`
  - `src/Lib.Shared.DataModels/Entities/Itrs/IRevokeCollectionAccessItrEntity.cs`
- **Properties**:
  - `GrantCollectionAccess`: `CollectionId`, `TargetUserId`, `Role` (editor | viewer)
  - `RevokeCollectionAccess`: `CollectionId`, `TargetUserId`
- **Dependencies**: Epic 1 Feature 1.1
- **Testing**: Interface contracts

##### Task 3.1.2: Create Grant Access Validators
- **Files to Create**:
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/GrantCollectionAccessArgEntityValidatorContainer.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/GrantCollectionAccessArgEntityValidator_HasValidCollectionId.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/GrantCollectionAccessArgEntityValidator_HasValidTargetUserId.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/GrantCollectionAccessArgEntityValidator_HasValidRole.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/GrantCollectionAccessArgEntityValidator_AuthUserIsOwner.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/GrantCollectionAccessArgEntityValidator_TargetUserExists.cs`
- **Validation Rules**:
  - CollectionId: Valid GUID
  - TargetUserId: Valid GUID, user exists in system
  - Role: "editor" | "viewer" (cannot grant "owner")
  - Authorization: Authenticated user is collection owner
- **Dependencies**: Task 3.1.1, Epic 1 Feature 1.7
- **Testing**: Validator tests

##### Task 3.1.3: Implement Grant Access Service Layers
- **Files to Create**:
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/GrantCollectionAccessEntryService.cs`
  - `src/Lib.Domain.Collections/Commands/GrantCollectionAccessDomainService.cs`
  - `src/Lib.Aggregator.Collections/Commands/GrantCollectionAccessAggregatorService.cs`
- **Business Logic**:
  - Fetch collection document
  - Add new `AuthorizedUserExtEntity` to `authorized_users` array
  - If user already exists, update role
  - Set `granted_at` timestamp and `granted_by` to authenticated user
  - Update collection document
- **Dependencies**: Task 3.1.2, Epic 1 Features 1.3-1.6
- **Testing**: Unit tests for each layer

##### Task 3.1.4: Implement Revoke Access Service Layers
- **Files to Create**:
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/RevokeCollectionAccessEntryService.cs`
  - `src/Lib.Domain.Collections/Commands/RevokeCollectionAccessDomainService.cs`
  - `src/Lib.Aggregator.Collections/Commands/RevokeCollectionAccessAggregatorService.cs`
- **Business Logic**:
  - Fetch collection document
  - Remove `AuthorizedUserExtEntity` from `authorized_users` array
  - Cannot remove owner
  - Owner can remove anyone; users can remove themselves
  - Update collection document
- **Dependencies**: Task 3.1.1, Epic 1 Features 1.3-1.6
- **Testing**: Unit tests

##### Task 3.1.5: Create Revoke Access Validators
- **Files to Create**:
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/RevokeCollectionAccessArgEntityValidatorContainer.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/RevokeCollectionAccessArgEntityValidator_CannotRevokeOwner.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/RevokeCollectionAccessArgEntityValidator_AuthUserCanRevoke.cs` (owner OR self)
- **Dependencies**: Task 3.1.1, Epic 1 Feature 1.7
- **Testing**: Validator tests

---

### Feature 3.2: Backend Sharing Queries

**Description**: Query endpoints for collection access information.

#### Tasks:

##### Task 3.2.1: Create GetCollectionAccessList Query
- **Service Files**:
  - `src/Lib.MtgDiscovery.Entry/Queries/Collections/GetCollectionAccessListEntryService.cs`
  - `src/Lib.Domain.Collections/Queries/GetCollectionAccessListDomainService.cs`
  - `src/Lib.Aggregator.Collections/Queries/GetCollectionAccessListAggregatorService.cs`
- **Returns**: Array of `AuthorizedUserOutEntity` for a given collection
- **Authorization**: Only collection owner or users with access can view
- **Dependencies**: Epic 1 Features 1.3-1.6
- **Testing**: Unit tests

##### Task 3.2.2: Create GetSharedCollections Query
- **Service Files**:
  - `src/Lib.MtgDiscovery.Entry/Queries/Collections/GetSharedCollectionsEntryService.cs`
  - `src/Lib.Domain.Collections/Queries/GetSharedCollectionsDomainService.cs`
  - `src/Lib.Aggregator.Collections/Queries/GetSharedCollectionsAggregatorService.cs`
- **Returns**: Array of `CollectionOutEntity` where user is in `authorized_users` but not owner
- **Dependencies**: Epic 1 Feature 1.5
- **Testing**: Unit tests

---

### Feature 3.3: GraphQL Sharing Schema

**Description**: GraphQL mutations and queries for sharing operations.

#### Tasks:

##### Task 3.3.1: Create Sharing Mutation Methods
- **File**: Modify `src/App.MtgDiscovery.GraphQL/Mutations/CollectionMutationMethods.cs`
- **New Mutations**:
  - `grantCollectionAccess(args: GrantCollectionAccessInput!): CollectionResponse!`
  - `revokeCollectionAccess(args: RevokeCollectionAccessInput!): CollectionResponse!`
- **Input Types**:
  - `GrantCollectionAccessInput`: `collectionId`, `targetUserId`, `role`
  - `RevokeCollectionAccessInput`: `collectionId`, `targetUserId`
- **Authorization**: `[Authorize]` with `ClaimsPrincipal`
- **Dependencies**: Feature 3.1
- **Testing**: Integration tests

##### Task 3.3.2: Create Sharing Query Methods
- **File**: Modify `src/App.MtgDiscovery.GraphQL/Queries/CollectionQueryMethods.cs`
- **New Queries**:
  - `collectionAccessList(collectionId: ID!): AuthorizedUsersResponse!`
  - `sharedCollections: CollectionsResponse!`
- **Authorization**: `[Authorize]`
- **Dependencies**: Feature 3.2
- **Testing**: Integration tests

---

### Feature 3.4: Frontend Sharing Mutations

**Description**: Frontend GraphQL mutations for sharing.

#### Tasks:

##### Task 3.4.1: Define Sharing GraphQL Mutations
- **Files to Create**:
  - `client/src/graphql/mutations/grantCollectionAccess.graphql`
  - `client/src/graphql/mutations/revokeCollectionAccess.graphql`
- **Example**:
  ```graphql
  mutation GrantCollectionAccess($args: GrantCollectionAccessInput!) {
    grantCollectionAccess(args: $args) {
      __typename
      ... on CollectionSuccessResponse {
        data {
          id
          authorizedUsers {
            userId
            role
            grantedAt
            grantedBy
          }
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
  ```
- **Dependencies**: Feature 3.3
- **Testing**: Codegen validation

##### Task 3.4.2: Define Sharing GraphQL Queries
- **Files to Create**:
  - `client/src/graphql/queries/getCollectionAccessList.graphql`
  - `client/src/graphql/queries/getSharedCollections.graphql`
- **Dependencies**: Feature 3.3
- **Testing**: Codegen validation

##### Task 3.4.3: Generate TypeScript Types
- **Command**: `npm run codegen`
- **Hooks Generated**:
  - `useGrantCollectionAccessMutation()`
  - `useRevokeCollectionAccessMutation()`
  - `useGetCollectionAccessListQuery()`
  - `useGetSharedCollectionsQuery()`
- **Dependencies**: Tasks 3.4.1, 3.4.2
- **Testing**: TypeScript compilation

---

### Feature 3.5: Collection Sharing UI - Grant Access

**Description**: UI for granting collection access to other users.

#### Tasks:

##### Task 3.5.1: Create GrantAccessDialog Organism
- **File**: Create `client/src/components/organisms/GrantAccessDialog.tsx`
- **Component Type**: Material-UI Dialog
- **Props**:
  - `open: boolean`
  - `onClose: () => void`
  - `collectionId: string`
  - `collectionName: string`
- **Form Fields**:
  - **User ID**: TextField (required, GUID format)
    - Label: "User ID to grant access"
    - Helper text: "You'll need to know the exact User ID"
  - **Role**: Radio buttons (editor | viewer)
    - Editor: Can add/remove cards
    - Viewer: Can only view collection
- **Validation**:
  - User ID: Required, valid GUID format
  - Role: Required
- **Submit**:
  - Call `useGrantCollectionAccessMutation()`
  - Show loading state
  - Show error toast on failure (e.g., "User not found")
  - Show success toast on success
  - Close dialog
- **Dependencies**: Task 3.4.3
- **Testing**: Component tests with mock mutation

##### Task 3.5.2: Create "Share Collection" Button
- **File**: Modify `client/src/components/molecules/shared/CollectionCard.tsx`
- **Changes**:
  - Add "Share" icon button (only for collections owned by authenticated user)
  - Opens `GrantAccessDialog` on click
- **Dependencies**: Task 3.5.1
- **Testing**: Component tests

---

### Feature 3.6: Collection Access List UI

**Description**: Display who has access to a collection.

#### Tasks:

##### Task 3.6.1: Create AccessListDialog Organism
- **File**: Create `client/src/components/organisms/AccessListDialog.tsx`
- **Component Type**: Material-UI Dialog
- **Props**:
  - `open: boolean`
  - `onClose: () => void`
  - `collectionId: string`
  - `collectionName: string`
- **Data Fetching**: `useGetCollectionAccessListQuery()`
- **Content**:
  - List of authorized users
  - Each row shows:
    - User ID (truncated with copy button)
    - Role badge (owner | editor | viewer)
    - Granted at timestamp
    - Granted by (user ID)
    - "Remove" button (only for owner, cannot remove self)
- **Actions**:
  - Remove user: Call `useRevokeCollectionAccessMutation()`
  - Show confirmation dialog before removing
- **Dependencies**: Task 3.4.3
- **Testing**: Component tests

##### Task 3.6.2: Create "View Access" Button
- **File**: Modify `client/src/components/molecules/shared/CollectionCard.tsx`
- **Changes**:
  - Add "View Access" button (collections owned by authenticated user)
  - Opens `AccessListDialog` on click
- **Dependencies**: Task 3.6.1
- **Testing**: Component tests

---

### Feature 3.7: Shared Collections Display

**Description**: Display shared collections in Collections page.

#### Tasks:

##### Task 3.7.1: Update CollectionsPage with Shared Collections
- **File**: Modify `client/src/pages/CollectionsPage.tsx`
- **Changes**:
  - Query: `useGetSharedCollectionsQuery()`
  - Section: "Shared With Me"
  - Display: Grid of `CollectionCard` components
  - Each card shows:
    - Collection name
    - Owner's user ID
    - User's role (editor | viewer)
    - "Remove Myself" button (calls `revokeCollectionAccess` with own userId)
- **Dependencies**: Task 3.4.3
- **Testing**: Page tests

##### Task 3.7.2: Create SharedCollectionCard Molecule
- **File**: Create `client/src/components/molecules/shared/SharedCollectionCard.tsx`
- **Component Type**: Material-UI Card (extends `CollectionCard`)
- **Additional Props**:
  - `ownerUserId: string`
  - `userRole: "editor" | "viewer"`
  - `onRemoveSelf: () => void`
- **UI Differences**:
  - Shows "Shared by: {ownerUserId}"
  - Role badge displayed prominently
  - "Remove Myself" button instead of "Share"
  - No "View Access" button (only owner can view)
- **Dependencies**: Task 2.5.2
- **Testing**: Component tests

---

### Feature 3.8: Collection Sharing Notifications

**Description**: Toast notifications for sharing operations.

#### Tasks:

##### Task 3.8.1: Add Sharing Success Notifications
- **File**: Modify `client/src/components/organisms/GrantAccessDialog.tsx` and `AccessListDialog.tsx`
- **Notifications**:
  - Grant access success: "Access granted to user {userId} with {role} role"
  - Revoke access success: "Access revoked for user {userId}"
  - Remove self success: "You have been removed from {collectionName}"
- **Dependencies**: Tasks 3.5.1, 3.6.1, 3.7.1
- **Testing**: Component tests verify toast calls

##### Task 3.8.2: Add Sharing Error Notifications
- **Error Messages**:
  - User not found: "User with ID {userId} not found"
  - Already has access: "User already has access to this collection"
  - Cannot remove owner: "Cannot remove collection owner"
  - Unauthorized: "You don't have permission to share this collection"
- **Dependencies**: Tasks 3.5.1, 3.6.1
- **Testing**: Component tests with error scenarios

---

### Feature 3.9: User ID Discovery (Future Enhancement Placeholder)

**Description**: Placeholder for future user search functionality.

#### Tasks:

##### Task 3.9.1: Document User ID Discovery Limitation
- **File**: Update `.docs/COLLECTION_IDENTITY_ARCHITECTURE.md`
- **Section**: "Future Enhancements"
- **Content**:
  ```markdown
  ### User ID Discovery

  **Current Limitation**: Users must know the exact User ID to grant collection access.

  **Future Enhancement**: Implement user search functionality:
  - Search by nickname/display name
  - Search by email (if user has opted in)
  - Autocomplete dropdown in GrantAccessDialog
  - Recent/frequent collaborators list
  - QR code sharing for easy ID exchange

  **Backend Requirements**:
  - User search query (by nickname, email)
  - Privacy settings (allow/disallow discovery)
  - User profile service

  **Frontend Requirements**:
  - UserSearchAutocomplete component
  - User profile preview cards
  ```
- **Dependencies**: None
- **Testing**: Documentation review

##### Task 3.9.2: Add User ID Display in User Profile
- **File**: Create placeholder `client/src/pages/UserProfilePage.tsx`
- **Content**:
  - Display user's own User ID
  - "Copy to Clipboard" button
  - Helper text: "Share this ID with others to grant them collection access"
- **Dependencies**: Epic 1 (user authentication)
- **Testing**: Page tests

---

### Feature 3.10: Collection Ownership Transfer

**Description**: Allow primary owners to transfer ownership of non-default collections to authorized users.

#### Tasks:

##### Task 3.10.1: Create Transfer Ownership Entities
- **Files to Create**:
  - `src/Lib.Shared.DataModels/Entities/Args/ITransferCollectionOwnershipArgEntity.cs` — Properties: `CollectionId`, `TargetUserId`
  - `src/Lib.Shared.DataModels/Entities/Itrs/ITransferCollectionOwnershipItrEntity.cs` — Properties: `CollectionId`, `CurrentOwnerId`, `TargetUserId`
- **Dependencies**: Epic 1 Feature 1.1
- **Testing**: Interface contracts

##### Task 3.10.2: Create Transfer Ownership Validators
- **Files to Create**:
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/TransferCollectionOwnershipArgEntityValidatorContainer.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/TransferCollectionOwnershipArgEntityValidator_HasValidCollectionId.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/TransferCollectionOwnershipArgEntityValidator_HasValidTargetUserId.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/TransferCollectionOwnershipArgEntityValidator_AuthUserIsPrimaryOwner.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/TransferCollectionOwnershipArgEntityValidator_IsNotDefaultCollection.cs`
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/TransferCollectionOwnershipArgEntityValidator_TargetIsAuthorizedUser.cs`
- **Validation Rules**:
  - CollectionId: Valid GUID
  - TargetUserId: Valid GUID, user exists in `authorized_users` for this collection
  - Authorization: Authenticated user is the primary owner (`owner_id`)
  - Cannot transfer default collection
- **Dependencies**: Task 3.10.1, Epic 1 Feature 1.7
- **Testing**: Unit tests for each validator

##### Task 3.10.3: Implement Transfer Ownership Service Layers
- **Files to Create**:
  - `src/Lib.MtgDiscovery.Entry/Commands/Collections/TransferCollectionOwnershipEntryService.cs`
  - `src/Lib.Domain.Collections/Commands/TransferCollectionOwnershipDomainService.cs`
  - `src/Lib.Aggregator.Collections/Commands/TransferCollectionOwnershipAggregatorService.cs`
- **Business Logic**:
  - Fetch collection document
  - Change `owner_id` to target user
  - Ensure previous owner has "owner" role entry in `authorized_users` (co-owner)
  - Ensure target user's `authorized_users` entry is updated to "owner" role
  - Update `updated_at` timestamp
  - Update collection document
- **Dependencies**: Tasks 3.10.1, 3.10.2, Epic 1 Features 1.3-1.6
- **Testing**: Unit tests for each layer

##### Task 3.10.4: Add Transfer to GraphQL Mutations
- **File**: Modify `src/App.MtgDiscovery.GraphQL/Mutations/CollectionMutationMethods.cs`
- **New Mutation**: `transferCollectionOwnership(args: TransferCollectionOwnershipInput!): CollectionResponse!` (authenticated, primary owner only)
- **Dependencies**: Task 3.10.3
- **Testing**: Integration tests

##### Task 3.10.5: Create Transfer Ownership Tests
- **Files to Create**:
  - `src/Lib.MtgDiscovery.Entry.Tests/Commands/Collections/TransferCollectionOwnershipEntryServiceTests.cs`
  - `src/Lib.Domain.Collections.Tests/Commands/TransferCollectionOwnershipDomainServiceTests.cs`
  - `src/Lib.Aggregator.Collections.Tests/Commands/TransferCollectionOwnershipAggregatorServiceTests.cs`
- **Test Cases**:
  - Successful transfer changes `owner_id`
  - Previous owner retains "owner" role in `authorized_users`
  - Non-primary-owner cannot transfer
  - Default collection cannot be transferred
  - Target must be an existing authorized user
  - Target's `authorized_users` entry updated to "owner" role
- **Dependencies**: Task 3.10.3
- **Testing**: All tests pass

##### Task 3.10.6: Frontend Transfer UI
- **File**: Modify `client/src/components/molecules/shared/CollectionCard.tsx`
- **Changes**:
  - Add "Transfer Ownership" button (primary owner only, non-default collections)
  - Opens confirmation dialog with authorized user selection
  - Calls `transferCollectionOwnership` mutation
- **GraphQL Files to Create**:
  - `client/src/graphql/mutations/transferCollectionOwnership.graphql`
- **Dependencies**: Task 3.10.4
- **Testing**: Component tests

---

### Epic 3 Completion Criteria

- [ ] Users can grant collection access via GraphQL
- [ ] Users can revoke collection access via GraphQL
- [ ] GrantAccessDialog functional (requires User ID input)
- [ ] AccessListDialog displays all authorized users
- [ ] CollectionsPage displays shared collections
- [ ] Users can remove themselves from shared collections
- [ ] Sharing notifications displayed for all operations
- [ ] Owner can view and manage collection access list
- [ ] Authorization enforced (only owner can grant/revoke)
- [ ] Primary owner can transfer ownership of non-default collections
- [ ] Previous owner becomes co-owner after transfer
- [ ] Default collection cannot be transferred
- [ ] Transfer target must be an existing authorized user
- [ ] All sharing and transfer tests pass

---

## Cross-Epic Considerations

### Database Migration Strategy

1. **Epic 1 Migration** (Task 1.8.5):
   - Create Collections container
   - For each user: Create default collection
   - Update UserCards/UserWishlistCards/UserSetCards with `collection_id`
   - Run once, idempotent

2. **Rollback Plan**:
   - Keep `user_id` field in UserCards (don't remove)
   - If migration fails, revert to `user_id` queries
   - Collections container can be deleted if needed

3. **Data Consistency**:
   - Dual-write period: Write both `user_id` and `collection_id`
   - After migration verified, deprecate `user_id` field
   - Remove `user_id` in future major version

### Backward Compatibility

1. **GraphQL Mutations** (Epic 1):
   - `collectionId` parameter optional initially
   - If not provided, default to user's default collection
   - Deprecation notice in schema documentation

2. **Frontend Migration** (Epic 2):
   - CollectionManagementContext provides default collection as fallback
   - Existing components work without changes (use default collection)
   - New components use active collection

### Performance Considerations

1. **Cosmos DB Queries**:
   - Collections container: Partition by `owner_id` (efficient for "my collections" queries)
   - UserCards: Consider composite partition key `(user_id, collection_id)` for future optimization
   - GetAccessibleCollections: Cross-partition query (requires optimization if user has many shared collections)

2. **Frontend Caching**:
   - Apollo cache: Normalize collections by ID
   - Cache collection access list per collection
   - Invalidate on grant/revoke operations

3. **Optimistic UI**:
   - Collection selection: Update immediately, no server round-trip
   - Grant access: Optimistic add to access list
   - Revoke access: Optimistic remove from access list

### Security Considerations

1. **Authorization Enforcement**:
   - All collection mutations validate user's role
   - GraphQL resolvers check `ClaimsPrincipal`
   - Adapter layer validates collection access before Cosmos operations

2. **Visibility Enforcement**:
   - Private collections: All access (read and write) requires explicit `authorizedUsers` entry
   - Public collections: Read access is open to any authenticated user with the collection ID; write access still requires editor+ role in `authorizedUsers`
   - Only the primary owner (`owner_id`) can change visibility
   - Changing from public to private does not auto-revoke existing viewer grants

3. **Primary Owner vs Co-Owner Enforcement**:
   - Primary owner (`owner_id`): Can delete, transfer ownership, change visibility
   - Co-owner (role "owner" in `authorized_users`): Full CRUD + sharing, but cannot delete, transfer, or change visibility
   - Validators distinguish between `AuthUserIsPrimaryCollectionOwnerValidator` and `AuthUserIsCollectionOwnerValidator`

4. **User ID Privacy**:
   - User IDs are GUIDs (not sequential, harder to guess)
   - No public user search (prevents enumeration)
   - Future: Add privacy settings for discoverability

5. **Rate Limiting** (Future):
   - Limit collection sharing invitations per user per day
   - Prevent abuse of grant/revoke operations

### Testing Strategy

1. **Unit Tests** (All Epics):
   - Every service layer method tested
   - Validators tested with edge cases
   - Entity mapping correctness verified

2. **Integration Tests**:
   - GraphQL mutations end-to-end
   - Cosmos DB operations with test container
   - Authorization scenarios (owner, editor, viewer)

3. **Frontend Tests**:
   - Component tests with mock data
   - Context tests with fake Apollo
   - User flow tests (Cypress/Playwright)

4. **Migration Tests**:
   - Test migration script with sample data
   - Verify data consistency before/after
   - Test rollback procedure

---

## Success Metrics

### Epic 1 Metrics
- [ ] 100% unit test coverage for collection infrastructure
- [ ] Migration script successfully migrates all existing user data
- [ ] All existing UserCards operations continue to work
- [ ] New collections can be created via GraphQL
- [ ] Authorization correctly enforces collection access

### Epic 2 Metrics
- [ ] Users can select active collection in <2 clicks
- [ ] Active collection persists across sessions
- [ ] Collection selector visible and intuitive in header
- [ ] CreateCollectionDialog completes in <5 seconds
- [ ] All card operations use active collection

### Epic 3 Metrics
- [ ] Users can grant collection access in <10 seconds
- [ ] Access list displays all authorized users correctly
- [ ] Shared collections displayed separately from owned collections
- [ ] Revoke access (self-removal) works within <5 seconds
- [ ] Owner can view and manage all access permissions

---

## Implementation Timeline Estimate

### Epic 1: Core Infrastructure
- **Estimated Effort**: 60-80 hours
- **Breakdown**:
  - Feature 1.1-1.2: 10 hours (Cosmos schema, entities, default collection)
  - Feature 1.3-1.6: 25 hours (Service layers)
  - Feature 1.7: 8 hours (Authorization)
  - Feature 1.8: 12 hours (Schema migration, data migration script)
  - Feature 1.9: 10 hours (GraphQL schema)
  - Feature 1.10-1.11: 15 hours (DI registration, testing)

### Epic 2: Collection Selection UI
- **Estimated Effort**: 30-40 hours
- **Breakdown**:
  - Feature 2.1-2.2: 8 hours (Context, GraphQL queries)
  - Feature 2.3-2.4: 10 hours (Selector component, dialog)
  - Feature 2.5: 8 hours (Collections page)
  - Feature 2.6-2.7: 6 hours (Persistence, update operations)

### Epic 3: Collection Sharing
- **Estimated Effort**: 35-45 hours
- **Breakdown**:
  - Feature 3.1-3.2: 15 hours (Backend sharing logic)
  - Feature 3.3-3.4: 6 hours (GraphQL schema, frontend mutations)
  - Feature 3.5-3.6: 10 hours (Grant access UI, access list UI)
  - Feature 3.7-3.8: 8 hours (Shared collections display, notifications)
  - Feature 3.9: 2 hours (User ID discovery placeholder)

**Total Estimated Effort**: 125-165 hours

---

## Risk Assessment

### High Risks
1. **Data Migration Complexity**:
   - **Risk**: Migration script fails mid-process, leaving inconsistent data
   - **Mitigation**: Idempotent migration, transaction support, rollback plan, test thoroughly in staging

2. **Authorization Logic Bugs**:
   - **Risk**: Users can access collections they shouldn't
   - **Mitigation**: Comprehensive authorization tests, security review, penetration testing

3. **Breaking Changes**:
   - **Risk**: Existing frontend breaks after backend changes
   - **Mitigation**: Backward compatibility, feature flags, gradual rollout

### Medium Risks
1. **Performance Degradation**:
   - **Risk**: Cross-partition queries slow down collection access
   - **Mitigation**: Indexing strategy, caching, query optimization

2. **User Experience Confusion**:
   - **Risk**: Users don't understand collection switching
   - **Mitigation**: Onboarding flow, tooltips, clear UI labels

3. **Cosmos DB Costs**:
   - **Risk**: Additional container increases costs
   - **Mitigation**: Monitor RU consumption, optimize queries, set throughput limits

### Low Risks
1. **User ID Discovery Limitation**:
   - **Risk**: Users frustrated by needing to know exact User ID
   - **Mitigation**: Document limitation, provide user ID prominently in profile, plan for future enhancement

---

## Dependencies

### External Dependencies
- Azure Cosmos DB SDK (existing)
- HotChocolate GraphQL (existing)
- Material-UI v5 (existing)
- React 19 (existing)
- Apollo Client (existing)

### Internal Dependencies
- Existing user authentication system (Auth0)
- Existing user registration flow
- Existing UserCards/UserWishlistCards infrastructure

### Pre-requisites
- Development environment setup
- Test Cosmos DB account
- Frontend dev server running
- GraphQL schema generator (codegen)

---

## Next Steps

1. **Review and Approve Plan**: Stakeholders review this implementation plan
2. **Epic 1 Kickoff**: Begin with Feature 1.1 (Collection Entity Model)
3. **Iterative Development**: Complete each feature before moving to next
4. **Continuous Testing**: Write tests alongside implementation
5. **Code Review**: Peer review for each feature
6. **Staging Deployment**: Deploy Epic 1 to staging environment
7. **Migration Dry Run**: Test migration script with production data snapshot
8. **Production Deployment**: Deploy Epic 1 to production
9. **Epic 2 Kickoff**: Begin after Epic 1 is stable
10. **Epic 3 Kickoff**: Begin after Epic 2 is stable

---

## Open Questions (All Resolved)

1. ~~**Collection Limits**: Should there be a maximum number of collections per user?~~ **Resolved**: No enforced limit. Keeps the code simple; can add a limit later if needed.
2. ~~**Collection Deletion**: Should users be able to delete collections? What happens to shared access?~~ **Resolved**: Yes. Hard delete of the Collection document + all associated UserCards, UserWishlistCards, and UserSetCards records. Default collection cannot be deleted. Only the primary owner (`owner_id`) can delete. Shared access is implicitly removed when the collection ceases to exist.
3. ~~**Collection Transfer**: Should ownership be transferable?~~ **Resolved**: Yes, for non-default collections. Primary owner (`owner_id`) can transfer to any existing authorized user. Previous owner becomes a co-owner (retains "owner" role in `authorized_users`). Default collection cannot be transferred.
4. ~~**Access Audit Log**: Should we track access history (who accessed when)?~~ **Resolved**: No. System doesn't warrant that level of tracking.
5. ~~**Collection Templates**: Should there be templates for common collection types (e.g., Cube with specific structure)?~~ **Resolved**: No.
6. ~~**Bulk Sharing**: Should owners be able to grant access to multiple users at once?~~ **Resolved**: No. Users must be found individually then granted permissions.
7. ~~**Access Expiration**: Should shared access have expiration dates?~~ **Resolved**: No.
8. ~~**Collection Privacy Levels**: Should collections be private/public/unlisted?~~ **Resolved**: Collections have a `visibility` field (`private` | `public`) stored as a string for future extensibility. Private collections are only visible to authorized users. Public collections are viewable by any authenticated user who has the collection ID. No browsing/listing of public collections. Unlisted was excluded since collections aren't currently listed.
9. ~~**Collection Search**: Should collections be searchable by name/type (future)?~~ **Resolved**: No. Public visibility means viewable-by-ID, not browsable/searchable.
10. ~~**Collection Statistics Dashboard**: Should there be analytics per collection?~~ **Resolved**: No additional statistics beyond what currently exists for a collection.

---

## Appendix A: Entity Mapping Reference

### Collection Entity Flow

```
GraphQL Input (CollectionArgEntity)
  - Name, Type, Visibility
  ↓ Entry Layer (validates, defaults visibility to "private" if not provided)
Internal Transfer (CollectionItrEntity)
  - CollectionId, OwnerId, Name, Type, Visibility, IsDefault, AuthorizedUsers
  ↓ Domain Layer (business rules: unique names per user, one default)
Internal Transfer (CollectionItrEntity)
  ↓ Aggregator Layer
Transfer (CollectionXfrEntity)
  ↓ Adapter Layer
External (CollectionExtEntity) → Cosmos DB
  - Fields: id, owner_id, name, type, visibility, is_default, authorized_users[], created_at, updated_at
  ↓ Return Path
Out-Flow (CollectionOufEntity)
  ↓ Entry Layer
GraphQL Output (CollectionOutEntity)
```

### UserCard Entity Flow with CollectionId

```
GraphQL Input (AddUserCardArgEntity)
  - CardId, SetId, UserId, CollectionId, UserCardDetails
  ↓ Entry Layer (validates, enriches with card metadata)
Internal Transfer (UserCardItrEntity)
  - All above + CardName, SetName, Artist, ArtistIds, CardNameGuid
  ↓ Domain Layer (passthrough)
Internal Transfer (UserCardItrEntity)
  ↓ Aggregator Layer (dual-write coordination)
Transfer (AddUserCardXfrEntity)
  ↓ Adapter Layer
External (UserCardExtEntity) → Cosmos UserCards container
  - Partition Key: user_id
  - Document ID: card_id
  - Fields: user_id, card_id, collection_id, set_id, collected[]
  ↓ Aggregator also writes to
External (UserSetCardExtEntity) → Cosmos UserSetCards container
  - Partition Key: user_id
  - Document ID: set_id
  - Fields: user_id, set_id, collection_id, total_cards, groups{}
  ↓ Return Path
Out-Flow (UserCardOufEntity)
  ↓ Entry Layer
GraphQL Output (CardItemOutEntity with UserCardOutEntity)
```

---

## Appendix B: GraphQL Schema Examples

### Collection Type
```graphql
type Collection {
  id: ID!
  ownerId: ID!
  name: String!
  type: CollectionType!
  visibility: CollectionVisibility!
  isDefault: Boolean!
  authorizedUsers: [AuthorizedUser!]!
  createdAt: DateTime!
  updatedAt: DateTime!
}

enum CollectionType {
  DEFAULT
  CUSTOM
  CUBE
  TRADE
}

enum CollectionVisibility {
  PRIVATE
  PUBLIC
}

type AuthorizedUser {
  userId: ID!
  role: CollectionRole!
  grantedAt: DateTime!
  grantedBy: ID!
}

enum CollectionRole {
  OWNER
  EDITOR
  VIEWER
}
```

### Queries
```graphql
extend type Query {
  getCollection(collectionId: ID!): CollectionResponse!
  myCollections: CollectionsResponse!
  accessibleCollections: CollectionsResponse!
  sharedCollections: CollectionsResponse!
  collectionAccessList(collectionId: ID!): AuthorizedUsersResponse!
}
```

### Mutations
```graphql
extend type Mutation {
  createCollection(args: CreateCollectionInput!): CollectionResponse!
  deleteCollection(collectionId: ID!): CollectionResponse!
  updateCollectionVisibility(args: UpdateCollectionVisibilityInput!): CollectionResponse!
  transferCollectionOwnership(args: TransferCollectionOwnershipInput!): CollectionResponse!
  grantCollectionAccess(args: GrantCollectionAccessInput!): CollectionResponse!
  revokeCollectionAccess(args: RevokeCollectionAccessInput!): CollectionResponse!

  # Updated mutations
  addCardToCollection(args: AddCardToCollectionInput!): AddCardToCollectionResponse!
  addCardToWishlist(args: AddCardToWishlistInput!): AddCardToWishlistResponse!
}

input UpdateCollectionVisibilityInput {
  collectionId: ID!
  visibility: CollectionVisibility!
}

input TransferCollectionOwnershipInput {
  collectionId: ID!
  targetUserId: ID!
}

input CreateCollectionInput {
  name: String!
  type: CollectionType!
  visibility: CollectionVisibility = PRIVATE
}

input GrantCollectionAccessInput {
  collectionId: ID!
  targetUserId: ID!
  role: CollectionRole!
}

input RevokeCollectionAccessInput {
  collectionId: ID!
  targetUserId: ID!
}

# Updated input
input AddCardToCollectionInput {
  cardId: ID!
  setId: ID!
  userId: ID! # Deprecated, use collectionId
  collectionId: ID # Optional, defaults to user's default collection
  userCardDetails: UserCardDetailsInput!
}
```

### Response Types
```graphql
union CollectionResponse = CollectionSuccessResponse | FailureResponse
union CollectionsResponse = CollectionsSuccessResponse | FailureResponse
union AuthorizedUsersResponse = AuthorizedUsersSuccessResponse | FailureResponse

type CollectionSuccessResponse {
  data: Collection!
}

type CollectionsSuccessResponse {
  data: [Collection!]!
}

type AuthorizedUsersSuccessResponse {
  data: [AuthorizedUser!]!
}
```

---

## Appendix C: Cosmos DB Container Specifications

### Collections Container
```json
{
  "id": "Collections",
  "partitionKey": {
    "paths": ["/owner_id"],
    "kind": "Hash"
  },
  "indexingPolicy": {
    "indexingMode": "consistent",
    "automatic": true,
    "includedPaths": [
      { "path": "/*" }
    ],
    "excludedPaths": [
      { "path": "/\"_etag\"/?" }
    ],
    "compositeIndexes": [
      [
        { "path": "/owner_id", "order": "ascending" },
        { "path": "/created_at", "order": "descending" }
      ]
    ]
  },
  "throughput": 400
}
```

### Updated UserCards Container (Schema)
```json
{
  "id": "unique-card-id",
  "user_id": "user-guid",
  "collection_id": "collection-guid",
  "card_id": "scryfall-card-id",
  "set_id": "scryfall-set-id",
  "card_name": "Black Lotus",
  "set_name": "Limited Edition Alpha",
  "set_code": "LEA",
  "released_at": "1993-08-05",
  "artist": "Christopher Rush",
  "artist_ids": ["artist-guid"],
  "card_name_guid": "card-name-guid",
  "collected": [
    {
      "finish": "nonfoil",
      "special": "none",
      "count": 1
    }
  ]
}
```

---

## Appendix D: Testing Checklist

### Epic 1 Tests
- [ ] CollectionExtEntity serialization/deserialization
- [ ] Collection entity interface implementations
- [ ] CollectionGopher reads collection correctly
- [ ] CollectionScribe creates/updates/deletes collections
- [ ] CollectionsInquisitor queries by owner
- [ ] CollectionsInquisitor queries by authorized user
- [ ] CreateCollectionEntryService validates input
- [ ] CreateCollectionDomainService passes through
- [ ] CreateCollectionAggregatorService calls adapter
- [ ] CollectionCommandAdapter maps entities correctly
- [ ] CollectionAuthorizationService validates roles
- [ ] CollectionAuthorizationService allows public collection view access for any authenticated user
- [ ] CollectionAuthorizationService denies private collection access to non-authorized users
- [ ] Only primary owner can change collection visibility
- [ ] User registration creates default collection (private visibility)
- [ ] Migration script creates collections
- [ ] Migration script updates UserCards with collection_id
- [ ] GraphQL createCollection mutation works
- [ ] GraphQL myCollections query returns collections
- [ ] GraphQL accessibleCollections query includes shared
- [ ] AddCardToCollection mutation accepts collectionId
- [ ] Authorization validators enforce collection access
- [ ] Delete collection removes Collection document
- [ ] Delete collection removes associated UserCards
- [ ] Delete collection removes associated UserWishlistCards
- [ ] Delete collection removes associated UserSetCards
- [ ] Non-primary-owner cannot delete collection
- [ ] Default collection cannot be deleted
- [ ] Primary owner vs co-owner distinction enforced

### Epic 2 Tests
- [ ] CollectionManagementContext loads collections on mount
- [ ] CollectionManagementContext sets active collection
- [ ] CollectionManagementContext persists to localStorage
- [ ] CollectionSelector renders all collections
- [ ] CollectionSelector highlights active collection
- [ ] CollectionBadge renders correct colors
- [ ] CreateCollectionDialog validates input
- [ ] CreateCollectionDialog defaults visibility to private
- [ ] CreateCollectionDialog calls mutation
- [ ] CollectionCard displays visibility indicator (lock/globe icon)
- [ ] CollectionsPage displays owned collections
- [ ] CollectionCard renders collection info
- [ ] CollectionCard "Select" button sets active collection
- [ ] CollectionContext uses active collection
- [ ] WishlistContext uses active collection
- [ ] Frontend queries include collectionId filter
- [ ] Delete button shown only for primary owner on non-default collections
- [ ] Delete confirmation dialog works correctly

### Epic 3 Tests
- [ ] GrantCollectionAccessEntryService validates input
- [ ] GrantCollectionAccessDomainService enforces owner check
- [ ] GrantCollectionAccessAggregatorService updates document
- [ ] RevokeCollectionAccessEntryService validates input
- [ ] RevokeCollectionAccessDomainService allows owner/self
- [ ] GetCollectionAccessListEntryService returns authorized users
- [ ] GetSharedCollectionsEntryService returns shared collections
- [ ] GraphQL grantCollectionAccess mutation works
- [ ] GraphQL revokeCollectionAccess mutation works
- [ ] GraphQL collectionAccessList query returns users
- [ ] GraphQL sharedCollections query returns collections
- [ ] GrantAccessDialog validates user ID
- [ ] GrantAccessDialog calls mutation
- [ ] AccessListDialog displays authorized users
- [ ] AccessListDialog allows owner to revoke
- [ ] SharedCollectionCard displays owner
- [ ] SharedCollectionCard "Remove Myself" button works
- [ ] Sharing notifications display correctly
- [ ] Authorization enforced for all sharing operations
- [ ] Transfer ownership changes `owner_id`
- [ ] Previous owner retains "owner" role in `authorized_users`
- [ ] Non-primary-owner cannot transfer
- [ ] Default collection cannot be transferred
- [ ] Transfer target must be existing authorized user
- [ ] Frontend transfer button shown for primary owner on non-default collections

---

## Appendix E: Key File Reference

### Backend Files (Epic 1)
- `src/Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/CollectionExtEntity.cs`
- `src/Lib.Adapter.Scryfall.Cosmos/Apis/Containers/CollectionsCosmosContainer.cs`
- `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Gophers/CollectionGopher.cs`
- `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Scribes/CollectionScribe.cs`
- `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitors/CollectionsInquisitor.cs`
- `src/Lib.MtgDiscovery.Entry/Commands/Collections/CreateCollectionEntryService.cs`
- `src/Lib.MtgDiscovery.Entry/Commands/Collections/DeleteCollectionEntryService.cs`
- `src/Lib.Domain.Collections/Commands/CreateCollectionDomainService.cs`
- `src/Lib.Domain.Collections/Commands/DeleteCollectionDomainService.cs`
- `src/Lib.Aggregator.Collections/Commands/CreateCollectionAggregatorService.cs`
- `src/Lib.Aggregator.Collections/Commands/DeleteCollectionAggregatorService.cs`
- `src/Lib.Adapter.Collections/Commands/CollectionCommandAdapter.cs`
- `src/Lib.Domain.Collections/Authorization/CollectionAuthorizationService.cs`
- `src/App.MtgDiscovery.GraphQL/Mutations/CollectionMutationMethods.cs`
- `src/App.MtgDiscovery.GraphQL/Queries/CollectionQueryMethods.cs`

### Frontend Files (Epic 2)
- `client/src/contexts/CollectionManagementContext.tsx`
- `client/src/components/atoms/shared/CollectionSelector.tsx`
- `client/src/components/atoms/shared/CollectionBadge.tsx`
- `client/src/components/organisms/CreateCollectionDialog.tsx`
- `client/src/components/molecules/shared/CollectionCard.tsx`
- `client/src/pages/CollectionsPage.tsx`
- `client/src/graphql/queries/getMyCollections.graphql`
- `client/src/graphql/mutations/createCollection.graphql`

### Frontend Files (Epic 3)
- `client/src/components/organisms/GrantAccessDialog.tsx`
- `client/src/components/organisms/AccessListDialog.tsx`
- `client/src/components/molecules/shared/SharedCollectionCard.tsx`
- `client/src/graphql/mutations/grantCollectionAccess.graphql`
- `client/src/graphql/mutations/revokeCollectionAccess.graphql`
- `client/src/graphql/mutations/transferCollectionOwnership.graphql`
- `client/src/graphql/queries/getCollectionAccessList.graphql`
- `client/src/graphql/queries/getSharedCollections.graphql`

### Backend Files (Epic 3 - Transfer)
- `src/Lib.MtgDiscovery.Entry/Commands/Collections/TransferCollectionOwnershipEntryService.cs`
- `src/Lib.Domain.Collections/Commands/TransferCollectionOwnershipDomainService.cs`
- `src/Lib.Aggregator.Collections/Commands/TransferCollectionOwnershipAggregatorService.cs`

---

**End of Implementation Plan**
