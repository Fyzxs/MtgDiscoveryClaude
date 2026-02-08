# Lib.Aggregators Layer

## Purpose
Orchestrate data retrieval from multiple adapters and transform between representation layers. Aggregators **know which adapters to call** and how to combine their responses. They are pure coordination and transformation — never business logic, never external system concerns.

## Architecture Pattern

**Passthrough Facade → Router → Behavior Aggregators → (Adapters)**

```
Apis/{Domain}AggregatorService         (public passthrough facade)
  ├── Commands/{Domain}CommandAggregator   (internal router)
  │     ├── {Behavior}Aggregator           (internal behavior)
  │     └── {Behavior}Aggregator
  └── Queries/{Domain}QueryAggregator      (internal router)
        ├── {Behavior}Aggregator           (internal behavior)
        └── {Behavior}Aggregator
```

### Passthrough Facade

The `{Domain}AggregatorService` in `Apis/` delegates every call to a command or query router. It contains NO logic — pure passthrough with `await ... .ConfigureAwait(false)`.

See: `.claude/rules/csharp/layers/aggregator/apis.md` for full documentation.

### Router Classes

`{Domain}CommandAggregator` and `{Domain}QueryAggregator` sit between the facade and individual behaviors. They construct all behavior classes via constructor chaining and delegate each method to the appropriate behavior's `Execute()`.

See: `cqrs-commands.md` and `cqrs-queries.md` for full documentation.

## Entity Transformation Pipeline

```
ItrEntity (from Domain)  →  [ItrToXfr Mapper]  →  XfrEntity
                                                        ↓
                                          [Adapter call]  ↓  ExtEntity
                                                ↓
                          [ExtToOuf Mapper]  ←  [Adapter response]
                                    ↓
                            OufEntity (to Domain)
```

Three mappers: **ItrToXfr** (request), **ExtToOuf** (response), **ItemToItem** (collection items)

See: `Lib.Aggregator.Artists/Queries/Mappers/` for implementation examples

## Naming Conventions

| Item | Scope | Pattern | Example |
|------|-------|---------|---------|
| Facade Interface | public | `I{Domain}AggregatorService` | `IArtistAggregatorService` |
| Facade Implementation | public | `{Domain}AggregatorService` | `ArtistAggregatorService` |
| Command Router | internal | `{Domain}CommandAggregator` | `CollectionCommandAggregator` |
| Query Router | internal | `{Domain}QueryAggregator` | `CollectionQueryAggregator` |
| Behavior Interface | internal | `I{Behavior}Aggregator` | `IArtistSearchAggregator` |
| Behavior Implementation | internal | `{Behavior}Aggregator` | `ArtistSearchAggregator` |
| Request Mapper | internal | `I{Entity}ItrToXfrMapper` | `IArtistSearchTermItrToXfrMapper` |
| Response Mapper | internal | `I{Entity}ExtToOufMapper` | `IArtistSearchExtToOufMapper` |

**Key rule**: `AggregatorService` suffix = public scope (`Apis/`). `Aggregator` suffix = internal scope (`Commands/`, `Queries/`).

## Key Patterns

**Mapper Examples**:
- Request mapping: `ArtistSearchTermItrToXfrMapper.cs:10-32`
- Response mapping: `ArtistSearchExtToOufMapper.cs:11-32` (note: parallel mapping with Task.WhenAll)

**Aggregator Example**:
- Complete operation: `ArtistSearchAggregatorService.cs:36-48` (map request → call adapter → handle response → return)

**Constructor Pattern**:
- Public with logger, private with dependencies (adapter service + mappers): `ArtistSearchAggregatorService.cs:20-34`

## Collection Mapper Base Classes

Two base classes in `Lib.Shared.Abstractions/Actions/Mappers/` support collection mapping:

### CollectionCreateMapper<TSource, TResult>

Maps `IEnumerable<TSource>` → `TResult[]` using `Task.WhenAll` for parallel execution. Used for top-level collection mapping in services (e.g., mapping an entire adapter response collection).

```csharp
// Base class wraps an item mapper and applies it to each element in parallel
public abstract class CollectionCreateMapper<TSource, TResult>
    : ICreateMapper<IEnumerable<TSource>, IEnumerable<TResult>>
{
    private readonly ICreateMapper<TSource, TResult> _mapper;

    protected CollectionCreateMapper(ICreateMapper<TSource, TResult> mapper) => _mapper = mapper;

    public async Task<IEnumerable<TResult>> Map(IEnumerable<TSource> source)
    {
        ICollection<Task<TResult>> tasks = [.. source.Select(item => _mapper.Map(item))];
        TResult[] results = await Task.WhenAll(tasks).ConfigureAwait(false);
        return results;
    }
}
```

**Usage**:
```csharp
internal sealed class CollectionCollectionExtToOufMapper
    : CollectionCreateMapper<CollectionExtEntity, ICollectionOufEntity>,
      ICollectionCollectionExtToOufMapper
{
    public CollectionCollectionExtToOufMapper() : this(new CollectionExtToOufMapper()) { }
    private CollectionCollectionExtToOufMapper(ICollectionExtToOufMapper mapper) : base(mapper) { }
}
```

