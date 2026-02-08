---
paths:
  - "csharp/src/Lib.Aggregators/**/Queries/*"
---

# Aggregator Query Behaviors

The concrete implementation of the `I{Type}QueryAggregatorService` interface defined in the APIs folder is here.
It uses constructor chaining to create instances of each behavior it exposes.

These are very targeted classes, following single responsibility by implementing a single behavior.

## Interface Pattern

The class and interfaces are named like `{Behavior}Aggregator` with interfaces having the `I` prefix.
All interfaces must inherit from `IOperationResponseService<TInput, TOutput>` — never define `Execute` manually.

```csharp
internal interface IArtistSearchAggregator : IOperationResponseService<IArtistSearchTermItrEntity, IArtistSearchResultCollectionOufEntity>;
```

## Entity Type Conventions

- `TInput` must be a `IItrEntity`
- `TOutput` must be an `IOufEntity`
## Execute Flow

All query aggregators follow this sequence:

1. Map `ItrEntity` → `XfrEntity` via injected mapper
2. Call adapter service with `XfrEntity` + `CancellationToken`
3. Check `response.IsFailure` — return `FailureOperationResponse` if failed
4. Map `ExtEntity` → `OufEntity` via injected mapper(s)
5. Return `SuccessOperationResponse<IOufEntity>`
