---
task: m-implement-collector-user-registration
branch: feature/implement-collector-user-registration
status: in-progress
created: 2025-01-14
modules: [App.MtgDiscovery.GraphQL, Lib.Adapter.Scryfall.Cosmos, Lib.Universal]
---

# Implement Collector User Registration

## Problem/Goal
Create an authenticated GraphQL mutation "registerUserInfo" that processes JWT tokens to register new collector users. The system needs to extract user information from JWT, create a unique collector ID using GuidUtility, and store user data in a new CollectorInfo Cosmos collection. Registration should only occur for new users who don't already exist.

## Success Criteria
- [ ] Create new CollectorInfo Cosmos container definition and container
- [ ] Implement authenticated GraphQL mutation "registerUserInfo"
- [ ] JWT processing extracts sub, nickname from token
- [ ] GuidUtility transforms jwt.sub into unique collector_id (partition + id)
- [ ] User existence check prevents duplicate registrations
- [ ] CollectorInfo document structure matches requirements:
  - id, partition = collector_id
  - collector_id = Guid-ified jwt.sub  
  - display_name = jwt.nickname
  - source_id = jwt.sub
- [ ] Proper error handling for authentication failures
- [ ] Unit tests for user registration logic
- [ ] Integration tests for GraphQL mutation

## Context Manifest

### How Authentication Currently Works: JWT Bearer Token Processing in GraphQL

When a user makes an authenticated GraphQL request, the system follows a well-established pattern starting with Auth0 JWT validation. The GraphQL API is configured in `Startup.cs` with JWT Bearer authentication that validates tokens against Auth0's authority (`https://{Auth0:Domain}/`) using the configured audience. The `TokenValidationParameters` sets `NameClaimType = ClaimTypes.NameIdentifier` to map the JWT's "sub" claim to the standard .NET identity claim.

Once a JWT is validated, the `ClaimsPrincipal` object becomes available to GraphQL resolvers through dependency injection. This is demonstrated in `UserQueryMethods.cs` where authenticated methods like `AuthenticatedHello` and `GetCurrentUser` receive a `ClaimsPrincipal` parameter decorated with the `[Authorize]` attribute. These methods extract specific claims from the JWT:
- `ClaimTypes.NameIdentifier` (mapped from JWT "sub") - the unique user identifier from Auth0
- `ClaimTypes.Email` - user's email address
- `"nickname"` custom claim - user's display name
- `ClaimTypes.Name` - user's full name

The authentication flow is tested through `graphql-auth0-test.http` which shows that unauthenticated requests return authorization errors, while authenticated requests with valid JWT tokens in the `Authorization: Bearer` header successfully access protected operations.

### How Cosmos Container Creation Currently Works: MicroObjects Pattern for Data Storage

The codebase follows a strict MicroObjects pattern for Cosmos DB containers where every concept is explicitly represented through interfaces and classes. Container creation involves four key components working together:

First, a container name class inheriting from `CosmosContainerName` defines the logical name (like `ArtistItemsCosmosContainerName` with `AsSystemType() => "ArtistItems"`). Second, a container definition class implementing `ICosmosContainerDefinition` specifies the account, database, container name, and partition key path. For example, `ArtistItemsCosmosContainerDefinition` returns `MtgDiscoveryCosmosAccountName`, `MtgDiscoveryCosmosDatabaseName`, `ArtistItemsCosmosContainerName`, and `PartitionCosmosPartitionKeyPath` (which maps to "/partition").

Third, the container adapter class extends `CosmosContainerAdapter` and takes a logger, container definition, and connection config in its constructor. Finally, operations against the container use specialized classes like "Scribe" (for writes), "Gopher" (for reads), and "Inquisitor" (for queries) that take the container adapter as a dependency.

All containers in this system use "/partition" as the partition key path, meaning documents must have a "partition" field that serves as both the logical partition and often duplicates the document's "id" field for single-document partitions.

### How GuidUtility Works: Deterministic GUID Generation from Strings

