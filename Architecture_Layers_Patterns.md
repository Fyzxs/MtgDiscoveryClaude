# Architecture Layers Patterns

## Overview
This document comprehensively describes the patterns, conventions, and approaches used in each layer of the MtgDiscoveryVibe architecture. The architecture follows a strict layered approach with MicroObjects principles, ensuring clear separation of concerns and maintainable code structure.

## Data Flow Architecture

### Request Flow (Inbound)
```
App Layer → Entry Layer → Shared Layer → Domain Layer → Aggregator Layer → Adapter Layer
```

### Response Flow (Outbound)
```
Adapter Layer → Aggregator Layer → Domain Layer → Shared Layer → Entry Layer → App Layer
```

### Entity Transformation Flow
```
ArgEntity (App) → ItrEntity (Internal) → ExtEntity (External) → ItrEntity → OutEntity (App)
```

---

## Layer 1: App Layer (`App.MtgDiscovery.GraphQL`)

### Purpose
Primary entry point for GraphQL API requests, handling authentication, authorization, and request/response transformation.

### Key Design Patterns

#### GraphQL Extension Pattern
```csharp
[ExtendObjectType(typeof(ApiQuery))]
public sealed class CardQueryMethods
{
    public async Task<ResponseModel> GetCard([Service] ICardEntryService entryService, CardIdsArgEntity args)
    {
        // Implementation
    }
}
```

#### Union Type Response Pattern
```csharp
[UnionType("CardResponse")]
public abstract class CardResponseModelUnionType : ResponseModel
{
    protected CardResponseModelUnionType() : base(null) { }
}
```

#### Constructor Chain Pattern
```csharp
public CardQueryMethods(ILogger<CardQueryMethods> logger)
    : this(new CardEntryService(logger))
{ }

private CardQueryMethods(ICardEntryService cardEntryService)
{
    _cardEntryService = cardEntryService;
}
```

### Naming Conventions
- **Query Classes**: `*QueryMethods` (e.g., `CardQueryMethods`, `SetQueryMethods`)
- **Mutation Classes**: `*MutationMethods` (e.g., `UserMutationMethods`)
- **Argument Entities**: `*ArgEntity` (input from GraphQL)
- **Output Entities**: `*OutEntity` (output to GraphQL)
- **Type Classes**: `*OutEntityType` (GraphQL type definitions)
- **Response Models**: `*ResponseModel` with union types
- **Mappers**: `*TypeMapper` for entity transformation

### GraphQL Response Type Pattern (Critical)

Every GraphQL response type **MUST** follow this three-part pattern:

#### 1. Union Type Class
```csharp
// CORRECT: Uses dedicated type classes
public sealed class UserCardCollectionResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("UserCardCollectionResponseModel");
        descriptor.Description("Union type for user card collection response");
        descriptor.Type<UserCardCollectionSuccessDataResponseModelType>();  // ✓ Dedicated type
        descriptor.Type<FailureResponseModelType>();                        // ✓ Dedicated type
    }
}

// INCORRECT: Using inline ObjectType<T> - THIS WILL FAIL AT RUNTIME
public sealed class BadResponseModelUnionType : UnionType<ResponseModel>
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor
            .Name("BadResponseModel")
            .Type<ObjectType<SuccessDataResponseModel<SomeEntity>>>()  // ✗ Will not resolve
            .Type<ObjectType<FailureResponseModel>>();                 // ✗ Will not resolve
    }
}
```

#### 2. Success Response Type Class
```csharp
// Every success response MUST have a dedicated type class
public sealed class UserCardCollectionSuccessDataResponseModelType
    : ObjectType<SuccessDataResponseModel<UserCardCollectionOutEntity>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<UserCardCollectionOutEntity>> descriptor)
    {
        descriptor.Name("SuccessUserCardCollectionResponse");  // GraphQL type name
        descriptor.Description("Response returned when adding cards to collection is successful");

        descriptor.Field(f => f.Data)
            .Type<NonNullType<UserCardCollectionOutEntityType>>()  // Reference entity type
            .Description("The user card collection result");

        descriptor.Field(f => f.Status)
            .Type<StatusDataModelType>()
            .Description("Status information about the success");

        descriptor.Field(f => f.MetaData)
            .Type<MetaDataModelType>()
            .Description("Metadata about the response");
    }
}
```

