# Lib.Aggregators Layer

## Purpose
Orchestrate data retrieval from multiple adapters and transform between representation layers. Aggregators **know which adapters to call** and how to combine their responses. They are pure coordination and transformation — never business logic, never external system concerns.

## Layer Pattern

**Composite Service → Passthrough Service → Specialized Aggregators → (Adapters)**

```
IArtistAggregatorService (composite, public)
  ↓ delegates to
ArtistAggregatorService (passthrough service)
  ↓ delegates to
IArtistSearchAggregatorService, ICardsByArtistAggregatorService (specialized aggregators)
  ↓ call
IArtistAdapterService (at adapter layer)
```

See: `Lib.Aggregator.Artists/Apis/`
- Interface: `IArtistAggregatorService.cs` (composite interface)
- Service: `ArtistAggregatorService.cs` (passthrough pattern)
- Operations: `ArtistSearchAggregatorService.cs`, `CardsByArtistAggregatorService.cs`

## Architecture

### Public API
- **Composite Service Interface** (`IArtistAggregatorService`): Inherits from all specialized aggregator interfaces
- **Service Implementation**: Passthrough pattern delegating to specialized aggregators
- **Specialized Aggregator Interfaces**: `IArtistSearchAggregatorService`, `ICardsByArtistAggregatorService`

See: `Lib.Aggregator.Artists/Apis/IArtistAggregatorService.cs` (composite pattern)

### Entity Transformation Pipeline

```
ItrEntity (input from Domain layer)
  ↓ [Mapper: ItrEntity → XfrEntity]
XfrEntity (input for Adapter)
  ↓ [Adapter call]
ExtEntity (response from Adapter)
  ↓ [Mapper: ExtEntity → OufEntity]
OufEntity (output back to Domain layer)
```

**Three Critical Mappers**:
- **ItrToXfr**: Transform domain's ItrEntity → adapter's XfrEntity (request mapping)
- **ExtToOuf**: Transform adapter's ExtEntity → domain's OufEntity (response mapping)
- **ItemToItem**: Transform collection items within a mapping

See: `Lib.Aggregator.Artists/Queries/Mappers/`
- `ArtistSearchTermItrToXfrMapper.cs` (ItrEntity → XfrEntity)
- `ArtistSearchExtToItrMapper.cs` (ExtEntity → OufEntity)

### Aggregator Operation Pattern

Each aggregator operation:
1. Constructor chain (public with logger, private with dependencies)
2. Inject dependencies: adapter service + required mappers
3. Implement Execute method: map → call adapter → handle response → return
4. Never throw exceptions (use IOperationResponse)

See: `Lib.Aggregator.Artists/Queries/ArtistSearchAggregatorService.cs:14-49`
- Constructor chaining: lines 20-34 (logger → mappers → private)
- Execute pattern: lines 36-48 (map request → call adapter → handle failure → map response)
- ConfigureAwait: lines 38, 39, 46

## Actions

Aggregators use cross-cutting action patterns from `Lib.Shared.Abstractions/Actions/`. Always inherit from the provided interfaces:

### ICreateMapper<TSource, TResult> ⭐ PRIMARY PATTERN
**Purpose**: Transform data between layer boundaries (ItrEntity ↔ XfrEntity ↔ OufEntity)

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

**Examples**:
- **Request Mapping** (ItrEntity → XfrEntity): `ArtistSearchTermItrToXfrMapper.cs:10-32`
  - Constructor chaining: lines 14-18
  - Composes dependency mapper: line 12
  - Transform logic: lines 20-31
  - Calls composed mapper: line 22

- **Response Mapping** (ExtEntity → OufEntity): `ArtistSearchExtToItrMapper.cs:11-32`
  - Parallel mapping: lines 21-23 (Task.WhenAll for collection)
  - Build result: lines 25-28
  - Return output entity: line 30

### IFilterAction<TItem, TFailureStatus>
**Purpose**: Exclude items by criteria before sending to adapter

**Interface**: `Lib.Shared.Abstractions/Actions/Filters/IFilterAction.cs:5-11`

Used for: Pre-adapter validation, permission checks, cache validation

### IEnrichmentAction<TTarget>
**Purpose**: Add supplemental data to aggregated results

**Interface**: `Lib.Shared.Abstractions/Actions/Enrichments/IEnrichmentAction.cs:5-13`

Used for: Adding computed fields, fetching related data, enriching collections

### IValidatorAction<TItem, TFailureStatus>
**Purpose**: Validate aggregator input/output

**Interface**: `Lib.Shared.Abstractions/Actions/Validators/IValidatorAction.cs`