The `GuidUtility` class provides RFC 4122-compliant deterministic GUID generation using namespace-based UUIDs. The core method `Create(namespaceId, name, version)` takes a namespace GUID, a string name, and a version number (3 for MD5, 5 for SHA-1) to produce a consistent GUID for the same inputs.

The process converts the namespace GUID to network byte order, concatenates the namespace bytes with the UTF-8 encoded name bytes, computes the hash (MD5 or SHA-1), copies the first 16 bytes to form the new GUID, sets version bits in the time_hi_and_version field, sets the variant bits in clock_seq_hi_and_reserved, and converts back to local byte order.

This pattern is used throughout the system for creating deterministic identifiers, as seen in `CardNameGuidGenerator` which uses a fixed namespace GUID "4d746743-6172-644e-616d-654775696400" to generate consistent GUIDs for card names. For the collector registration system, JWT "sub" values will be processed through `GuidUtility.Create()` to generate deterministic collector IDs that remain consistent across multiple registration attempts.

### How GraphQL Mutations and Response Patterns Work: Union Types and Error Handling

The GraphQL API uses a sophisticated response pattern with union types to handle both success and failure scenarios. Every query method returns a `ResponseModel` base class and is decorated with `[GraphQLType(typeof(SomeResponseModelUnionType))]` to specify the union type containing both success and failure response variants.

The pattern involves checking `IOperationResponse<T>.IsFailure` from the entry service layer. On failure, methods return a `FailureResponseModel` with a `StatusDataModel` containing the error message and HTTP status code from `response.OuterException.StatusMessage` and `response.OuterException.StatusCode`. On success, they return a `SuccessDataResponseModel<T>` with the transformed data in the `Data` property.

GraphQL methods are organized in classes decorated with `[ExtendObjectType(typeof(ApiQuery))]` for queries, and mutations would follow the same pattern but extend a mutation root type. Methods take strongly-typed argument entities (like `CardNameArgEntity`) that map to GraphQL input types, call through to the `IEntryService` layer, and transform the results using mapper classes before returning the appropriate response model.

The error handling is consistent throughout: entry service responses contain `IOperationResponse<T>` objects that either succeed with typed data or fail with exception details, and GraphQL methods transform these into the appropriate union type responses for the client.

### For New Feature Implementation: Collector User Registration Integration Points

Since we're implementing collector user registration, it will integrate with existing authentication patterns but introduce new concepts to the system. The new GraphQL mutation will need to follow the established `[Authorize]` pattern to require JWT authentication, then extract the "sub" and "nickname" claims from the `ClaimsPrincipal` similar to existing user methods.

However, instead of just reading user data, this mutation needs to perform writes to Cosmos DB. This requires creating a new container following the MicroObjects pattern: `CollectorInfoCosmosContainerName`, `CollectorInfoCosmosContainerDefinition`, and `CollectorInfoCosmosContainer` classes. The container definition will use the same `MtgDiscoveryCosmosAccountName`, `MtgDiscoveryCosmosDatabaseName`, and `PartitionCosmosPartitionKeyPath` pattern as existing containers.

The business logic will need to use `GuidUtility.Create()` with a new namespace GUID to transform JWT "sub" values into deterministic collector IDs. This ID will serve as both the document ID and partition value, following the pattern where single documents use the same value for both fields.

The mutation will need to implement existence checking to prevent duplicate registrations. This requires creating a read operation first (probably through a "Gopher" class) to check if a collector already exists, then conditionally performing the write through a "Scribe" class only for new users.

The response will follow the union type pattern with success returning the created collector information and failure returning appropriate error messages for scenarios like "user already exists" or "invalid JWT data".

### Technical Reference Details

#### Authentication Components
- JWT validation: `Startup.cs:44-54` - Auth0 configuration
- Claims extraction: `UserQueryMethods.cs:17-18, 30-34` - ClaimsPrincipal usage
- Authorization attribute: `[Authorize]` decorator on methods
- Available claims: "sub" (ClaimTypes.NameIdentifier), "nickname", "email", "name"