#### 3. Entity Type Classes
```csharp
// Each entity in the response needs its own type class
public sealed class UserCardCollectionOutEntityType : ObjectType<UserCardCollectionOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<UserCardCollectionOutEntity>> descriptor)
    {
        descriptor.Name("UserCardCollection");
        descriptor.Description("A user's card collection entry");

        descriptor.Field(f => f.UserId)
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier for the user");

        // Define all fields...
    }
}
```

#### 4. Schema Registration
```csharp
// All types MUST be registered in schema extensions
public static IRequestExecutorBuilder AddApiMutation(this IRequestExecutorBuilder builder)
{
    return builder
        .AddTypeExtension<UserCardsMutationMethods>()
        .AddType<UserCardCollectionResponseModelUnionType>()         // Union type
        .AddType<UserCardCollectionSuccessDataResponseModelType>()   // Success type
        .AddType<UserCardCollectionOutEntityType>()                  // Entity type
        .AddType<CollectedItemOutEntityType>()                       // Nested entity types
        .AddType<FailureResponseModelType>();                        // Failure type (shared)
}
```

#### Validation Checklist
- [ ] Union type class exists and extends `UnionType` (not `UnionType<T>`)
- [ ] Success response type class exists as `{Entity}SuccessDataResponseModelType`
- [ ] Entity type class exists as `{Entity}OutEntityType`
- [ ] All nested entity types have their own type classes
- [ ] Union type references dedicated type classes (not `ObjectType<T>`)
- [ ] All types are registered in schema extensions
- [ ] GraphQL type names follow pattern: `Success{Entity}Response`

### Common Abstractions
```csharp
// Base response model for all GraphQL responses
public abstract class ResponseModel
{
    protected ResponseModel(object? value) { }
}

// Success response with data
public sealed class SuccessDataResponseModel<T> : ResponseModel
{
    public T Data { get; init; }
    public StatusDataModel Status { get; init; }
}

// Failure response with error information
public sealed class FailureResponseModel : ResponseModel
{
    public string Message { get; init; }
    public StatusDataModel Status { get; init; }
}
```

### MicroObjects Patterns Applied
- **Interface for Every Mapper**: `ICreateMapper<TSource, TDestination>`
- **Immutable DTOs**: All entities use `init` setters
- **No Logic in Constructors**: Only field assignment
- **Explicit Mapping**: Dedicated mapper classes instead of implicit conversions

---

## Layer 2: Entry Layer (`Lib.MtgDiscovery.Entry`)

### Purpose
Service entry point that validates incoming requests, transforms data between layers, and delegates to domain services.

### Key Design Patterns

#### Validator Pattern Implementation (Many Small Classes Pattern)

The Entry Layer uses a distinctive validation pattern that creates many small, focused validator classes. This is a deliberate architectural decision.

##### Pattern Structure
Each complete validation requires:
1. **Container Class**: Composes multiple validators in sequence
2. **Validator Interface**: `I{Type}ArgEntityValidator` extending `IValidatorAction`
3. **Individual Validators**: Each with 3 nested classes (Validator, Message, and main class)

##### Complete Implementation Example
```csharp
// 1. Interface Definition (1 class)
internal interface ICardIdsArgEntityValidator
    : IValidatorAction<ICardIdsArgEntity, IOperationResponse<ICardItemCollectionItrEntity>>;

// 2. Container Class (1 class) - Composes all validators
internal sealed class CardIdsArgEntityValidatorContainer
    : ValidatorActionContainer<ICardIdsArgEntity, IOperationResponse<ICardItemCollectionItrEntity>>,
      ICardIdsArgEntityValidator
{
    public CardIdsArgEntityValidatorContainer() : base([
        new IsNotNullCardIdsArgEntityValidator(),    // 3 classes
        new IdsNotNullCardIdsArgEntityValidator(),   // 3 classes
        new HasIdsCardIdsArgEntityValidator(),       // 3 classes
        new ValidCardIdsArgEntityValidator(),        // 3 classes
    ])
    { }
}

// 3. Individual Validator Example (3 classes per validator)
internal sealed class IsNotNullCardIdsArgEntityValidator
    : OperationResponseValidator<ICardIdsArgEntity, ICardItemCollectionItrEntity>
{
    public IsNotNullCardIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    // Nested class 1: Validation logic
    public sealed class Validator : IValidator<ICardIdsArgEntity>
    {
        public Task<bool> IsValid(ICardIdsArgEntity arg) =>
            Task.FromResult(arg is not null);
    }

    // Nested class 2: Typed error message
    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided object is null";
    }
}
```

