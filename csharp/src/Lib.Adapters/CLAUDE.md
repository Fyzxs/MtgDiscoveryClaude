# Lib.Adapters Layer

## Purpose
Interface with external systems (Cosmos DB, Scryfall API, etc.), transforming between internal representations (ItrEntity) and external system formats (ExtEntity). Adapters are the **last line of defense** — they handle external system protocols, error translation, and data transformation.

## Layer Pattern

**Composite Interface → Passthrough Service → Specialized Adapters**

```
IArtistAdapterService (composite, public)
  ↓ delegates to
ArtistAdapterService (passthrough service)
  ↓ delegates to
ISearchArtistsAdapter, ICardsByArtistIdAdapter, etc. (specialized adapters)
```

See: `Lib.Adapter.Artists/Apis/`
- Interface: `IArtistAdapterService.cs` (composite interface inheriting from all specialized adapters)
- Service: `ArtistAdapterService.cs` (passthrough pattern, ready for future expansion)

## Architecture

### Public API
- **Composite Service Interface** (`IArtistAdapterService`): Inherits from all specialized adapter interfaces
- **Service Implementation**: Passthrough pattern delegating to specialized adapters
- **Specialized Adapter Interfaces**: `ISearchArtistsAdapter`, `ICardsByArtistIdAdapter`, etc.

See: `Lib.Adapter.Artists/Apis/IArtistAdapterService.cs:1-30` (complete documentation on composition pattern)

### Entity Transformation Pipeline

```
ItrEntity (input from Aggregator layer)
  ↓ [Adapter internal logic]
ExtEntity (external system format)
  ↓ [External system call]
ExtEntity (response from external system)
  ↓ [Mapper: implements ICreateMapper]
OufEntity (output back to Aggregator layer)
```

**Xfr vs Ext Entities**:
- **XfrEntity** (Transfer): Input from Aggregator layer (complete ItrEntity objects)
- **ExtEntity** (External): Format for external systems (Cosmos docs, API response objects)

See: `Lib.Adapter.Artists/Apis/Entities/IArtistIdXfrEntity.cs`, `IArtistNameXfrEntity.cs`, `IArtistSearchTermXfrEntity.cs`

### Specialized Adapter Pattern

Each adapter inherits from its interface and implements:
1. Constructor chain (public with logger, private with dependencies)
2. Query logic (async, no blocking)
3. External system communication
4. Error handling (exceptions become `IOperationResponse`)

See: `Lib.Adapter.Artists/Queries/SearchArtistsAdapter.cs:19-80`
- Constructor chaining: lines 23-25
- Query via Inquisition: lines 44-46
- Error handling: line 48 (IsNotSuccessful check)
- Aggregation logic: lines 29-78
- Return response: line 78

## Actions

Adapters use cross-cutting action patterns from `Lib.Shared.Abstractions/Actions/`. Always inherit from the provided interfaces:

### ICreateMapper<TSource, TResult> ⭐ PRIMARY PATTERN
**Purpose**: Transform data between system boundaries (ExtEntity → OufEntity)

**Interface**: `Lib.Shared.Abstractions/Actions/Mappers/ICreateMapper.cs:10-18`

**Pattern**:
```csharp
internal interface ISomeMapper : ICreateMapper<TSource, TResult>;
internal sealed class SomeMapper : ISomeMapper
{
    private readonly IDependency _dependency;

    public SomeMapper() : this(new Dependency()) { }
    private SomeMapper(IDependency dep) => _dependency = dep;

    public async Task<TResult> Map(TSource source)
    {
        // Transform source to result
        return new TResult { /* mapped properties */ };
    }
}
```

**Example**: `Lib.Adapter.Cards/Queries/Mappers/CollectionCardIdToReadPointItemMapper.cs`
- Interface: `ICollectionCardIdToReadPointItemMapper.cs:7` (extends `ICreateMapper<IEnumerable<string>, ICollection<ReadPointItem>>`)
- Implementation: `CollectionCardIdToReadPointItemMapper.cs:7-18`
- Constructor chaining: lines 11-12, 15
- Map method: line 17 (delegates to composed mapper)

### IFilterAction<TItem, TFailureStatus>
**Purpose**: Exclude items by criteria, return failure status if filtered out

