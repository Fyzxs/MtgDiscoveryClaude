---
paths:
  - "csharp/src/Lib.Adapters/**/Queries/*"
---

The concrete implementation of the `I{Type}QueryAdapter` interface defined in the APIs folder is here.
It uses constructor chaining to create instances of each behavior it exposes.

These are very targeted classes, following single responsibility by implementing a single behavior.

The class and interfaces are named like `{Behavior}Adapter` Interfaces having the `I` prefix.

The `I{Behavior}Adapter` must implement `IOperationResponseService` with the appropriate types.
The `TInput` must be a `IXferEntity` and the `TOutput` must be an `IExtEntity`