##### Class Count Breakdown
For a complete validation of `CardIdsArgEntity`:
- 1 interface (`ICardIdsArgEntityValidator`)
- 1 container (`CardIdsArgEntityValidatorContainer`)
- 4 validators × 3 classes each = 12 classes
- **Total: 14 classes for one complete validation**

##### Why This Pattern? (Design Rationale)

**Benefits Achieved:**
1. **Test Isolation**: Each validator is independently testable with exactly one failure reason
2. **Compile-Time Safety**: Error messages are typed classes, not magic strings
3. **Open/Closed Principle**: New validations are new classes; existing validators never change
4. **Simple Tests**: No complex test configuration or string matching required
5. **Clear Failures**: Tests fail for exactly one reason, making debugging trivial
6. **Immutability**: Validators have no state and never change after creation

**Tradeoffs Accepted:**
- Many files (appears verbose at first glance)
- Learning curve for developers new to the pattern
- More classes than traditional validation approaches

**Alternatives Rejected:**
```csharp
// REJECTED: Func delegates - loses type safety and testability
container.AddRule(arg => arg != null, "Object is null");

// REJECTED: Consolidated validator - complexity moves to tests
public bool Validate(ICardIdsArgEntity arg)
{
    if (arg == null) return false;
    if (arg.CardIds == null) return false;
    if (arg.CardIds.Count == 0) return false;
    // Complex test setup to verify each condition
}

// REJECTED: String messages - violates No Primitives
throw new ValidationException("Provided object is null");

// REJECTED: Inheritance hierarchies - unnecessary complexity
public abstract class BaseValidator<T> { }
```

##### Key Implementation Rules
1. **One Validator Per Rule**: Each validation rule gets its own class
2. **Three Classes Per Validator**: Main class, Validator, and Message
3. **Immutable Validators**: No state, only behavior
4. **Typed Messages**: Error messages as classes, not strings
5. **Never Modify**: Existing validators should NEVER change
6. **Container Composition**: All validators composed in the container constructor

#### Original Validator Container Pattern Example
```csharp
internal sealed class CardSearchTermArgEntityValidatorContainer
    : ValidatorActionContainer<CardSearchTermArgEntity, EntryFailureStatus>
{
    public CardSearchTermArgEntityValidatorContainer() : base(
        new NotNullArgEntityValidator(),
        new CardSearchTermArgEntityValidator())
    { }
}
```

#### Validate-Map-Delegate Pattern
```csharp
public async Task<IOperationResponse<ScryfallCardCollectionOutEntity>> CardNameSearchAsync(CardSearchTermArgEntity searchTermArgEntity)
{
    // 1. Validate
    IValidatorActionResult<EntryFailureStatus> validationResult = _searchTermArgValidator.Validate(searchTermArgEntity);

    if (validationResult.Failed) return new FailureOperationResponse<ScryfallCardCollectionOutEntity>(/*...*/);

    // 2. Map
    CardNameSearchItrEntity searchItrEntity = _searchTermArgToItrMapper.Map(searchTermArgEntity);

    // 3. Delegate
    IOperationResponse<CardNameSearchResultCollectionItrEntity> response = await _cardDomainService.CardNameSearchAsync(searchItrEntity).ConfigureAwait(false);

    // 4. Map response
    return MapResponse(response);
}
```

### Mapper Pattern Implementation (CRITICAL)

The Entry Layer uses explicit mapper classes for ALL data transformations. This is one of the most commonly mis-implemented patterns.

#### Correct Implementation - Explicit Mapper Classes
```csharp
// CORRECT: Dedicated mapper class with single responsibility
internal sealed class CardSearchTermArgToItrMapper
    : ICreateMapper<CardSearchTermArgEntity, CardNameSearchItrEntity>
{
    public CardNameSearchItrEntity Map(CardSearchTermArgEntity source)
    {
        return new CardNameSearchItrEntity
        {
            SearchTerm = source.SearchTerm,
            // Explicit property mapping
        };
    }
}

// CORRECT: Usage in Entry Service
public class CardEntryService
{
    private readonly ICreateMapper<CardSearchTermArgEntity, CardNameSearchItrEntity> _searchTermMapper;

    public async Task<IOperationResponse<OutEntity>> SearchAsync(CardSearchTermArgEntity args)
    {
        // Explicit mapper invocation
        CardNameSearchItrEntity itrEntity = _searchTermMapper.Map(args);
        // ...
    }
}
```