#### Container Creation Pattern
```csharp
// Container Name
public sealed class CollectorInfoCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "CollectorInfo";
}

// Container Definition  
public sealed class CollectorInfoCosmosContainerDefinition : ICosmosContainerDefinition
{
    public CosmosFriendlyAccountName FriendlyAccountName() => new MtgDiscoveryCosmosAccountName();
    public CosmosDatabaseName DatabaseName() => new MtgDiscoveryCosmosDatabaseName();
    public CosmosContainerName ContainerName() => new CollectorInfoCosmosContainerName();
    public CosmosPartitionKeyPath PartitionKeyPath() => new PartitionCosmosPartitionKeyPath();
}

// Container Adapter
public sealed class CollectorInfoCosmosContainer : CosmosContainerAdapter
{
    public CollectorInfoCosmosContainer(ILogger logger)
        : base(logger, new CollectorInfoCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    {
    }
}
```

#### GuidUtility Usage Pattern
```csharp
private static readonly Guid s_collectorNamespace = new("some-namespace-guid-here");
Guid collectorGuid = GuidUtility.Create(s_collectorNamespace, jwtSubValue);
```

#### GraphQL Mutation Response Pattern
```csharp
[ExtendObjectType(typeof(ApiMutation))] // If mutations exist, otherwise extend ApiQuery
public class CollectorMutationMethods
{
    [Authorize]
    [GraphQLType(typeof(CollectorRegistrationResponseModelUnionType))]
    public async Task<ResponseModel> RegisterUserInfo(ClaimsPrincipal claimsPrincipal)
    {
        // Extract JWT claims
        // Call GuidUtility to generate collector ID
        // Check existence via entry service
        // Create new collector if not exists
        // Return appropriate response model
    }
}
```

#### Document Structure
```csharp
public class CollectorInfoDocument 
{
    [JsonProperty("id")]
    public string Id { get; set; } // collector_id

    [JsonProperty("partition")]  
    public string Partition { get; set; } // collector_id

    [JsonProperty("collector_id")]
    public string CollectorId { get; set; } // collector_id

    [JsonProperty("display_name")]
    public string DisplayName { get; set; } // jwt.nickname

    [JsonProperty("source_id")]
    public string SourceId { get; set; } // jwt.sub
}
```

#### Entry Service Integration
The mutation will need to call through the standard entry service pattern, which means:
1. Creating argument entities for the mutation input
2. Creating validators for the arguments  
3. Creating mappers to transform arguments to internal types
4. Implementing the business logic in domain/data layers
5. Following the `IOperationResponse<T>` pattern for error handling

#### File Locations
- Container definitions: `src/Lib.Adapter.Scryfall.Cosmos/Cosmos/Containers/`
- Container names: `src/Lib.Adapter.Scryfall.Cosmos/Cosmos/Values/`
- GraphQL mutations: `src/App.MtgDiscovery.GraphQL/Queries/` (or new Mutations folder)
- Entity models: `src/Lib.Adapter.Scryfall.Cosmos/Apis/Entities/`
- Response types: `src/App.MtgDiscovery.GraphQL/Entities/Types/ResponseModels/`
- Entry services: `src/Lib.MtgDiscovery.Entry/`
- Tests: corresponding `*.Tests` projects for each component

## Context Files
<!-- Added by context-gathering agent or manually -->

## User Notes
**CollectorInfo Document Structure:**
```json
{
  "id": "collector_id", 
  "partition": "collector_id",
  "collector_id": "Guid-ified jwt.sub",
  "display_name": "jwt.nickname", 
  "source_id": "jwt.sub"
}
```

- JWT.sub gets GuidUtility treatment to generate unique ID
- This ID becomes both partition and id value for the document
- Only register if user doesn't already exist
- Foundation for broader collector tracking system

## Detailed Task Breakdown

### **Task Status Legend:**
- 🔴 Not Started
- 🔄 In Progress  
- ⚠️ Awaiting Verification
- ✅ Complete

---

### **PHASE 1: Domain Objects Foundation** (Serial - Must Complete First)

