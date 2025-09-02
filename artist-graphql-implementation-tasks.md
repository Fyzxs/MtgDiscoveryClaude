# Artist GraphQL Implementation Tasks

## Overview
Implement Artist Search and Cards by Artist GraphQL endpoints following the existing Card Search and Cards by Name patterns.

## Important Guidelines

### Project File Guidelines
**When creating new .csproj files**: Only include `<Project Sdk>` and `<ItemGroup>` sections. Do NOT include `<PropertyGroup>` with TargetFramework, ImplicitUsings, Nullable, etc. - these are configured globally in `Directory.Build.props`.

### Architectural Guidelines  
**Aggregator layers must be independent**: Each aggregator (Lib.Aggregator.Cards, Lib.Aggregator.Artists, etc.) should only reference adapters and shared libraries. Never reference other aggregator projects - copy needed models/mappers instead.

## Task Dependencies Flow
```
1. Project Structure → 2. Shared Interfaces → 3. Cosmos Operators → 4. Aggregator Layer 
→ 5. Domain Layer → 6. Data Layer → 7. Entry Layer → 8. GraphQL Layer → 9. Testing
```

---

## ✅ Task 1: Create Project Structure - COMPLETED
**Dependencies:** None  
**Estimated Time:** 30 minutes

### Context
We need two new projects following the existing layered architecture pattern. These projects will handle artist-specific domain logic and data aggregation.

### Implementation Steps
1. Create `Lib.Domain.Artists` project
   ```bash
   dotnet new classlib -n Lib.Domain.Artists -o src/Lib.Domain.Artists
   dotnet sln src/MtgDiscoveryVibe.sln add src/Lib.Domain.Artists/Lib.Domain.Artists.csproj
   ```

2. Create `Lib.Aggregator.Artists` project
   ```bash
   dotnet new classlib -n Lib.Aggregator.Artists -o src/Lib.Aggregator.Artists
   dotnet sln src/MtgDiscoveryVibe.sln add src/Lib.Aggregator.Artists/Lib.Aggregator.Artists.csproj
   ```

3. **IMPORTANT**: Clean up csproj files to follow existing patterns
   - Remove the auto-generated `PropertyGroup` section (TargetFramework, ImplicitUsings, Nullable)
   - These properties are already configured in `Directory.Build.props`
   - Final csproj should only contain `<Project Sdk>` and `<ItemGroup>` with references
   - Example correct format:
     ```xml
     <Project Sdk="Microsoft.NET.Sdk">
       <ItemGroup>
         <ProjectReference Include="..\ProjectName\ProjectName.csproj" />
       </ItemGroup>
     </Project>
     ```

4. Add project references following the layered architecture:
   - `Lib.Domain.Artists` references:
     - `Lib.Shared.DataModels`
     - `Lib.Shared.Invocation`
     - `Lib.Universal`
   
   - `Lib.Aggregator.Artists` references:
     - `Lib.Adapter.Scryfall.Cosmos`
     - `Lib.Shared.DataModels`
     - `Lib.Shared.Invocation`
     - `Lib.Universal`

4. Create folder structure in each project:
   - `Apis/` (public interfaces)
   - `Queries/` (query implementations)
   - `Operations/` (exceptions)
   - `Entities/` (Aggregator project only)

### Reference Pattern
Follow the structure of `Lib.Domain.Cards` and `Lib.Aggregator.Cards`

---

## ✅ Task 2: Create Shared Interfaces - COMPLETED
**Dependencies:** Task 1  
**Estimated Time:** 45 minutes

### Context
Define the data transfer interfaces that will be used across all layers for artist operations.

### Files to Create
In `Lib.Shared.DataModels/Entities/`:

1. **`IArtistSearchTermItrEntity.cs`**
   ```csharp
   public interface IArtistSearchTermItrEntity
   {
       string SearchTerm { get; }
   }
   ```
   Reference: `ICardSearchTermItrEntity.cs`

2. **`IArtistIdItrEntity.cs`**
   ```csharp
   public interface IArtistIdItrEntity
   {
       string ArtistId { get; }
   }
   ```
   Reference: `ICardNameItrEntity.cs`

