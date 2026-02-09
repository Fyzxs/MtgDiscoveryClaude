---
paths:
  - "csharp/src/**/*Scribe.cs"
---

# Cosmos Scribe Pattern

## Purpose

Scribes provide **upsert operations** for Cosmos DB. They create or update a document in a container.

## Base Types

- **Interface**: `ICosmosScribe : ICosmosContainerUpsertOperator`
- **Base class**: `CosmosScribe`
- **Location**: `core/Lib.Cosmos/Apis/Operators/CosmosScribe.cs`

## Method Signature

```csharp
Task<OpResponse<T>> UpsertAsync<T>(T item, CancellationToken cancellationToken = default);
```

## Naming Convention

`{Domain}Scribe` — e.g., `UserCardsScribe`, `CollectionScribe`, `ScryfallSetItemsScribe`

## Implementation Pattern

```csharp
public sealed class UserCardsScribe : CosmosScribe
{
    public UserCardsScribe(ILogger logger)
        : base(new UserCardsCosmosContainer(logger))
    { }
}
```

**Key points:**
- Inherit from `CosmosScribe`
- Pass the appropriate `CosmosContainer` to the base constructor
- Constructor takes `ILogger` for diagnostics

## Location

All Scribe implementations live in:
`Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Scribes/`

## Usage Example

```csharp
// In an adapter
private readonly ICosmosScribe _scribe;

public async Task<OpResponse<UserCardExtEntity>> SaveUserCardAsync(UserCardExtEntity entity, CancellationToken ct)
{
    return await _scribe.UpsertAsync(entity, ct).ConfigureAwait(false);
}
```

## Existing Implementations

| Scribe | Container |
|--------|-----------|
| `UserCardsScribe` | User card documents |
| `CollectionScribe` | Collection documents |
| `ScryfallSetItemsScribe` | Set metadata |
| `ScryfallCardItemsScribe` | Card metadata |
| `UserSetCardsScribe` | User set tracking |
| `UserWishlistCardsScribe` | User wishlist |

See: `Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Scribes/` for full list.

## Related Patterns

- **Gopher**: Read operations — see `cosmos-gopher.md`
- **Inquisition**: Query operations — see `cosmos-inquisition.md`
- **Integrator**: Merge changes before upsert — see `../actions/integrators.md`