#### Task 1.1: Create CollectorId Domain Object
- [ ] **Status:** 🔴 Not Started

**File:** `src/Lib.Shared.CollectorInfo/Values/CollectorId.cs`
**Implementation:** Copy-first approach - copy existing file, then modify minimally
**Multi-Reference Validation:** Check ALL these files before implementing:
- `src/Lib.Shared.Invocation/Primitives/PrincipalId.cs` (StringEqualityToSystemType pattern)  
- `src/Lib.Universal/Primitives/StringEqualityToSystemType.cs` (base class pattern)
- `src/Lib.Universal/Primitives/ToSystemType.cs` (core primitive pattern)
**Design Decision:** Wraps string GUID value, immutable, follows MicroObjects pattern
**Details:**
- Private readonly string field
- Constructor validates non-null/empty
- Implicit string conversion operator
- Equals/GetHashCode overrides
- Interface: `ICollectorId` in same namespace

#### Task 1.2: Create UserSubject Domain Object
- [ ] **Status:** 🔴 Not Started

**File:** `src/Lib.Shared.CollectorInfo/Values/UserSubject.cs`
**Reference:** `src/Lib.Shared.Cards/Values/CardName.cs` (string wrapper pattern)
**Design Decision:** Raw JWT.sub value, no transformation
**Details:** Same pattern as CollectorId but for raw subject strings

#### Task 1.3: Create UserNickname Domain Object
- [ ] **Status:** 🔴 Not Started

**File:** `src/Lib.Shared.CollectorInfo/Values/UserNickname.cs`
**Reference:** `src/Lib.Shared.Cards/Values/ArtistName.cs` (display name pattern)
**Details:** String wrapper for display purposes

#### Task 1.4: Create UserEmail Domain Object
- [ ] **Status:** 🔴 Not Started

**File:** `src/Lib.Shared.CollectorInfo/Values/UserEmail.cs`
**Reference:** Similar to UserNickname but add basic email format validation

#### Task 1.5: Create Lib.Shared.CollectorInfo Project
- [ ] **Status:** 🔴 Not Started

**Reference:** `src/Lib.Shared.Cards/Lib.Shared.Cards.csproj`
**Details:** New project to house collector domain objects, reference Universal

---

### **PHASE 2: Cosmos Infrastructure** (Can Be Parallel After Phase 1)

#### Task 2.1: Design CollectorInfo Document Structure
- [ ] **Status:** 🔴 Not Started

**Design Decision:** 
```json
{
  "id": "collector-guid",
  "partition": "collector-guid", 
  "collector_id": "collector-guid",
  "display_name": "User Nickname",
  "source_id": "jwt.sub"
}
```

#### Task 2.2: Create CollectorInfo Container Name
- [ ] **Status:** 🔴 Not Started

**File:** `src/Lib.Adapter.Scryfall.Cosmos/Cosmos/Values/CollectorInfoCosmosContainerName.cs`
**Reference:** `src/Lib.Adapter.Scryfall.Cosmos/Cosmos/Values/CardItemsCosmosContainerName.cs`
**Details:** Return "CollectorInfo" string

#### Task 2.3: Create CollectorInfo Container Definition
- [ ] **Status:** 🔴 Not Started

**File:** `src/Lib.Adapter.Scryfall.Cosmos/Cosmos/Containers/CollectorInfoCosmosContainerDefinition.cs`
**Reference:** `src/Lib.Adapter.Scryfall.Cosmos/Cosmos/Containers/CardItemsCosmosContainerDefinition.cs`
**Design Decision:** Uses /partition key path like other containers

#### Task 2.4: Create CollectorInfo Container Adapter
- [ ] **Status:** 🔴 Not Started

**File:** `src/Lib.Adapter.Scryfall.Cosmos/Cosmos/Containers/CollectorInfoCosmosContainer.cs`
**Reference:** `src/Lib.Adapter.Scryfall.Cosmos/Cosmos/Containers/CardItemsCosmosContainer.cs`
**Details:** Inherits from CosmosContainerAdapter