#### Common Anti-Patterns to AVOID
```csharp
// WRONG: Inline mapping in service methods
public async Task<IOperationResponse<OutEntity>> SearchAsync(CardSearchTermArgEntity args)
{
    // DON'T DO THIS - mapping logic leaked into service
    var itrEntity = new CardNameSearchItrEntity
    {
        SearchTerm = args.SearchTerm
    };
}

// WRONG: Static mapper methods
public static class CardMapper
{
    // DON'T DO THIS - violates MicroObjects principles (no statics)
    public static ItrEntity ToItr(ArgEntity arg) { }
}

// WRONG: Extension methods for mapping
public static class MappingExtensions
{
    // DON'T DO THIS - hidden dependencies and implicit behavior
    public static ItrEntity ToItr(this ArgEntity arg) { }
}

// WRONG: AutoMapper or reflection-based mapping
services.AddAutoMapper(); // DON'T DO THIS - violates explicit mapping principle
```

#### Mapper Composition for Complex Transformations
```csharp
// Response mapper that composes other mappers
internal sealed class CardCollectionItrToOutMapper
    : ICreateMapper<ICardCollectionItrEntity, CardCollectionOutEntity>
{
    private readonly ICreateMapper<ICardItrEntity, CardOutEntity> _cardMapper;

    public CardCollectionItrToOutMapper()
    {
        _cardMapper = new CardItrToOutMapper();
    }

    public CardCollectionOutEntity Map(ICardCollectionItrEntity source)
    {
        return new CardCollectionOutEntity
        {
            Cards = source.Cards.Select(card => _cardMapper.Map(card)),
            TotalCount = source.Cards.Count()
        };
    }
}
```

#### Key Mapper Rules
1. **One Mapper Per Transformation**: Each Arg→Itr and Itr→Out transformation gets its own mapper
2. **Explicit Property Mapping**: Every property is explicitly mapped, no magic
3. **Immutable Mappers**: Mappers have no state, only behavior
4. **Interface Contract**: All mappers implement `ICreateMapper<TSource, TDestination>`
5. **Constructor Injection**: Mappers are injected as dependencies, not created inline
6. **No Bidirectional Mapping**: Separate mappers for each direction of transformation

### Naming Conventions
- **Services**: `*EntryService` (e.g., `CardEntryService`)
- **Validators**: `*ArgEntityValidator` and `*ArgEntityValidatorContainer`
- **Mappers**: `*ArgsToItrMapper` for argument transformation
- **Response Mappers**: `*ItrToOutEntityCollectionMapper` for response transformation
- **Mapper Interfaces**: `ICreateMapper<TSource, TDestination>` consistently used

### Common Abstractions
```csharp
// Validator action interface
public interface IValidatorAction<TItem, TFailureStatus>
{
    IValidatorActionResult<TFailureStatus> Validate(TItem item);
}

// Validator result
public interface IValidatorActionResult<TFailureStatus>
{
    bool Failed { get; }
    TFailureStatus FailureStatus { get; }
}
```

### MicroObjects Patterns Applied
- **Composition of Validators**: Multiple small validators composed in containers
- **Immutable Validators**: Validators have no mutable state
- **Early Return Pattern**: Return immediately on validation failure
- **Interface Segregation**: Specific validators for each validation concern

---

## Layer 3: Shared Layer (`Lib.Shared.*`)

### Purpose
Provides cross-cutting concerns, action patterns, and shared abstractions used across all layers.

### Key Design Patterns

#### Action Pattern
```csharp
// Filter action
public interface IFilterAction<TItem>
{
    IFilterActionResult<TItem> Filter(TItem item);
}

// Validator action
public interface IValidatorAction<TItem, TFailureStatus>
{
    IValidatorActionResult<TFailureStatus> Validate(TItem item);
}

// Enrichment action
public interface IEnrichmentAction<TItem>
{
    TItem Enrich(TItem item);
}
```

#### Operation Response Pattern
```csharp
public interface IOperationResponse<TResponseData>
{
    bool Success { get; }
    TResponseData? Data { get; }
    string ErrorMessage { get; }
    HttpStatusCode HttpStatusCode { get; }
}
```

#### Container Composition Pattern
```csharp
public abstract class ValidatorActionContainer<TItem, TFailureStatus>
    : IValidatorAction<TItem, TFailureStatus>
{
    private readonly IValidatorAction<TItem, TFailureStatus>[] _validators;

    protected ValidatorActionContainer(params IValidatorAction<TItem, TFailureStatus>[] validators)
    {
        _validators = validators;
    }
}
```

