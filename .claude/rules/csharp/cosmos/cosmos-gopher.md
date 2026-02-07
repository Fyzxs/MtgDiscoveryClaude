---
paths:
  - "csharp/src/**/*Gopher.cs"
---

# Cosmos Gopher Pattern

## Purpose

Gophers provide **point-read operations** for Cosmos DB. They retrieve a single document by its ID and partition key.

## Base Types

- **Interface**: `ICosmosGopher : ICosmosContainerReadOperator`
- **Base class**: `CosmosGopher`
- **Location**: `core/Lib.Cosmos/Apis/Operators/CosmosGopher.cs`

## Method Signature

```csharp
Task<OpResponse<T>> ReadAsync<T>(ReadPointItem item, CancellationToken cancellationToken = default);
```

## Naming Convention

`{Domain}Gopher` — e.g., `UserCardsGopher`, `CollectionGopher`, `ScryfallSetItemsGopher`

## Implementation Pattern

```csharp
public sealed class UserCardsGopher : CosmosGopher
{
    public UserCardsGopher(ILogger logger)
        : base(new UserCardsCosmosContainer(logger))
    { }
}
```

**Key points:**
- Inherit from `CosmosGopher`
- Pass the appropriate `CosmosContainer` to the base constructor
- Constructor takes `ILogger` for diagnostics

## Location

All Gopher implementations live in:
`Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Gophers/`

## Usage Example

```csharp
// In an adapter
private readonly ICosmosGopher _gopher;

public async Task<OpResponse<UserCardExtEntity>> ReadUserCardAsync(ReadPointItem item, CancellationToken ct)
{
    return await _gopher.ReadAsync<UserCardExtEntity>(item, ct).ConfigureAwait(false);
}
```

## Existing Implementations

| Gopher | Container |
|--------|-----------|
| `UserCardsGopher` | User card documents |
| `CollectionGopher` | Collection documents |
| `ScryfallSetItemsGopher` | Set metadata |
| `ScryfallCardItemsGopher` | Card metadata |
| `UserSetCardsGopher` | User set tracking |
| `UserWishlistCardsGopher` | User wishlist |

See: `Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Gophers/` for full list.

## Related Patterns

- **ReadPointItem**: Type-safe carrier for point-read parameters — see `cosmos-read-point.md`
- **Scribe**: Write operations — see `cosmos-scribe.md`
- **Inquisition**: Query operations — see `cosmos-inquisition.md`
