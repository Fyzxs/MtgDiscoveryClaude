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

> **See:** `csharp/src/core/Lib.Cosmos/Apis/Operators/CosmosGopher.cs`

## Naming Convention

`{Domain}Gopher` — e.g., `UserCardsGopher`, `CollectionGopher`, `ScryfallSetItemsGopher`

## Implementation Pattern

> **See:** `csharp/src/Lib.Adapters/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Gophers/UserCardsGopher.cs`

**Key points:**
- Inherit from `CosmosGopher`
- Pass the appropriate `CosmosContainer` to the base constructor
- Constructor takes `ILogger` for diagnostics

## Location

All Gopher implementations live in:
`Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Gophers/`

## Usage Example

> **See:** `csharp/src/Lib.Adapters/Lib.Adapter.UserCards/Commands/AddUserCardAdapter.cs` (Gopher usage pattern)

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

- **Scribe**: Write operations — see `cosmos-scribe.md`
- **Inquisition**: Query operations — see `cosmos-inquisition.md`