### Naming Conventions
- **Interfaces**: `I*Action` for action patterns
- **Results**: `*ActionResult` for action outcomes
- **Containers**: `*ActionContainer` for action composition
- **Entities**: `I*ItrEntity` for internal layer entities
- **Exceptions**: `OperationException` for operation failures

### Common Abstractions
```csharp
// Entity interfaces
public interface ICardItrEntity
{
    string Id { get; }
    string Name { get; }
}

// Collection interfaces
public interface ICardCollectionItrEntity
{
    IEnumerable<ICardItrEntity> Cards { get; }
}

// Mapper interface
public interface ICreateMapper<TSource, TDestination>
{
    TDestination Map(TSource source);
}
```

### MicroObjects Patterns Applied
- **Pure Interfaces**: Shared layer contains contracts only
- **Composition Over Inheritance**: Action containers compose behaviors
- **Immutable Responses**: Response objects are read-only after creation
- **Type Safety Through Generics**: Strong typing enforced via generic constraints

---

## Layer 4: Domain Layer (`Lib.Domain.*`)

### Purpose
Contains business logic and domain rules that apply universally across all consumers.

### Key Design Patterns

#### Passthrough Service Pattern (Current)
```csharp
internal sealed class QueryCardDomainService : ICardDomainService
{
    private readonly ICardAggregatorService _cardAggregatorService;

    public async Task<IOperationResponse<ICardCollectionItrEntity>> GetCardsAsync(
        CardIdsItrEntity cardIds)
    {
        // Currently passes through to aggregator
        // Ready for business logic insertion
        return await _cardAggregatorService.GetCardsAsync(cardIds)
            .ConfigureAwait(false);
    }
}
```

#### Future Business Logic Insertion Points
```csharp
public async Task<IOperationResponse<ICardCollectionItrEntity>> GetCardsAsync(
    CardIdsItrEntity cardIds)
{
    // Future: Apply business rules
    // - Validate card availability
    // - Apply pricing rules
    // - Check user permissions
    // - Apply card filtering based on user preferences

    var response = await _cardAggregatorService.GetCardsAsync(cardIds)
        .ConfigureAwait(false);

    // Future: Post-processing
    // - Apply business transformations
    // - Enrich with calculated fields

    return response;
}
```

### Naming Conventions
- **Services**: `*DomainService` (e.g., `CardDomainService`)
- **Implementations**: `Query*DomainService` for query operations
- **Interfaces**: `I*DomainService` for contracts

### Common Abstractions
```csharp
public interface ICardDomainService
{
    Task<IOperationResponse<ICardCollectionItrEntity>> GetCardsAsync(CardIdsItrEntity cardIds);
    Task<IOperationResponse<ICardCollectionItrEntity>> GetCardsBySetAsync(SetCodeItrEntity setCode);
}
```

### MicroObjects Patterns Applied
- **Interface First**: Every domain service has an interface
- **Single Responsibility**: Each service handles one domain area
- **Ready for Extension**: Architecture prepared for business logic
- **Composition Pattern**: Domain services compose aggregator services

---

## Layer 5: Aggregator Layer (`Lib.Aggregator.*`)

### Purpose
Orchestrates data retrieval from multiple adapter sources and aggregates responses.

### Key Design Patterns

#### Data Aggregation Pattern
```csharp
internal sealed class QueryCardAggregatorService : ICardAggregatorService
{
    private readonly ICardAdapterService _cardAdapterService;
    private readonly ISetAdapterService _setAdapterService;

    public async Task<IOperationResponse<ICardCollectionItrEntity>> GetCardsBySetAsync(
        SetCodeItrEntity setCode)
    {
        // 1. Get set information
        var setResponse = await _setAdapterService.GetSetAsync(setCode)
            .ConfigureAwait(false);

        // 2. Get cards for set
        var cardsResponse = await _cardAdapterService.GetCardsBySetAsync(setCode)
            .ConfigureAwait(false);

        // 3. Aggregate and return
        return AggregateResponses(setResponse, cardsResponse);
    }
}
```

#### Collection Building Pattern
```csharp
private ICardCollectionItrEntity BuildCollection(
    IEnumerable<ICardItrEntity> cards,
    ISetItrEntity set)
{
    return new CardCollectionItrEntity
    {
        Cards = cards.OrderBy(c => c.CollectorNumber),
        SetInfo = set,
        TotalCount = cards.Count()
    };
}
```

