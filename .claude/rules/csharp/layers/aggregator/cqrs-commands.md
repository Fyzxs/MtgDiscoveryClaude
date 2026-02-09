---
paths:
  - "csharp/src/Lib.Aggregators/**/Commands/*"
---

# Aggregator Command Behaviors

## Router Class: `{Domain}CommandAggregator`

The `{Domain}CommandAggregator` implements the `I{Domain}CommandAggregatorService` interface defined in `Apis/`. It constructs all command behavior classes via constructor chaining and delegates each method to the appropriate behavior's `Execute()`.

### Naming

- Class: `{Domain}CommandAggregator` (internal scope, `Aggregator` suffix)
- Implements: `I{Domain}CommandAggregatorService` (public interface from `Apis/`)

### Pattern

> **See:** `csharp/src/Lib.Aggregators/Lib.Aggregator.Collections/Commands/CollectionCommandAggregator.cs`

Each method delegates to the corresponding behavior's `Execute(input, cancellationToken)` — no logic in the router.

## Behavior Classes

These are targeted classes following single responsibility — each implements a single command behavior.

### Naming

The class and interfaces use `{Behavior}Aggregator` with the `I` prefix for interfaces. The `Aggregator` suffix (not `AggregatorService`) signals internal scope.

### Interface Pattern

All interfaces must inherit from `IOperationResponseService<TInput, TOutput>` — never define `Execute` manually.

> **See:** `csharp/src/Lib.Aggregators/Lib.Aggregator.UserWishlistCards/Commands/IAddUserWishlistCardAggregator.cs`

### Entity Type Conventions

- `TInput` must be an `IItrEntity`
- `TOutput` must be an `IOufEntity`

## Standard Execute Flow

All command aggregators follow this sequence:

1. Map `ItrEntity` to `XfrEntity` via injected mapper
2. Call adapter service with `XfrEntity` + `CancellationToken`
3. Check `response.IsFailure` — return `FailureOperationResponse` if failed
4. Map `ExtEntity` to `OufEntity` via injected mapper
5. Return `SuccessOperationResponse<IOufEntity>`

### Standard Dependencies (3)

```
1. Adapter service
2. ItrToXfr mapper
3. ExtToOuf mapper (or collection mapper)
```

## Multi-Step Transaction Pattern

Some commands require multiple adapter calls with rollback on partial failure. This is an acceptable deviation from the standard 5-step flow when a single command must coordinate across multiple adapters.

### Pattern

> **See:** `csharp/src/Lib.Aggregators/Lib.Aggregator.UserCards/Commands/AddUserCardAggregatorService.cs`

### When to Use

- The command must coordinate writes across 2+ adapters
- Partial failure of secondary operations requires compensating the primary
- Dependencies will exceed the standard 3 (typically 5-6: multiple adapter services + mappers + rollback mapper)

### Reference

`Lib.Aggregator.UserCards/Commands/AddUserCardAggregatorService.cs` demonstrates this pattern.
