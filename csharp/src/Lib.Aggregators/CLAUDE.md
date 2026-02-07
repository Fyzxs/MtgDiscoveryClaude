# Lib.Aggregators Layer

## Purpose
Orchestrate data retrieval from multiple adapters and transform between representation layers. Aggregators **know which adapters to call** and how to combine their responses. They are pure coordination and transformation — never business logic, never external system concerns.

## Architecture Pattern

**Composite Service → Passthrough Service → Specialized Aggregators → (Adapters)**

Example: `Lib.Aggregator.Artists/Apis/`
- Interface: `IArtistAggregatorService.cs` (composite, inherits all specialized interfaces)
- Service: `ArtistAggregatorService.cs` (passthrough pattern)
- Operations: `ArtistSearchAggregatorService.cs`, `CardsByArtistAggregatorService.cs`

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

## Key Patterns

**Mapper Examples**:
- Request mapping: `ArtistSearchTermItrToXfrMapper.cs:10-32`
- Response mapping: `ArtistSearchExtToOufMapper.cs:11-32` (note: parallel mapping with Task.WhenAll)

**Aggregator Example**:
- Complete operation: `ArtistSearchAggregatorService.cs:36-48` (map request → call adapter → handle response → return)

**Constructor Pattern**:
- Public with logger, private with dependencies (adapter service + mappers): `ArtistSearchAggregatorService.cs:20-34`

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

## Naming Conventions

| Item | Pattern | Example |
|------|---------|---------|
| Service Interface | `I{Domain}AggregatorService` | `IArtistAggregatorService` |
| Service Implementation | `{Domain}AggregatorService` | `ArtistAggregatorService` |
| Aggregator Interface | `I{Operation}{Domain}AggregatorService` | `IArtistSearchAggregatorService` |
| Aggregator Implementation | `{Operation}{Domain}AggregatorService` | `ArtistSearchAggregatorService` |
| Request Mapper | `I{Entity}ItrToXfrMapper` | `IArtistSearchTermItrToXfrMapper` |
| Response Mapper | `I{Entity}ExtToOufMapper` | `IArtistSearchExtToOufMapper` |

## When Adding a New Aggregator

1. Create composite interface in `Apis/`
2. Create passthrough service in `Apis/`
3. Create specialized operation class
4. Create mappers (request and response)
5. Register in composite interface

See: `Lib.Aggregator.Artists/` for complete example

## Additional Patterns

### CacheKey on XfrEntities

Every `XfrEntity` includes a computed `CacheKey` string property used for adapter-level caching:

```csharp
public string CacheKey => $"artist:search:{Normalized}";
```

### Execute Method Convention

All internal aggregator operation services use `Execute(IItrEntity, CancellationToken)` as their single method:

```csharp
public async Task<IOperationResponse<IOufEntity>> Execute(
    IItrEntity input, CancellationToken cancellationToken)
```

### Exception Wrapping

Aggregators may wrap adapter failures in domain-specific exceptions for context:

```csharp
new FailureOperationResponse<IEnumerable<ISealedProductOufEntity>>(
    new SealedProductsAggregatorException($"Failed to retrieve sealed products for set '{input.SetCode}'", response.OuterException));
```

Exception classes live in `Exceptions/` within the aggregator project.

### Collection Mapping with Task.WhenAll

When mapping collections of `ExtEntity` → `OufEntity`, use `Task.WhenAll` for parallel execution:

```csharp
IEnumerable<IOufEntity> oufEntities = await Task.WhenAll(
    response.ResponseData.Select(ext => _extToOufMapper.Map(ext))).ConfigureAwait(false);
```
