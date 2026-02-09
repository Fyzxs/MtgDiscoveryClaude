---
paths:
  - "csharp/src/Lib.Adapters/**/Commands/*"
---

# Adapter Command Behaviors

The concrete implementation of the `I{Type}CommandAdapter` interface defined in the APIs folder is here.
It uses constructor chaining to create instances of each behavior it exposes.

These are very targeted classes, following single responsibility by implementing a single behavior.

## Interface Pattern

The class and interfaces are named like `{Behavior}Adapter` Interfaces having the `I` prefix.

The `I{Behavior}Adapter` must implement `IOperationResponseService` with the appropriate types.

## Entity Type Conventions

- `TInput` must be a `IXfrEntity` (transfer entity from aggregator)
- `TOutput` must be an `ExtEntity` (external entity from storage)

No exceptions. Adapters never accept `ItrEntity` or return `OufEntity`.

## Command Flow

Commands typically follow the read-modify-write pattern:
1. **Read**: Use Gopher to fetch current state
2. **Resolve**: Use Resolver to handle missing entities (Null Object pattern)
3. **Integrate**: Use Integrator to merge changes
4. **Write**: Use Scribe to persist

See: `../cosmos/cosmos-gopher.md`, `../../actions/resolvers.md`, `../../actions/integrators.md`, `../cosmos/cosmos-scribe.md`

## Example

**See:** `csharp/src/Lib.Adapters/Lib.Adapter.UserCards/Commands/IAddUserCardAdapter.cs`