### Naming Conventions
- **Services**: `*AggregatorService` (e.g., `CardAggregatorService`)
- **Implementations**: `Query*AggregatorService` for query operations
- **Collections**: `*CollectionItrEntity` for aggregated results

### Common Abstractions
```csharp
public interface ICardAggregatorService
{
    Task<IOperationResponse<ICardCollectionItrEntity>> GetCardsAsync(CardIdsItrEntity cardIds);
    Task<IOperationResponse<ICardCollectionItrEntity>> GetCardsByArtistAsync(ArtistIdItrEntity artistId);
}
```

### MicroObjects Patterns Applied
- **Orchestration Without Business Logic**: Pure coordination role
- **Error Context Enrichment**: Adds domain context to adapter errors
- **Collection Types**: Explicit collection entities over generic lists
- **Interface Consistency**: Matches domain layer interface signatures

---

## Layer 6: Adapter Layer (`Lib.Adapter.*`)

### Purpose
Interfaces with external systems, transforming between internal and external data representations.

### Key Design Patterns

#### Adapter Pattern
```csharp
internal sealed class CardCosmosQueryAdapter : ICardAdapterService
{
    private readonly ICosmosGopher<ScryfallCardItem> _cosmosGopher;
    private readonly ICreateMapper<ScryfallCardItem, ICardItrEntity> _mapper;

    public async Task<IOperationResponse<ICardItrEntity>> GetCardAsync(string cardId)
    {
        try
        {
            // 1. Call external system
            ScryfallCardItem item = await _cosmosGopher.FetchAsync(cardId)
                .ConfigureAwait(false);

            // 2. Map to internal entity
            ICardItrEntity card = _mapper.Map(item);

            // 3. Return success
            return new SuccessOperationResponse<ICardItrEntity>(card);
        }
        catch (CosmosException ex)
        {
            // 4. Wrap external exception
            throw new CardAdapterException("Failed to retrieve card", ex);
        }
    }
}
```

#### Repository Pattern (Cosmos)
```csharp
internal sealed class CardScribe : CosmosScribe<ScryfallCardItem>
{
    protected override string PartitionKey => "/set_code";
    protected override string ContainerName => "cards";
}
```

### Mapper Pattern Implementation (External to Internal Transformation)

The Adapter Layer uses explicit mapper classes for transforming between external storage entities and internal ITR entities, similar to Entry Layer but with different focus.

#### Correct Implementation - Dedicated Mapper Classes
```csharp
// CORRECT: Explicit mapper for external-to-internal transformation
internal sealed class ScryfallSetItemToSetItemItrEntityMapper
{
    public ISetItemItrEntity Map(ScryfallSetItem scryfallSetItem)
    {
        dynamic data = scryfallSetItem.Data;

        return new SetItemItrEntity
        {
            Id = data.id,
            Code = data.code,
            Name = data.name,
            // Explicit property mapping from external format
            CardCount = data.card_count,
            Digital = data.digital,
            // Handle nullable external fields
            PrintedSize = data.printed_size ?? 0,
            // Complex nested transformations
            Groupings = MapGroupings(data)
        };
    }

    private ICollection<ISetGroupingItrEntity> MapGroupings(dynamic data)
    {
        // Handle external data structure variations
        // Transform nested external objects
    }
}

// CORRECT: Simple delegation mapper in shared location
public sealed class ScryfallCardItemToCardItemItrEntityMapper
{
    public ICardItemItrEntity Map(ScryfallCardItem scryfallCard) =>
        new CardItemItrEntity(scryfallCard);
}

// CORRECT: Usage in Adapter
internal sealed class CardCosmosQueryAdapter : ICardQueryAdapter
{
    private readonly ScryfallCardItemToCardItemItrEntityMapper _cardMapper;

    public async Task<IOperationResponse<ICardItemItrEntity>> GetCardAsync(string cardId)
    {
        // 1. Fetch from external system
        ScryfallCardItem externalItem = await _gopher.FetchAsync(cardId)
            .ConfigureAwait(false);

        // 2. Map using explicit mapper
        ICardItemItrEntity internalEntity = _cardMapper.Map(externalItem);

        // 3. Return internal representation
        return new SuccessOperationResponse<ICardItemItrEntity>(internalEntity);
    }
}
```

#### Key Differences from Entry Layer Mappers

