---
paths:
  - "csharp/src/Lib.Aggregator/**/Commands/*"
---

# Adapter Command Behaviors

The concrete implementation of the `I{Type}CommandAggregator` interface defined in the APIs folder is here.
It uses constructor chaining to create instances of each behavior it exposes.

These are very targeted classes, following single responsibility by implementing a single behavior.

## Interface Pattern

The class and interfaces are named like `{Behavior}Aggregator` Interfaces having the `I` prefix.

The `I{Behavior}Aggregator` must implement `IOperationResponseService` with the appropriate types.

## Entity Type Conventions

- `TInput` must be a `IItrEntity` (Inflow internal entity)
- `TOutput` must be an `OufEntity` (Outflow internal entity)

No exceptions.