3. **`IArtistSearchResultItrEntity.cs`**
   ```csharp
   public interface IArtistSearchResultItrEntity
   {
       string ArtistId { get; }
       string Name { get; }
   }
   ```
   Reference: `ICardNameSearchResultItrEntity.cs`

4. **`IArtistSearchResultCollectionItrEntity.cs`**
   ```csharp
   public interface IArtistSearchResultCollectionItrEntity
   {
       IEnumerable<IArtistSearchResultItrEntity> Artists { get; }
   }
   ```
   Reference: `ICardNameSearchResultCollectionItrEntity.cs`

5. **`IArtistItemItrEntity.cs`**
   ```csharp
   public interface IArtistItemItrEntity
   {
       dynamic Data { get; }
   }
   ```
   Reference: `ICardItemItrEntity.cs`

6. **`IArtistItemCollectionItrEntity.cs`**
   ```csharp
   public interface IArtistItemCollectionItrEntity
   {
       IEnumerable<IArtistItemItrEntity> Data { get; }
   }
   ```
   Reference: `ICardItemCollectionItrEntity.cs`

### Implementation Notes
- All interfaces should be in the `Lib.Shared.DataModels.Entities` namespace
- Follow the exact property naming conventions from the reference files
- These are pure data transfer interfaces with no behavior

---

## ✅ Task 3: Implement Cosmos Operators - COMPLETED
**Dependencies:** Task 2  
**Estimated Time:** 1.5 hours

### Context
Create the Cosmos DB query operators for artist data. These will query the existing collections created during bulk ingestion.

### Files to Create
In `Lib.Adapter.Scryfall.Cosmos/Apis/Operators/`:

1. **`ArtistNameTrigramsInquisitor.cs`** ✅ COMPLETED
   - Query the `ArtistNameTrigram` collection for artist search
   - Container: `ArtistNameTrigramsCosmosContainer`
   - Follows existing pattern from `CardNameTrigramsInquisitor.cs`

2. **`ScryfallArtistItemsGopher.cs`** ✅ COMPLETED
   - Query the `ScryfallArtistItem` collection by artist ID  
   - Container: `ArtistItemsCosmosContainer`
   - Follows existing pattern from `ScryfallCardItemsGopher.cs`

3. **`ScryfallArtistCardsInquisitor.cs`** ✅ COMPLETED
   - Query the `ScryfallArtistCard` collection for cards by artist
   - Container: `ArtistCardsCosmosContainer`
   - Follows existing inquisitor patterns for querying by partition

### Implementation Pattern
```csharp
internal sealed class ArtistNameTrigramQuerier : IArtistNameTrigramQuerier
{
    private readonly ICosmosContainerAdapter _container;
    
    public ArtistNameTrigramQuerier(ICosmosConfiguration config, ILogger logger)
    {
        _container = new CosmosContainerAdapter(
            new ConfigArtistNameTrigramsContainerSpecification(config), 
            logger);
    }
    
    // Query methods following existing patterns
}
```

### Reference Patterns
- Look at `QueryCardAggregatorService.CardNameSearchAsync` for trigram query logic
- Use `QueryDefinition` with parameterized queries
- Return `FeedIterator<T>` results as lists

---

## ✅ Task 4: Implement Aggregator Layer - COMPLETED
**Dependencies:** Task 3  
**Estimated Time:** 2 hours

### Context
The aggregator layer coordinates data retrieval from Cosmos and transforms it into domain entities.

### Files Created ✅

In `Lib.Aggregator.Artists/`:

1. **`Apis/IArtistAggregatorService.cs`** ✅ COMPLETED
   - Public interface with ArtistSearchAsync and CardsByArtistAsync methods

2. **`Apis/ArtistAggregatorService.cs`** ✅ COMPLETED  
   - Public service implementation delegating to QueryArtistAggregatorService
   - Constructor injection pattern with logger

3. **`Queries/QueryArtistAggregatorService.cs`** ✅ COMPLETED
   - Core implementation with trigram-based artist search algorithm
   - Cards by artist functionality using artist ID partition queries
   - Complete trigram scoring and ranking logic

