# C# Layer Patterns — Cross-Cutting Actions

This document defines the standard action patterns used across Adapters, Aggregators, and Domain layers. When you need to perform one of these operations, implement the corresponding interface from `Lib.Shared.Abstractions/Actions/`.

## ICreateMapper<TSource, TResult>

**Purpose**: Transform data between layer boundaries (ExtEntity → OufEntity, ItrEntity → XfrEntity, etc.)

**Interface**: `Lib.Shared.Abstractions/Actions/Mappers/ICreateMapper.cs`

**Action Pattern**:
```csharp
internal interface ISomeAction : IActionInterface;//Possibly with Generics
internal sealed class SomeMapper : ISomeMapper
{
    ...
}
```

**Examples**:
- Adapter: `Lib.Adapter.Cards/Queries/Mappers/CollectionCardIdToReadPointItemMapper.cs:7-18`
- Aggregator: `Lib.Aggregator.Artists/Queries/Mappers/ArtistSearchExtToItrMapper.cs:11-32`

---

## IFilterAction<TItem, TFailureStatus>

**Purpose**: Exclude items by criteria, return failure status if filtered out

**Interface**: `Lib.Shared.Abstractions/Actions/Filters/IFilterAction.cs`

**Usage**: Request-level filtering, permission checks, cache validation

---

## IEnrichmentAction<TTarget>

**Purpose**: Add supplemental data to results after retrieval

**Interface**: `Lib.Shared.Abstractions/Actions/Enrichments/IEnrichmentAction.cs`

**Usage**: Adding metadata, computing derived fields, async supplementation

---

## IValidatorAction<TItem, TFailureStatus>

**Purpose**: Validate adapter input/output

**Interface**: `Lib.Shared.Abstractions/Actions/Validators/IValidatorAction.cs`

**Usage**: Input validation before external calls, response validation

---

## IResolver<TIdentifier, TResult>

**Purpose**: Resolve identifier to actual object

**Interface**: `Lib.Shared.Abstractions/Actions/Resolvers/IResolver.cs`

**Usage**: Looking up cached data, resolving references

---

## IIntegrator<TTarget, TSource>

**Purpose**: Merge delta changes into existing object

**Interface**: `Lib.Shared.Abstractions/Actions/Integrators/IIntegrator.cs`

**Usage**: Combining multiple adapter responses, incremental updates

---

## ITransformationAction<TItem>

**Purpose**: Transform item in-place

**Interface**: `Lib.Shared.Abstractions/Actions/Transformations/ITransformationAction.cs`

**Usage**: Normalization, standardization, internal format conversion

---

## Key Rules

1. **Always use constructor chaining**: Public constructor with logger/config, private with dependencies
2. **Async always**: Use `ConfigureAwait(false)` on all async calls
3. **Inject don't create**: Never instantiate mappers/actions inline; always inject as dependencies
4. **Return responses**: Use `IOperationResponse<T>` instead of throwing exceptions
5. **No logic in constructors**: Only assign dependencies to fields

---

## See Also

- Adapter layer: `Lib.Adapters/CLAUDE.md`
- Aggregator layer: `Lib.Aggregators/CLAUDE.md`
- Domain layer: `Lib.Domains/CLAUDE.md`
