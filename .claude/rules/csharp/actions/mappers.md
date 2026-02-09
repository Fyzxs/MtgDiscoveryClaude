---
paths:
  - "csharp/src/**/Mappers/**
---

# Mapper Pattern

## Purpose

Mappers **transform data between types**, creating new instances from source objects. They handle layer boundary crossings and type transformations.

## Base Interfaces

All mappers should derive from base interfaces in `Lib.Shared.Abstractions.Actions.Mappers`:

```csharp
// Single source transformation
public interface ICreateMapper<in TSource, TResult>
{
    Task<TResult> Map(TSource source);
}

// Dual source transformation
public interface ICreateMapper<in TSourceFirst, in TSourceSecond, TResult>
{
    Task<TResult> Map(TSourceFirst source1, TSourceSecond source2);
}

// In-place mapping (modifies result)
public interface IMapper<in TSource, in TResult>
{
    Task Map(TSource source, TResult result);
}
```

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

```csharp
// Interface extends base
internal interface IUserCardsSetXfrToArgsMapper
    : ICreateMapper<IUserCardsSetXfrEntity, UserCardItemsBySetExtEntity>;

// Implementation
internal sealed class UserCardsSetXfrToArgsMapper : IUserCardsSetXfrToArgsMapper
{
    public Task<UserCardItemsBySetExtEntity> Map(IUserCardsSetXfrEntity source)
    {
        UserCardItemsBySetExtEntity args = new()
        {
            UserId = source.UserId,
            SetId = source.SetId
        };
        return Task.FromResult(args);
    }
}
```

## Synchronous vs Async

Base interfaces are async (`Task<>`) for consistency. For synchronous operations, wrap with `Task.FromResult()`:

```csharp
public Task<ReadPointItem> Map(IAddUserCardXfrEntity source)
{
    ReadPointItem readPoint = new()
    {
        Id = new ProvidedCosmosItemId(source.CardId),
        Partition = new ProvidedPartitionKeyValue(source.UserId)
    };
    return Task.FromResult(readPoint);
}
```

## Three-Input Mappers

The base interfaces support 1-2 inputs. For mappers requiring 3+ inputs, define a custom interface:

```csharp
internal interface IUserCardMetadataMapper
{
    UserCardExtEntity Map(
        UserCardExtEntity existing,
        IAddUserCardXfrEntity newData,
        IEnumerable<UserCardDetailsExtEntity> updatedCollectedList);
}
```

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

```csharp
internal sealed class CollectionUserCardExtToOufMapper
    : CollectionCreateMapper<UserCardExtEntity, IUserCardOufEntity>,
      ICollectionUserCardExtToOufMapper
{
    public CollectionUserCardExtToOufMapper() : base(new UserCardExtToOufMapper()) { }
}
```

### `ChildCollectionMapper<TChildSource, TChildResult>`

Protected helper for mappers that need to map a child collection as part of a larger mapping. Exposes `MapChildren()` for use in the parent mapper's `Map()` method.

```csharp
internal sealed class ArtistSearchExtToOufMapper
    : ChildCollectionMapper<ArtistNameTrigramDataExtEntity, IArtistSearchResultOufEntity>,
      IArtistSearchExtToOufMapper
{
    public ArtistSearchExtToOufMapper() : base(new ArtistNameTrigramDataExtToOufEntityMapper()) { }

    public async Task<IArtistSearchResultCollectionOufEntity> Map(IEnumerable<ArtistNameTrigramDataExtEntity> source)
    {
        IArtistSearchResultOufEntity[] mappedArtists = await MapChildren(source).ConfigureAwait(false);
        return new ArtistSearchResultCollectionOufEntity { Artists = mappedArtists };
    }
}
```

**Location**: `Lib.Shared.Abstractions/Actions/Mappers/`

## Related Patterns

- **Integrator**: Uses mappers for merge operations — see `integrators.md`