4. **`Operations/ArtistAggregatorOperationException.cs`** ✅ COMPLETED
   - Custom exception extending OperationException

5. **`Entities/ArtistSearchResultItrEntity.cs`** ✅ COMPLETED
   - Implements IArtistSearchResultItrEntity with ArtistId and Name

6. **`Entities/ArtistSearchResultCollectionItrEntity.cs`** ✅ COMPLETED
   - Implements IArtistSearchResultCollectionItrEntity with Artists collection

7. **`Models/CardItemItrEntity.cs`** ✅ COMPLETED
   - Local copy of card item entity for cross-project compatibility
   - Full implementation of ICardItemItrEntity interface

8. **`Models/CardItemCollectionItrEntity.cs`** ✅ COMPLETED
   - Local copy of card collection entity

9. **`Queries/Mappers/ScryfallCardItemToCardItemItrEntityMapper.cs`** ✅ COMPLETED
   - Local mapper for converting Scryfall entities to domain entities

### Key Implementation Details
- **ARCHITECTURAL PRINCIPLE**: Aggregator layers must be independent - no cross-aggregator references
- Use constructor injection for Cosmos operators  
- Follow the trigram search algorithm exactly from card search
- Minimum 3 letters for search term
- Score results based on trigram match count
- Return results ordered by relevance
- Self-contained models and mappers (no dependency on Lib.Aggregator.Cards)

---

## ✅ Task 5: Implement Domain Layer - COMPLETED
**Dependencies:** Task 4  
**Estimated Time:** 1 hour

### Context
The domain layer is primarily a pass-through for artist queries but maintains the architectural pattern.

### Files Created ✅

In `Lib.Domain.Artists/`:

1. **`Apis/IArtistDomainService.cs`** ✅ COMPLETED
   - Public interface with ArtistSearchAsync and CardsByArtistAsync methods
   - Matches aggregator service interface exactly

2. **`Queries/QueryArtistDomainService.cs`** ✅ COMPLETED
   - Pass-through implementation delegating to ArtistAggregatorService
   - Constructor injection with logger following established pattern
   - All methods simply await and ConfigureAwait(false) on aggregator calls

3. **`Operations/ArtistDomainException.cs`** ✅ COMPLETED
   - Custom exception extending OperationException
   - HttpStatusCode.InternalServerError default
   - Follows exact pattern from other aggregator exceptions

### Implementation Notes
- This layer is intentionally thin for queries
- Maintains architectural consistency
- Future business logic would go here

---

## ✅ Task 6: Implement Data Layer - COMPLETED
**Dependencies:** Task 5  
**Estimated Time:** 45 minutes

### Context
The data layer coordinates between domain services, following the established pattern.

### Files to Modify/Create

1. **Modify `Lib.MtgDiscovery.Data/Apis/IDataService.cs`**
   - Add artist methods to interface

2. **Create `Lib.MtgDiscovery.Data/Queries/ArtistDataService.cs`**
   - Implement `IArtistDataService` (internal interface)
   - Delegate to `IArtistDomainService`
   - Follow pattern from `CardDataService.cs`

### Implementation Pattern
```csharp
internal sealed class ArtistDataService : IArtistDataService
{
    private readonly IArtistDomainService _artistDomainService;
    
    public ArtistDataService(IArtistDomainService artistDomainService)
    {
        _artistDomainService = artistDomainService;
    }
    
    // Pass-through methods
}
```

---

## ✅ Task 7: Implement Entry Layer - COMPLETED
**Dependencies:** Task 6  
**Estimated Time:** 2 hours

### Context
The entry layer handles validation and mapping of GraphQL arguments to internal entities.

### Files to Create

In `Lib.MtgDiscovery.Entry/`:

1. **Argument Entities**
   - `Entities/ArtistSearchTermItrEntity.cs`
   - `Entities/ArtistIdItrEntity.cs`

2. **Validators**
   - `Queries/Validators/ArtistSearchTermArgEntityValidator.cs`
     - Validate minimum 3 characters
     - Validate not null/empty
   - `Queries/Validators/ArtistIdArgEntityValidator.cs`
     - Validate artist ID format
     - Validate not null/empty
   - Create validator containers for each