| Aspect | Entry Layer Mappers | Adapter Layer Mappers |
|--------|-------------------|---------------------|
| **Direction** | ArgEntity → ItrEntity → OutEntity | ExtEntity → ItrEntity |
| **Purpose** | Request/Response transformation | External system integration |
| **Location** | `[Commands\|Queries]/Mappers/` | `[Commands\|Queries]/Mappers/` or `Lib.Aggregator.Scryfall.Shared/Mappers/` |
| **Complexity** | Simple property mapping | Handles external format variations |
| **Null Handling** | Validation rejects nulls | Gracefully handles missing external data |
| **Error Handling** | Validation failures | External system inconsistencies |

#### Adapter Mapper Organization
```
Lib.Adapter.Sets/
├── Queries/
│   └── Mappers/
│       └── ScryfallSetItemToSetItemItrEntityMapper.cs  // Set-specific mapper
│
Lib.Adapter.Cards/
├── Queries/
│   └── CardCosmosQueryAdapter.cs  // Uses shared mapper
│
Lib.Aggregator.Scryfall.Shared/
└── Mappers/
    └── ScryfallCardItemToCardItemItrEntityMapper.cs  // Shared across adapters
```

#### Common Anti-Patterns to AVOID
```csharp
// WRONG: Inline mapping in adapter methods
public async Task<IOperationResponse<ICardItemItrEntity>> GetCardAsync(string id)
{
    var external = await _gopher.FetchAsync(id);
    // DON'T DO THIS - mapping logic leaked into adapter
    return new CardItemItrEntity
    {
        Id = external.Data.id,
        Name = external.Data.name
    };
}

// WRONG: Using AutoMapper or reflection
services.AddAutoMapper(typeof(AdapterMappingProfile));

// WRONG: Extension methods for mapping
public static ICardItemItrEntity ToInternal(this ScryfallCardItem external) { }

// WRONG: Static mapper classes
public static class AdapterMapper
{
    public static ItrEntity MapToInternal(ExtEntity ext) { } // Violates no statics
}
```

#### External Data Handling Patterns
```csharp
// Handling dynamic external data structures
internal sealed class ScryfallSetItemToSetItemItrEntityMapper
{
    public ISetItemItrEntity Map(ScryfallSetItem scryfallSetItem)
    {
        dynamic data = scryfallSetItem.Data;

        // Handle nullable external fields
        TcgPlayerId = data.tcgplayer_id ?? 0,

        // Handle missing properties with try-catch
        ICollection<ISetGroupingItrEntity> groupings = [];
        try
        {
            dynamic groupingsData = data.groupings;
            if (groupingsData != null)
            {
                // Transform complex nested structures
                groupings = TransformGroupings(groupingsData);
            }
        }
        catch (RuntimeBinderException)
        {
            // Property doesn't exist in external data
        }

        return new SetItemItrEntity { Groupings = groupings };
    }
}
```

#### Key Adapter Mapper Rules
1. **External Focus**: Handle variations in external data formats
2. **Defensive Mapping**: Gracefully handle missing or null external data
3. **Explicit Transformation**: Every field explicitly mapped
4. **Shared When Possible**: Common mappers in Lib.Aggregator.Scryfall.Shared
5. **No Business Logic**: Pure data transformation only
6. **Exception Resilience**: Continue mapping even with partial data
7. **Type Safety**: Map to strongly-typed ITR entities

### Naming Conventions
- **Services**: `*AdapterService` (e.g., `CardAdapterService`)
- **Implementations**: `*CosmosQueryAdapter` for Cosmos operations
- **Items**: `Scryfall*Item` for external data models
- **Exceptions**: `*AdapterException` for adapter failures
- **Operators**: `*Scribe` for write operations, `*Gopher` for read operations
- **Mappers**: `*ItemTo*ItrEntityMapper` for external-to-internal transformation

### Common Abstractions
```csharp
public interface ICardAdapterService
{
    Task<IOperationResponse<ICardItrEntity>> GetCardAsync(string cardId);
    Task<IOperationResponse<ICardCollectionItrEntity>> GetCardsBySetAsync(string setCode);
}
```

### MicroObjects Patterns Applied
- **External Interface Isolation**: External systems hidden behind adapters
- **Data Model Separation**: Clear boundary between external and internal models
- **Exception Translation**: External exceptions become domain exceptions
- **Resource Wrapping**: External resources wrapped in internal abstractions

---

## Layer 7: Infrastructure Layer (`Lib.Universal`, `Lib.Cosmos`)

### Purpose
Provides core infrastructure, utilities, and low-level services used across all layers.

### Key Design Patterns

