---
paths:
  - "csharp/src/Lib.Aggregators/**/Queries/*"
---

# Aggregator Query Behaviors

The concrete implementation of the `I{Type}QueryAggregatorService` interface defined in the APIs folder is here.
It uses constructor chaining to create instances of each behavior it exposes.

These are very targeted classes, following single responsibility by implementing a single behavior.

## Interface Pattern

The class and interfaces are named like `{Operation}{Domain}AggregatorService` with interfaces having the `I` prefix.
All interfaces must inherit from `IOperationResponseService<TInput, TOutput>` — never define `Execute` manually.

```csharp
internal interface IArtistSearchAggregatorService
    : IOperationResponseService<IArtistSearchTermItrEntity, IArtistSearchResultCollectionOufEntity>;
```

## Constructor Pattern

Public constructor takes `ILogger`, private constructor takes actual dependencies (adapter service + mappers):

```csharp
public CardsByArtistAggregatorService(ILogger logger) : this(
    new ArtistAdapterService(logger),
    new ArtistIdItrToXfrMapper(),
    new CollectionArtistCardExtToOufMapper(),
    new CollectionCardItemItrToOufMapper())
{ }

private CardsByArtistAggregatorService(
    IArtistAdapterService artistAdapterService,
    IArtistIdItrToXfrMapper artistIdToXfrMapper,
    ICollectionArtistCardExtToOufMapper artistCardCollectionMapper,
    ICollectionCardItemItrToOufMapper cardItemItrToOufMapper)
{
    _artistAdapterService = artistAdapterService;
    _artistIdToXfrMapper = artistIdToXfrMapper;
    _artistCardCollectionMapper = artistCardCollectionMapper;
    _cardItemItrToOufMapper = cardItemItrToOufMapper;
}
```

## Execute Flow

All query aggregators follow this sequence:

1. Map `ItrEntity` → `XfrEntity` via injected mapper
2. Call adapter service with `XfrEntity` + `CancellationToken`
3. Check `response.IsFailure` — return `FailureOperationResponse` if failed
4. Map `ExtEntity` → `OufEntity` via injected mapper(s)
   - For collections: `await Task.WhenAll(items.Select(mapper.Map))`
5. Return `SuccessOperationResponse<IOufEntity>`

## Entity Type Conventions

- `TInput` must be an `IItrEntity` (Inflow internal entity)
- `TOutput` must be an `IOufEntity` (Outflow internal entity)

No exceptions.
