---
paths:
  - "csharp/src/**/*Inquisition.cs"
---

# Cosmos Inquisition Pattern

## Purpose

Inquisitions provide **parameterized query operations** for Cosmos DB. They execute queries against containers and return collections of results.

## Base Types

- **Non-parameterized**: `ICosmosInquisition`
- **Parameterized**: `ICosmosInquisition<TParameters>`
- **Location**: `core/Lib.Cosmos/Apis/Operators/ICosmosInquisition.cs`

## Method Signatures

> **See:** `csharp/src/core/Lib.Cosmos/Apis/Operators/ICosmosInquisition.cs`

## Naming Conventions

| Type | Pattern | Example |
|------|---------|---------|
| Inquisition | `{Query}Inquisition` | `UserCardItemsBySetInquisition` |
| Parameters | `{Query}InquisitionArgs` or `{Query}ExtEntity` | `UserCardItemsBySetExtEntity` |

**Note**: Parameter entities use the singular suffix `ExtEntity`.

## Implementation Pattern

> **See:** `csharp/src/Lib.Adapters/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitions/UserCardItemsBySetInquisition.cs`

**Key points:**
- Implement `ICosmosInquisition<TParameters>`
- Use `ICosmosInquisitor` for query execution
- Use `InquiryDefinition` for SQL query definition
- Parameters bound via `WithParameter()`
- Always specify partition key for efficiency

## Locations

| Type | Path |
|------|------|
| Inquisitions | `Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitions/` |
| Parameter args | `Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitions/Entities/` |

## Parameter Entity Pattern

> **See:** `csharp/src/Lib.Adapters/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitions/Entities/CardsBySetIdInquisitionArgs.cs`

**Key points:**
- Use `init` for immutable properties
- Keep focused — only include query parameters

## Existing Implementations

| Inquisition | Parameters |
|-------------|------------|
| `UserCardItemsBySetInquisition` | `UserCardItemsBySetExtEntity` |
| `UserCardItemsByNameInquisition` | `UserCardItemsByNameExtEntity` |
| `CardsByArtistIdInquisition` | `CardsByArtistIdInquisitionArgs` |
| `CardNameTrigramSearchInquisition` | `CardNameTrigramSearchInquisitionArgs` |
| `AllUserSetCardsInquisition` | (non-parameterized) |

See: `Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitions/` for full list.

## Related Patterns

- **Gopher**: Point-read operations — see `cosmos-gopher.md`
- **Scribe**: Write operations — see `cosmos-scribe.md`