#### MonoState Pattern (Configuration)
```csharp
public abstract class MonoStateConfig : IConfig
{
    private static readonly Dictionary<string, string> _configuration = new();

    protected string GetValue(string key) => _configuration[key];
}

public sealed class ConfigCosmosDatabase : MonoStateConfig
{
    public string DatabaseName => GetValue("cosmos:database");
}
```

#### Client Adapter Pattern
```csharp
internal sealed class CosmosClientAdapter : ICosmosClient
{
    private readonly CosmosClient _client;

    public async Task<T> ReadItemAsync<T>(string id, PartitionKey partitionKey)
    {
        var response = await _client.ReadItemAsync<T>(id, partitionKey)
            .ConfigureAwait(false);
        return response.Resource;
    }
}
```

#### Template Method Pattern
```csharp
public abstract class CosmosGopher<TItem> : ICosmosGopher<TItem> where TItem : CosmosItem
{
    protected abstract string ContainerName { get; }
    protected abstract string PartitionKey { get; }

    public async Task<TItem> FetchAsync(string id)
    {
        // Template method implementation
    }
}
```

### Naming Conventions
- **MonoState Classes**: `MonoState*` for singleton-like behavior
- **Config Classes**: `Config*` for configuration access
- **Adapters**: `*Adapter` for external service wrapping
- **Base Classes**: Abstract classes for common functionality
- **Operators**: `*Gopher` for read, `*Inquisitor` for query, `*Scribe` for write

### Common Abstractions
```csharp
// Configuration interface
public interface IConfig
{
    string GetValue(string key);
}

// Cosmos operations
public interface ICosmosGopher<TItem>
{
    Task<TItem> FetchAsync(string id);
}

public interface ICosmosInquisitor<TItem>
{
    Task<IEnumerable<TItem>> InquireAsync(string query);
}
```

### MicroObjects Patterns Applied
- **Infrastructure Abstraction**: External services behind interfaces
- **Type-Safe Configuration**: Configuration keys as typed properties
- **Immutable Infrastructure**: Objects don't change after creation
- **Single Responsibility**: Each component has one specific purpose

---

## Cross-Cutting Patterns

### Constructor Chain Pattern
Applied consistently across all layers:
```csharp
// Public constructor for DI container
public ServiceClass(ILogger<ServiceClass> logger)
    : this(new DependencyClass(logger))
{ }

// Private constructor for testing
private ServiceClass(IDependencyClass dependency)
{
    _dependency = dependency;
}
```

### Async/Await Pattern
All async operations follow:
```csharp
public async Task<T> OperationAsync()
{
    return await _service.OperationAsync()
        .ConfigureAwait(false);  // Always use ConfigureAwait(false)
}
```

### Error Response Pattern
Consistent error handling:
```csharp
if (validationResult.Failed)
{
    return new FailureOperationResponse<T>(
        new OperationException(
            "Validation failed",
            HttpStatusCode.BadRequest));
}
```

### Entity Naming Convention
Layer-specific entity suffixes:
- **ArgEntity**: Arguments from external clients (App layer)
- **ItrEntity**: Internal transfer entities (all internal layers)
- **OutEntity**: Output entities to external clients (App layer)
- **ExtEntity**: External system entities (Adapter layer)
- **Item**: Database/storage entities (Adapter layer)

### Interface-First Design
Every implementation has a corresponding interface:
```csharp
public interface ICardService { }
internal sealed class CardService : ICardService { }
```

### Immutability Patterns
```csharp
// Domain objects
public sealed class CardId
{
    private readonly string _value;
    public CardId(string value) => _value = value;
}

// DTOs with init setters
public sealed class CardOutEntity
{
    public string Id { get; init; }
    public string Name { get; init; }
}
```

---

## MicroObjects Implementation Summary

Refer to [MicroObjects_Summary.md] for additional details on how to validate MicroObjects have been implemented properly.

---

## Benefits of This Architecture

### Maintainability
- Clear layer boundaries prevent architectural drift
- Consistent patterns reduce cognitive load
- Easy to locate functionality by layer

### Testability
- Constructor chain pattern enables unit testing
- Interface-first design allows mocking
- Layer isolation enables focused testing

### Extensibility
- New features slot into existing layers
- Domain layer ready for business logic
- Clear extension points in each layer

### Scalability
- Layers can be scaled independently
- Clear data flow prevents bottlenecks
- Adapter pattern allows multiple data sources

### Developer Experience
- Predictable structure across all layers
- Consistent naming conventions
- Clear patterns to follow for new features