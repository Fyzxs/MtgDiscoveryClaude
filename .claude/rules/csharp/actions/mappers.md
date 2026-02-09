---
paths:
  - "csharp/src/**/Mappers/**
---

# Mapper Pattern

## Purpose

Mappers **transform data between types**, creating new instances from source objects. They handle layer boundary crossings and type transformations.

## Base Interfaces

All mappers should derive from base interfaces in `Lib.Shared.Abstractions.Actions.Mappers`:

> **See:** `csharp/src/common/Lib.Shared.Abstractions/Actions/Mappers/ICreateMapper.cs` and `csharp/src/common/Lib.Shared.Abstractions/Actions/Mappers/IMapper.cs`

## Naming Conventions

### Same Entity Type (within a layer)

`{Type}{SourceEntity}To{DestinationEntity}Mapper`

When source and destination share the same entity type suffix (e.g., both ExtEntity):
- `UserInfoExtToRegistrationExtMapper` — ExtEntity to ExtEntity

### Different Entity Types (crossing boundaries)

`{SourceType}{SourceEntity}To{DestinationType}{DestinationEntity}Mapper`

When source and destination have different type suffixes:
- `UserInfoItrToXfrMapper` — ItrEntity to XfrEntity (Aggregator preparing for Adapter)
- `UserInfoExtToSyncOufMapper` — ExtEntity to OufEntity (Aggregator processing Adapter response)
- `UserCardsSetXfrToArgsMapper` — XfrEntity to query args (Adapter preparing query)

### Dual-Source Mappers

For mappers with two inputs, name reflects the primary source:
- `ICreateMapper<UserInfoExtEntity, bool, ResultEntity>` → `UserInfoExtToResultExtMapper`

The secondary parameter (e.g., `bool isFirstLogin`) is not reflected in the name.

## Implementation Pattern

> **See:** `csharp/src/Lib.Adapters/Lib.Adapter.UserCards/Queries/Mappers/UserCardsSetXfrToArgsMapper.cs`

## Synchronous vs Async

Base interfaces are async (`Task<>`) for consistency. For synchronous operations, wrap with `Task.FromResult()`:

> **See:** `csharp/src/Lib.Adapters/Lib.Adapter.UserCards/Commands/Mappers/AddUserCardXfrToReadPointMapper.cs`

## Three-Input Mappers

The base interfaces support 1-2 inputs. For mappers requiring 3+ inputs, define a custom interface:

> **See:** `csharp/src/Lib.Adapters/Lib.Adapter.UserCards/Commands/Integrators/UserCardIntegrator.cs` (for three-input mapper usage)

These are rare and typically used internally by Integrators. If you find yourself needing 3+ inputs frequently, consider whether your design could be simplified.

## Location in Adapters

`{Adapter}/Queries/Mappers/` or `{Adapter}/Commands/Mappers/`

## Common Use Cases (Adapter Layer)

| From | To | Example |
|------|-------|---------|
| XfrEntity | Inquisition args | `UserCardsSetXfrToArgsMapper` |
| XfrEntity | ReadPointItem | `AddUserCardXfrToReadPointMapper` |

Note: `ExtEntity → OufEntity` mappers belong in the **Aggregator layer**, not Adapters.

## Collection Mapper Base Classes

For mappers that transform collections of items, two abstract base classes eliminate boilerplate:

### `CollectionCreateMapper<TSource, TResult>`

Implements `ICreateMapper<IEnumerable<TSource>, IEnumerable<TResult>>`. Wraps a single-item mapper and applies it to each element via `Task.WhenAll`.

> **See:** `csharp/src/Lib.Aggregators/Lib.Aggregator.UserCards/Queries/Mappers/CollectionUserCardExtToOufMapper.cs`

### `ChildCollectionMapper<TChildSource, TChildResult>`

Protected helper for mappers that need to map a child collection as part of a larger mapping. Exposes `MapChildren()` for use in the parent mapper's `Map()` method.

> **See:** `csharp/src/Lib.Aggregators/Lib.Aggregator.Artists/Queries/Mappers/ArtistSearchResultCollectionExtToOufMapper.cs`

**Location**: `Lib.Shared.Abstractions/Actions/Mappers/`

## Related Patterns

- **Integrator**: Uses mappers for merge operations — see `integrators.md`
