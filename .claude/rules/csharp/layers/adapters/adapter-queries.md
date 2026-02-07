---
paths:
  - "csharp/src/Lib.Adapters/**/Queries/*"
---

# Adapter Query Behaviors

The concrete implementation of the `I{Type}QueryAdapter` interface defined in the APIs folder is here.
It uses constructor chaining to create instances of each behavior it exposes.

These are very targeted classes, following single responsibility by implementing a single behavior.

## Interface Pattern

The class and interfaces are named like `{Behavior}Adapter` Interfaces having the `I` prefix.

The `I{Behavior}Adapter` must implement `IOperationResponseService` with the appropriate types.

## Entity Type Conventions

- `TInput` must be a `IXfrEntity` (transfer entity from aggregator)
- `TOutput` must be an `ExtEntity` (external entity from storage)

No exceptions. Adapters never accept `ItrEntity` or return `OufEntity`.

## Folder Structure

Queries can have an `Entities` folder with concrete implementation of an `XfrEntity` from ANOTHER `Lib.Adapter.{TYPE}` that it calls.

## Example

```csharp
internal interface IUserCardsBySetAdapter
    : IOperationResponseService<IUserCardsSetXfrEntity, IEnumerable<UserCardExtEntity>>;
```