3. **Mappers**
   - `Queries/Mappers/ArtistSearchTermArgsToItrMapper.cs`
   - `Queries/Mappers/ArtistIdArgsToItrMapper.cs`

4. **Entry Service**
   - `Queries/ArtistEntryService.cs`
   - Implement validation → mapping → data service call pattern
   - Follow `CardEntryService.cs` pattern exactly

5. **Modify `Apis/IEntryService.cs`**
   - Add artist methods

### Validation Rules
- Search term: minimum 3 characters, not null/empty
- Artist ID: valid GUID format, not null/empty

---

## ✅ Task 8: Implement GraphQL Layer - COMPLETED
**Dependencies:** Task 7  
**Estimated Time:** 2 hours

### Context
Create the GraphQL endpoints and response types for artist operations.

### Files to Create

In `App.MtgDiscovery.GraphQL/`:

1. **Argument Entities**
   - `Entities/Args/ArtistSearchTermArgEntity.cs`
   - `Entities/Args/ArtistIdArgEntity.cs`

2. **Output Entities**
   - `Entities/Outs/Artists/ArtistSearchResultOutEntity.cs`
   - `Entities/Outs/Artists/ScryfallArtistOutEntity.cs`

3. **Response Types**
   - `Entities/Types/ResponseModels/ArtistSearchResponseModelUnionType.cs`
   - `Entities/Types/ResponseModels/ArtistResponseModelUnionType.cs`
   - Follow union type pattern for success/failure responses

4. **Query Methods**
   - `Queries/ArtistQueryMethods.cs`
   - Implement `ArtistSearch` and `CardsByArtist` methods
   - Use `[ExtendObjectType(typeof(ApiQuery))]`
   - Follow `CardQueryMethods.cs` pattern

5. **Schema Extensions**
   - `Schemas/ArtistSchemaExtensions.cs`
   - Register new types with HotChocolate

### GraphQL Query Examples
```graphql
query {
  artistSearch(searchTerm: { term: "rebecca" }) {
    ... on SuccessDataResponseModel {
      data {
        artistId
        name
      }
    }
    ... on FailureResponseModel {
      status {
        message
        statusCode
      }
    }
  }
}

query {
  cardsByArtist(artistId: { id: "artist-guid-here" }) {
    ... on SuccessDataResponseModel {
      data {
        id
        name
        # other card fields
      }
    }
  }
}
```

---

## ⏳ Task 9: Testing - PENDING
**Dependencies:** Tasks 1-8  
**Estimated Time:** 3 hours

### Context
Create comprehensive tests for the new functionality.

### Test Coverage Required

1. **Unit Tests**
   - Validators (null, empty, invalid format)
   - Mappers (correct transformation)
   - Exception handling

2. **Integration Tests**
   - Cosmos operators with test data
   - Aggregator search algorithm
   - End-to-end GraphQL queries

3. **Test Projects to Update**
   - Create `Lib.Domain.Artists.Tests` (remember to follow csproj pattern - no PropertyGroup)
   - Create `Lib.Aggregator.Artists.Tests` (remember to follow csproj pattern - no PropertyGroup)
   - Update existing GraphQL tests

### Testing Patterns
- Follow MSTest framework
- Use TypeWrapper for private constructors
- Create fakes, not mocks
- Test naming: `MethodName_Scenario_ExpectedBehavior`

---

## Verification Checklist

After all tasks complete:
- [ ] Build solution successfully
- [ ] Artist search returns relevant results
- [ ] Cards by artist returns all artist's cards
- [ ] GraphQL playground shows new queries
- [ ] Validation errors return proper messages
- [ ] All tests pass
- [ ] No compiler warnings
- [ ] Follows MicroObjects patterns

## Reference Files for Patterns

- Card Search: `QueryCardAggregatorService.CardNameSearchAsync`
- Cards by Name: `QueryCardAggregatorService.CardsByNameAsync`
- GraphQL: `CardQueryMethods.cs`
- Entry Service: `CardEntryService.cs`
- Validators: `CardSearchTermArgEntityValidator.cs`
- Mappers: `CardSearchTermArgsToItrMapper.cs`