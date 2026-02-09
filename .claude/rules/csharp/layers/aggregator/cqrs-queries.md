---
paths:
  - "csharp/src/Lib.Aggregators/**/Queries/*"
---

# Aggregator Query Behaviors

## Router Class: `{Domain}QueryAggregator`

The `{Domain}QueryAggregator` implements the `I{Domain}QueryAggregatorService` interface defined in `Apis/`. It constructs all query behavior classes via constructor chaining and delegates each method to the appropriate behavior's `Execute()`.

### Naming

- Class: `{Domain}QueryAggregator` (internal scope, `Aggregator` suffix)
- Implements: `I{Domain}QueryAggregatorService` (public interface from `Apis/`)

### Pattern

> **See:** `csharp/src/Lib.Aggregators/Lib.Aggregator.Collections/Queries/CollectionQueryAggregator.cs`

Each method delegates to the corresponding behavior's `Execute(input, cancellationToken)` — no logic in the router.

## Behavior Classes

These are targeted classes following single responsibility — each implements a single query behavior.

### Naming

The class and interfaces use `{Behavior}Aggregator` with the `I` prefix for interfaces. The `Aggregator` suffix (not `AggregatorService`) signals internal scope.

### Interface Pattern

All interfaces must inherit from `IOperationResponseService<TInput, TOutput>` — never define `Execute` manually.

> **See:** `csharp/src/Lib.Aggregators/Lib.Aggregator.Artists/Queries/` (contains ArtistSearchAggregator interface and implementation)

### Entity Type Conventions

- `TInput` must be an `IItrEntity`
- `TOutput` must be an `IOufEntity`

## Execute Flow

All query aggregators follow this sequence:

1. Map `ItrEntity` to `XfrEntity` via injected mapper
2. Call adapter service with `XfrEntity` + `CancellationToken`
3. Check `response.IsFailure` — return `FailureOperationResponse` if failed
4. Map `ExtEntity` to `OufEntity` via injected mapper(s)
5. Return `SuccessOperationResponse<IOufEntity>`

### Standard Dependencies (3)

```
1. Adapter service
2. ItrToXfr mapper
3. ExtToOuf mapper (or collection mapper)
```