Used for: Input validation before adapter calls, response validation from adapters

### IResolver<TIdentifier, TResult>
**Purpose**: Resolve identifier to actual object

**Interface**: `Lib.Shared.Abstractions/Actions/Resolvers/IResolver.cs`

Used for: Looking up cached aggregated data, resolving references

### IIntegrator<TTarget, TSource>
**Purpose**: Merge delta changes into existing result

**Interface**: `Lib.Shared.Abstractions/Actions/Integrators/IIntegrator.cs`

Used for: Combining multiple adapter responses, incremental updates

### ITransformationAction<TItem>
**Purpose**: Transform item in-place

**Interface**: `Lib.Shared.Abstractions/Actions/Transformations/ITransformationAction.cs`

Used for: Normalizing aggregated data, standardizing formats

## Key Rules

1. **Constructor Chain**: Public constructor with logger, private with dependencies (adapter service + mappers)
2. **Mapper Injection**: Always inject mappers as dependencies, never create inline
3. **ICreateMapper for ALL Transforms**: ALWAYS inherit from `ICreateMapper<TSource, TResult>`, never inline mapping logic
4. **Orchestration Only**: Coordinate adapter calls; NO business logic (belongs in Domain)
5. **No External System Logic**: Don't handle Cosmos details, API specifics (Adapter's job)
6. **IOperationResponse**: All methods return `IOperationResponse<T>`, never throw exceptions
7. **ConfigureAwait(false)**: All async calls include `.ConfigureAwait(false)`
8. **Entity Boundaries**: ItrEntity in, OufEntity out — never expose ExtEntity upward

## Common Patterns

### Passthrough Aggregator Service
Primary service delegates to specialized aggregators. See: `ArtistAggregatorService.cs:11-25`

### Request Mapping (ItrEntity → XfrEntity)
Transform domain entity to adapter input format. See: `ArtistSearchAggregatorService.cs:38` (line calls mapper)

### Response Mapping (ExtEntity → OufEntity)
Transform adapter response to domain output format. See: `ArtistSearchAggregatorService.cs:46` (line calls mapper)

### Parallel Collection Mapping
Map multiple items concurrently with Task.WhenAll. See: `ArtistSearchExtToItrMapper.cs:21-23`

### Constructor Chaining with Mappers
Inject adapter + mappers. See: `ArtistSearchAggregatorService.cs:20-34`

## Naming Conventions

| Item | Pattern | Example |
|------|---------|---------|
| **Service Interface** | `I{Domain}AggregatorService` | `IArtistAggregatorService` |
| **Service Implementation** | `{Domain}AggregatorService` | `ArtistAggregatorService` |
| **Aggregator Interface** | `I{Operation}{Domain}AggregatorService` | `IArtistSearchAggregatorService` |
| **Aggregator Implementation** | `{Operation}{Domain}AggregatorService` | `ArtistSearchAggregatorService` |
| **Request Mapper** | `I{Entity}ItrToXfrMapper` | `IArtistSearchTermItrToXfrMapper` |
| **Response Mapper** | `I{Entity}ExtToOufMapper` | `IArtistSearchExtToItrMapper` |
| **Item Mapper** | `I{Entity}ExtToItrEntityMapper` | `IArtistNameTrigramDataExtToItrEntityMapper` |
| **Xfr Entity** | `I{Entity}XfrEntity` | `IArtistSearchTermXfrEntity` |
| **Ouf Entity** | `I{Entity}OufEntity` | `IArtistSearchResultCollectionOufEntity` |

## Data Flow Checklist

When implementing a new aggregator:
- ✓ Receive ItrEntity from Domain layer
- ✓ Map ItrEntity → XfrEntity (inject mapper)
- ✓ Call adapter with XfrEntity (inject adapter service)
- ✓ Check `IsFailure` on adapter response
- ✓ Map ExtEntity → OufEntity (inject mapper)
- ✓ Return IOperationResponse<OufEntity>
- ✓ All async calls use ConfigureAwait(false)
- ✓ No business logic (Domain layer owns that)
- ✓ No external system concerns (Adapter layer owns that)

See: `ArtistSearchAggregatorService.cs:36-48` (complete example)

## When to Add a New Aggregator

1. Create interface in `Apis/` (inherits from specialized aggregator interface)
2. Create service in `Apis/` (passthrough pattern)
3. Create aggregator operation class (implements behavior)
4. Create request mapper `ItrEntity → XfrEntity`
5. Create response mapper `ExtEntity → OufEntity`
6. Register in composite service interface

See structure: `Lib.Aggregator.Artists/`