**Interface**: `Lib.Shared.Abstractions/Actions/Filters/IFilterAction.cs:5-11`

Used for: Request-level filtering, permission checks, cache validation

### IEnrichmentAction<TTarget>
**Purpose**: Add data to results after retrieval (async data population)

**Interface**: `Lib.Shared.Abstractions/Actions/Enrichments/IEnrichmentAction.cs:5-13`

Used for: Adding metadata, computing derived fields, async supplementation

### IValidatorAction<TItem, TFailureStatus>
**Purpose**: Validate adapter input/output

**Interface**: `Lib.Shared.Abstractions/Actions/Validators/IValidatorAction.cs`

Used for: Input validation before external calls, response validation

### IResolver<TIdentifier, TResult>
**Purpose**: Resolve identifier to actual object

**Interface**: `Lib.Shared.Abstractions/Actions/Resolvers/IResolver.cs`

Used for: Looking up objects by ID, name resolution

### IIntegrator<TTarget, TSource>
**Purpose**: Merge delta changes into existing object

**Interface**: `Lib.Shared.Abstractions/Actions/Integrators/IIntegrator.cs`

Used for: Updating cached data, merging partial responses

### ITransformationAction<TItem>
**Purpose**: Transform item in-place

**Interface**: `Lib.Shared.Abstractions/Actions/Transformations/ITransformationAction.cs`

Used for: Normalization, standardization, internal format conversion

## Key Rules

1. **Constructor Chain**: Public constructor with logger/config, private with dependencies
2. **Async Always**: All I/O operations async with `ConfigureAwait(false)`
3. **ICreateMapper for Transforms**: ALWAYS inherit from `ICreateMapper<TSource, TResult>`, never inline mapping
4. **IOperationResponse**: All methods return `IOperationResponse<T>`, never throw exceptions
5. **No Business Logic**: Adapters translate between systems; logic belongs in Domain/Aggregator
6. **Xfr to Ext**: Extract primitives from XfrEntity only within adapter methods

## Common Patterns

### Passthrough Adapter Service
Primary service delegates to specialized adapters without additional logic. See: `ArtistAdapterService.cs:28-61`

### Inquisition Pattern for Queries
Parameterized Cosmos queries with strongly-typed arguments. See: `SearchArtistsAdapter.cs:37-46` (using `ICosmosInquisition<ArtistNameTrigramSearchInquisitionArgs>`)

### Error Response Wrapping
External exceptions wrapped in `IOperationResponse`. See: `SearchArtistsAdapter.cs:48` (checking `IsNotSuccessful()`)

### Constructor Chain
See: `SearchArtistsAdapter.cs:23-25` — public logger, private dependencies

## Naming Conventions

| Item | Pattern | Example |
|------|---------|---------|
| **Service Interface** | `I{Domain}AdapterService` | `IArtistAdapterService` |
| **Service Implementation** | `{Domain}AdapterService` | `ArtistAdapterService` |
| **Adapter Interface** | `I{Operation}{Domain}Adapter` | `ISearchArtistsAdapter` |
| **Adapter Implementation** | `{Operation}{Domain}Adapter` | `SearchArtistsAdapter` |
| **Mapper Interface** | `I{Source}To{Target}Mapper` | `ICollectionCardIdToReadPointItemMapper` |
| **Mapper Implementation** | `{Source}To{Target}Mapper` | `CollectionCardIdToReadPointItemMapper` |
| **Xfr Entity** | `I{Entity}XfrEntity` | `IArtistSearchTermXfrEntity` |
| **Ext Entity** | `{Entity}ExtEntity` | `ArtistNameTrigramDataExtEntity` |
| **Exception** | `{Domain}AdapterException` | `ArtistAdapterException` |

## When to Add a New Adapter

1. Create interface in `Apis/` → inherits from specialized adapter interface
2. Create service in `Apis/` → passthrough pattern, delegates to adapter
3. Create Xfr entity in `Apis/Entities/` if accepting input from Aggregator
4. Create adapter implementation → implements behavior
5. Create mappers if transforming ExtEntity → OufEntity
6. Register in composition (add to composite service interface)

See full structure: `Lib.Adapter.Artists/`