#### Task 2.5: Create CollectorInfo Item Entity
- [ ] **Status:** 🔴 Not Started

**File:** `src/Lib.Adapter.Scryfall.Cosmos/Apis/Entities/CollectorInfoItem.cs`
**Implementation:** Copy-first approach - copy existing entity, then modify minimally
**Multi-Reference Validation:** Check ALL these files before implementing:
- `src/Lib.Adapter.Scryfall.Cosmos/Apis/Entities/ScryfallArtistItem.cs` (CosmosItem pattern)
- `src/Lib.Adapter.Scryfall.Cosmos/Apis/Entities/ScryfallSetItem.cs` (JSON property pattern)
- `src/Lib.Cosmos/Apis/CosmosItem.cs` (base class)
**Design Decision:** DTO with properties matching Cosmos document structure
**Details:**
- Properties: Id, Partition, CollectorId, DisplayName, SourceId
- All string properties with init setters

#### Task 2.6: Create CollectorInfo Scribe
- [ ] **Status:** 🔴 Not Started

**File:** `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/CollectorInfoScribe.cs`  
**Implementation:** Copy-first approach - copy existing scribe, then modify minimally
**Multi-Reference Validation:** Check ALL these files before implementing:
- `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/ScryfallCardItemsScribe.cs` (basic scribe pattern)
- `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/ScryfallArtistItemsScribe.cs` (scribe pattern)
- `src/Lib.Cosmos/Apis/Operators/CosmosScribe.cs` (base class pattern)
**Details:** 
- UpsertCollectorAsync method
- ExistsCollectorAsync method (for duplicate prevention)
- Uses CollectorInfoCosmosContainer

---

### **PHASE 3: Authentication Infrastructure** (Parallel with Phase 2)

#### Task 3.1: Create IAuthenticatedUser Interface
- [ ] **Status:** 🔴 Not Started

**File:** `src/App.MtgDiscovery.GraphQL/Authentication/IAuthenticatedUser.cs`
**Design Decision:** Method signatures without "Get" prefix
**Details:**
```csharp
public interface IAuthenticatedUser
{
    CollectorId CollectorId();
    UserSubject Subject();  
    UserNickname Nickname();
    UserEmail Email();
}
```

#### Task 3.2: Create AuthenticatedUser Implementation
- [ ] **Status:** 🔴 Not Started

**File:** `src/App.MtgDiscovery.GraphQL/Authentication/AuthenticatedUser.cs`
**Implementation:** Copy-first approach - copy existing auth patterns, then modify minimally
**Multi-Reference Validation:** Check ALL these files before implementing:
- `src/App.MtgDiscovery.GraphQL/Queries/UserQueryMethods.cs` (claim extraction patterns)
- `src/Lib.Shared.Abstractions/Identifiers/CardNameGuidGenerator.cs` (GuidUtility usage pattern)
- `src/Lib.Universal/Utilities/GuidUtility.cs` (deterministic GUID creation)
**Design Decision:** Scoped service, uses IHttpContextAccessor
**Details:**
- Constructor takes IHttpContextAccessor, IGuidUtility
- Extracts claims from HttpContext.User
- Transforms jwt.sub using GuidUtility for CollectorId
- Caches extracted values (scoped lifetime)
- Throws if not authenticated

#### Task 3.3: Register Authentication Services
- [ ] **Status:** 🔴 Not Started

**File:** `src/App.MtgDiscovery.GraphQL/Startup.cs`
**Reference:** Lines 25-43 for DI registration patterns
**Details:** 
- AddHttpContextAccessor()
- AddScoped<IAuthenticatedUser, AuthenticatedUser>()

---

### **PHASE 4: Mappers** (Serial - After Phases 1-3)

#### Task 4.1: Create RegisterCollectorCommand Domain Object
- [ ] **Status:** 🔴 Not Started