### ChildCollectionMapper<TChildSource, TChildResult>

Abstract base for mappers that internally map child collections via `MapChildren()`. Used when a parent entity contains a nested collection that needs mapping (e.g., a `CollectionExtEntity` with an `AuthorizedUsers` list).

```csharp
public abstract class ChildCollectionMapper<TChildSource, TChildResult>
{
    private readonly ICreateMapper<TChildSource, TChildResult> _childMapper;

    protected ChildCollectionMapper(ICreateMapper<TChildSource, TChildResult> childMapper)
        => _childMapper = childMapper;

    protected async Task<TChildResult[]> MapChildren(IEnumerable<TChildSource> children)
    {
        return await Task.WhenAll(children.Select(child => _childMapper.Map(child)))
            .ConfigureAwait(false);
    }
}
```

**Usage**:
```csharp
internal sealed class CollectionExtToOufMapper
    : ChildCollectionMapper<AuthorizedUserExtEntity, IAuthorizedUserOufEntity>,
      ICollectionExtToOufMapper
{
    public CollectionExtToOufMapper() : this(new AuthorizedUserExtToOufMapper()) { }
    private CollectionExtToOufMapper(IAuthorizedUserExtToOufMapper mapper) : base(mapper) { }

    public async Task<ICollectionOufEntity> Map(CollectionExtEntity source)
    {
        IAuthorizedUserOufEntity[] authorizedUsers = await MapChildren(source.AuthorizedUsers)
            .ConfigureAwait(false);
        // ... map remaining properties
    }
}
```

## Exception Pattern

Aggregator-specific exceptions provide domain context for adapter failures. Not all projects need them.

- **Naming**: `{Domain}AggregatorOperationException` or `{Domain}AggregatorException`
- **Base class**: Extends `OperationException` with `HttpStatusCode.InternalServerError`
- **Location**: `Exceptions/` at project root
- **Pragma**: Requires `#pragma warning disable CA1032`

```csharp
#pragma warning disable CA1032
internal sealed class CardAggregatorOperationException : OperationException
#pragma warning restore CA1032
{
    public CardAggregatorOperationException(string message, Exception innerException = null)
        : base(HttpStatusCode.InternalServerError, message, innerException)
    { }
}
```

**Projects with exceptions**: Cards, Sets, SealedProducts.

## Scryfall.Shared — Shared Aggregator Library

`Lib.Aggregator.Scryfall.Shared` is a **shared library** consumed by other aggregator projects. It does NOT follow the standard aggregator project structure (no `Apis/`, `Commands/`, or `Queries/`).

### Structure

```
Lib.Aggregator.Scryfall.Shared/
├── Entities/          (shared OufEntities: CardItemOufEntity, CardItemCollectionOufEntity)
├── Internals/         (ItrEntity implementations used internally by the shared mapper)
└── Mappers/           (shared mappers: DynamicToCardItemOufEntityMapper)
```

### When to Use

Create a shared aggregator library when multiple aggregator projects need the same OufEntities and mappers (e.g., multiple Scryfall-sourced aggregators share `CardItemOufEntity`).

### Internals/ Folder

The `Internals/` folder contains ItrEntity implementations that are internal to the shared library. These are an exception to the ItrEntity exclusion rule because they support the shared mapper's internal transformation logic (mapping Scryfall's dynamic JSON into typed entities).

## Cross-Cutting Patterns

Aggregators use action patterns from `Lib.Shared.Abstractions/Actions/`. See: `common/csharp-layer-patterns.md`

**Primary pattern**: `ICreateMapper<TSource, TResult>` for all transformations

## Key Rules

1. **Constructor Chain**: Public logger → private dependencies (adapter + mappers)
2. **Inject mappers**: Never create inline; always inject as dependencies
3. **Orchestration only**: Coordinate adapters, no business logic
4. **IOperationResponse**: Return responses, never throw exceptions
5. **ConfigureAwait(false)**: All async calls
6. **Entity boundaries**: ItrEntity in, OufEntity out (never expose ExtEntity upward)

## When Adding a New Aggregator

1. Create composite interface in `Apis/`
2. Create passthrough facade in `Apis/`
3. Create router class(es) in `Commands/` and/or `Queries/`
4. Create behavior class(es) with interface
5. Create mappers (request and response)
6. Register in router and facade

See: `Lib.Aggregator.Collections/` for complete example

## Additional Patterns

### CacheKey on XfrEntities

Every `XfrEntity` includes a computed `CacheKey` string property used for adapter-level caching:

```csharp
public string CacheKey => $"artist:search:{Normalized}";
```

### Execute Method Convention

All internal aggregator behavior classes use `Execute(IItrEntity, CancellationToken)` as their single method, inherited from `IOperationResponseService<TInput, TOutput>`.
