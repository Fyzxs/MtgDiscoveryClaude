---
paths:
  - "csharp/src/**/Integrators/**"
---

# Integrator Pattern

## Purpose

Integrators **merge changes from a delta object into a state object**, returning a state object with the integrated changes. They implement the read-modify-write pattern for command operations.

## Base Interface

**See:** `csharp/src/common/Lib.Shared.Abstractions/Actions/Integrators/IIntegrator.cs`

**Location**: `common/Lib.Shared.Abstractions/Actions/Integrators/IIntegrator.cs`

## Naming Convention

`{Domain}Integrator` — e.g., `UserCardIntegrator`, `UserSetCardIntegrator`

## Implementation Pattern

**See:** `csharp/src/Lib.Adapters/Lib.Adapter.UserCards/Commands/Integrators/UserCardIntegrator.cs`

**Key points:**
- Extend `IIntegrator<TState, TDelta>` in the interface
- `TState` is typically an `ExtEntity` (the persisted entity)
- `TDelta` is typically an `IXfrEntity` (the incoming change)
- Use mappers for complex merge logic
- Constructor chain pattern for dependencies

## Location in Adapters

`{Adapter}/Commands/Integrators/`

## Usage in Command Adapters

Integrators are used in read-modify-write command flows:

**See:** `csharp/src/Lib.Adapters/Lib.Adapter.UserCards/Commands/AddUserCardAdapter.cs`

## Existing Implementations

| Integrator | State | Delta |
|------------|-------|-------|
| `UserCardIntegrator` | `UserCardExtEntity` | `IAddUserCardXfrEntity` |
| `UserSetCardIntegrator` | `UserSetCardExtEntity` | `IAddCardToSetXfrEntity` |
| `AddSetGroupIntegrator` | `UserSetCardExtEntity` | `IAddSetGroupXfrEntity` |

See: `Lib.Adapter.UserCards/Commands/Integrators/`, `Lib.Adapter.UserSetCards/Commands/Integrators/`

## Related Patterns

- **Resolver**: Create new entities when reads return empty — see `resolvers.md`
- **Scribe**: Write the integrated result — see `../cosmos/cosmos-scribe.md`
- **Gopher**: Read current state — see `../cosmos/cosmos-gopher.md`