**File:** `src/Lib.Domain.Collector/Commands/RegisterCollectorCommand.cs`
**Implementation:** Copy-first approach - copy existing ItrEntity, then modify minimally
**Multi-Reference Validation:** Check ALL these files before implementing:
- `src/Lib.Shared.DataModels/Entities/ISetIdsItrEntity.cs` (ItrEntity interface pattern)
- `src/Lib.Shared.DataModels/Entities/ICardIdsItrEntity.cs` (collection entity pattern)
- `src/Lib.Aggregator.Cards/QueryCardsIds.cs` (command implementation pattern)
**Details:** Immutable command with CollectorId, UserSubject, UserNickname properties

#### Task 4.2: Create Lib.Domain.Collector Project
- [ ] **Status:** 🔴 Not Started

**Reference:** `src/Lib.Domain.Cards` project structure
**Details:** New domain project for collector business logic

#### Task 4.3: Create AuthenticatedUserToRegisterCollectorCommandMapper
- [ ] **Status:** 🔴 Not Started

**File:** `src/Lib.MtgDiscovery.Entry/Mappers/AuthenticatedUserToRegisterCollectorCommandMapper.cs`
**Implementation:** Copy-first approach - copy existing mapper, then modify minimally
**Multi-Reference Validation:** Check ALL these files before implementing:
- `src/Lib.Aggregator.Cards/Queries/Mappers/QueryCardsIdsToReadPointItemsMapper.cs` (mapper pattern)
- `src/Lib.Aggregator.Sets/Queries/Mappers/QuerySetsIdsToReadPointItemsMapper.cs` (transformation pattern)
- `src/Lib.Shared.Abstractions/Identifiers/CardNameGuidGenerator.cs` (GuidUtility usage)
**Design Decision:** Handles GuidUtility transformation here
**Details:**
- MapToRegisterCollectorCommand(IAuthenticatedUser) method
- Uses injected IGuidUtility to transform Subject() to CollectorId

---

### **PHASE 5: Domain Services** (Serial - After Phase 4)

#### Task 5.1: Create CollectorRegistrationResult Domain Object
- [ ] **Status:** 🔴 Not Started

**File:** `src/Lib.Domain.Collector/Results/CollectorRegistrationResult.cs`
**Reference:** Result patterns in Domain projects
**Design Decision:** Success/failure result with CollectorId on success

#### Task 5.2: Create ICollectorDomainService Interface
- [ ] **Status:** 🔴 Not Started

**File:** `src/Lib.Domain.Collector/Services/ICollectorDomainService.cs`
**Implementation:** Copy-first approach - copy existing domain service, then modify minimally
**Multi-Reference Validation:** Check ALL these files before implementing:
- `src/Lib.Domain.Cards/Apis/ICardDomainService.cs` (domain service pattern)
- `src/Lib.Domain.Sets/Apis/ISetDomainService.cs` (async method signatures)
- `src/Lib.Domain.Artists/Apis/IArtistDomainService.cs` (operation response pattern)
**Details:** RegisterCollectorAsync(RegisterCollectorCommand) method

#### Task 5.3: Create CollectorDomainService Implementation
- [ ] **Status:** 🔴 Not Started

**File:** `src/Lib.Domain.Collector/Services/CollectorDomainService.cs`
**Reference:** Domain service patterns
**Details:** Business logic for collector registration, validation

---

### **PHASE 6: Data Layer** (Serial - After Phase 5)

#### Task 6.1: Create ICollectorDataService Interface
- [ ] **Status:** 🔴 Not Started

**File:** `src/Lib.MtgDiscovery.Data/Services/ICollectorDataService.cs`
**Details:** Data access interface for collector operations

#### Task 6.2: Create CollectorDataService Implementation
- [ ] **Status:** 🔴 Not Started

**File:** `src/Lib.MtgDiscovery.Data/Services/CollectorDataService.cs`
**Reference:** Data service patterns
**Details:** Uses CollectorInfoScribe for Cosmos operations

---

### **PHASE 7: Entry Services** (Serial - After Phase 6)

#### Task 7.1: Create ICollectorEntryService Interface
- [ ] **Status:** 🔴 Not Started

