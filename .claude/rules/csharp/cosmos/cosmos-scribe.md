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

> **See:** `csharp/src/core/Lib.Cosmos/Apis/Operators/CosmosScribe.cs`

## Naming Convention

`{Domain}Scribe` — e.g., `UserCardsScribe`, `CollectionScribe`, `ScryfallSetItemsScribe`

## Implementation Pattern

> **See:** `csharp/src/Lib.Adapters/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Scribes/UserCardsScribe.cs`

**Key points:**
- Inherit from `CosmosScribe`
- Pass the appropriate `CosmosContainer` to the base constructor
- Constructor takes `ILogger` for diagnostics

## Location

All Scribe implementations live in:
`Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Scribes/`

## Usage Example

> **See:** `csharp/src/Lib.Adapters/Lib.Adapter.UserCards/Commands/AddUserCardAdapter.cs` (Scribe usage pattern)

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
