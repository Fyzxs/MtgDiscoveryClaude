---
paths:
  - "csharp/src/**/Resolvers/**"
---

# Resolver Pattern

## Purpose

Resolvers **create or retrieve entities based on context**, typically implementing the Null Object pattern when database reads return empty. All Resolvers must derive from base interfaces in `Lib.Shared.Abstractions.Actions.Resolvers`.

## Base Interfaces

### IResolver (Generic Base)

> **See:** `csharp/src/common/Lib.Shared.Abstractions/Actions/Resolvers/IResolver.cs`

**Location**: `common/Lib.Shared.Abstractions/Actions/Resolvers/IResolver.cs`

### ICosmosResolver (Cosmos-Specific)

> **See:** `csharp/src/core/Lib.Cosmos/Resolvers/ICosmosResolver.cs`

**Location**: `core/Lib.Cosmos/Resolvers/ICosmosResolver.cs`

**Prefer `ICosmosResolver`** for resolvers that handle Cosmos read responses.

## Naming Convention

`{Domain}Resolver` — e.g., `UserCardResolver`, `UserSetCardResolver`

## Standard Pattern (Cosmos Read Resolution)

Most resolvers handle Cosmos read responses and create Null Object entities when the read returns not-found:

> **See:** `csharp/src/Lib.Adapters/Lib.Adapter.UserCards/Commands/Resolvers/UserCardResolver.cs`

**Key points:**
- Use `ICosmosResolver<TResolved, TContext>` for consistency
- `TResolved` is the entity type (`ExtEntity`)
- `TContext` is typically `IXfrEntity` (the incoming request with creation data)
- Return the existing entity if found, otherwise create a new one

## Specialized Pattern (Sub-Entity Resolution)

Some resolvers extract or resolve sub-data from an already-resolved entity:

> **See:** `csharp/src/Lib.Adapters/Lib.Adapter.UserSetCards/Commands/Resolvers/UserSetCardGroupResolver.cs`

**Use this pattern when:**
- Input is an already-resolved entity (not `OpResponse`)
- Output is derived/extracted data from the entity
- Resolution is about finding nested data, not handling missing entities

## Location in Adapters

`{Adapter}/Commands/Resolvers/`

## Usage in Command Adapters

> **See:** `csharp/src/Lib.Adapters/Lib.Adapter.UserCards/Commands/AddUserCardAdapter.cs`

## Existing Implementations

### Standard Resolvers (ICosmosResolver)

| Resolver | Resolved | Context |
|----------|----------|---------|
| `UserCardResolver` | `UserCardExtEntity` | `IAddUserCardXfrEntity` |
| `UserSetCardResolver` | `UserSetCardExtEntity` | `IAddCardToSetXfrEntity` |
| `AddSetGroupResolver` | `UserSetCardExtEntity` | `IAddSetGroupToUserSetCardXfrEntity` |
| `UserInfoResolver` | `UserInfoExtEntity` | `IUserInfoXfrEntity` |

### Specialized Resolvers (Base IResolver)

| Resolver | Input | Resolved | Context |
|----------|-------|----------|---------|
| `UserSetCardGroupResolver` | `UserSetCardExtEntity` | `Dictionary<string, FinishGroupExtEntity>` | `IAddCardToSetXfrEntity` |

See: `Lib.Adapter.UserCards/Commands/Resolvers/`, `Lib.Adapter.UserSetCards/Commands/Resolvers/`

## Related Patterns

- **Integrator**: Merge changes into resolved entity — see `integrators.md`
- **Gopher**: Provides the read response to resolve — see `../cosmos/cosmos-gopher.md`