**File:** `src/Lib.MtgDiscovery.Entry/Services/ICollectorEntryService.cs`
**Details:** RegisterCollectorUserAsync(IAuthenticatedUser) method signature

#### Task 7.2: Create CollectorEntryService Implementation
- [ ] **Status:** 🔴 Not Started

**File:** `src/Lib.MtgDiscovery.Entry/Services/CollectorEntryService.cs`
**Reference:** Entry service patterns
**Design Decision:** Orchestrates mapper → domain → data flow
**Details:** Uses AuthenticatedUserToRegisterCollectorCommandMapper

---

### **PHASE 8: GraphQL Layer** (Serial - After Phase 7)

#### Task 8.1: Create RegisterUserInfoResponse Models
- [ ] **Status:** 🔴 Not Started

**File:** `src/App.MtgDiscovery.GraphQL/Responses/RegisterUserInfoResponse.cs`
**Reference:** `src/App.MtgDiscovery.GraphQL/Responses/` for union response patterns
**Design Decision:** Union type (Success/Failure) following established patterns

#### Task 8.2: Create CollectorMutationMethods
- [ ] **Status:** 🔴 Not Started

**File:** `src/App.MtgDiscovery.GraphQL/Mutations/CollectorMutationMethods.cs`
**Reference:** `src/App.MtgDiscovery.GraphQL/Queries/UserQueryMethods.cs` for [Authorize] patterns
**Details:**
- [ExtendObjectType(typeof(ApiMutation))] attribute
- RegisterUserInfo(IAuthenticatedUser) method
- Returns RegisterUserInfoResponse

#### Task 8.3: Register Mutation Extensions
- [ ] **Status:** 🔴 Not Started

**File:** `src/App.MtgDiscovery.GraphQL/Startup.cs`
**Reference:** Line 66 AddSetSchemaExtensions pattern
**Details:** Add .AddCollectorSchemaExtensions() to GraphQL server builder

#### Task 8.4: Create ApiMutation Base Class
- [ ] **Status:** 🔴 Not Started

**File:** `src/App.MtgDiscovery.GraphQL/Schemas/ApiMutation.cs`
**Reference:** `src/App.MtgDiscovery.GraphQL/Schemas/ApiQuery.cs`
**Details:** Base mutation class for extensions

---

### **PHASE 9: Error Handling & Validation** (Parallel with Phase 8)

#### Task 9.1: Authentication Error Scenarios
- [ ] **Status:** 🔴 Not Started

**Details:** Handle cases where JWT is invalid, missing claims, etc.
**Reference:** Existing error patterns in GraphQL layer

#### Task 9.2: Duplicate Registration Prevention
- [ ] **Status:** 🔴 Not Started

**Details:** Check CollectorInfoScribe.ExistsCollectorAsync before creating
**Design Decision:** Return success if user already exists (idempotent)

---

### **PHASE 10: Testing** (Parallel - After Implementation)

#### Task 10.1: Domain Object Tests
- [ ] **Status:** 🔴 Not Started

**Files:** Test files for each domain object
**Reference:** Existing domain object test patterns

#### Task 10.2: Service Tests
- [ ] **Status:** 🔴 Not Started

**Details:** Unit tests for Entry, Domain, Data services
**Reference:** Existing service test patterns

#### Task 10.3: GraphQL Integration Tests
- [ ] **Status:** 🔴 Not Started

**Details:** End-to-end mutation testing
**Reference:** Existing GraphQL test patterns

---

### **Dependencies Summary:**
- **Phase 1** → All other phases
- **Phases 2-3** → Parallel after Phase 1  
- **Phase 4** → After Phases 1-3
- **Phases 5-8** → Serial chain
- **Phases 9-10** → Parallel with later phases

**Estimated 18 discrete tasks across 10 phases with clear handoff points.**

## Work Log
<!-- Updated as work progresses -->
- [2025-01-14] Created task for collector user registration system
- [2025-01-14] Added comprehensive task breakdown with 18 discrete tasks across 10 phases