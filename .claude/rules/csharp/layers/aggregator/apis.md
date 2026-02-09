---
paths:
  - "csharp/src/Lib.Aggregators/**/Apis/**"
---

# Aggregator APIs Folder

The `Apis/` folder is the **public contract** for an aggregator project. Everything here MUST be `public` scoped. Internal operation classes do NOT belong here.

## Scope Rule

- **Public scope** (`Apis/`): Uses `AggregatorService` suffix — e.g., `CollectionsAggregatorService`
- **Internal scope** (`Commands/`, `Queries/`): Uses `Aggregator` suffix — e.g., `AddUserCardAggregator`

This distinction signals visibility: `AggregatorService` = consumed outside the project; `Aggregator` = internal implementation detail.

## Files in Apis/

| File | Purpose |
|------|---------|
| `I{Domain}AggregatorService.cs` | Composite interface — inherits both command and query interfaces |
| `{Domain}AggregatorService.cs` | Passthrough facade — delegates to command/query routers |
| `I{Domain}CommandAggregatorService.cs` | Command interface — defines all command operations |
| `I{Domain}QueryAggregatorService.cs` | Query interface — defines all query operations |

For single-CQRS projects (query-only or command-only), the composite interface inherits only the relevant CQRS interface, and the facade has a single dependency.

## Passthrough Facade Pattern

The `{Domain}AggregatorService` in `Apis/` is a **pure passthrough facade**. It constructs command/query router classes and delegates every method call directly. It MUST NOT contain any logic.

### Constructor Pattern

- **Both command + query**: 2 dependencies (command router + query router)
- **Query-only or command-only**: 1 dependency (single router)

### Implementation

```csharp
public sealed class CollectionsAggregatorService : ICollectionsAggregatorService
{
    private readonly ICollectionCommandAggregatorService _commandAggregator;
    private readonly ICollectionQueryAggregatorService _queryAggregator;

    public CollectionsAggregatorService(ILogger logger) : this(
        new CollectionCommandAggregator(logger),
        new CollectionQueryAggregator(logger))
    { }

    private CollectionsAggregatorService(
        ICollectionCommandAggregatorService commandAggregator,
        ICollectionQueryAggregatorService queryAggregator)
    {
        _commandAggregator = commandAggregator;
        _queryAggregator = queryAggregator;
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(
        ICollectionItrEntity entity, CancellationToken cancellationToken)
        => await _commandAggregator.CreateCollectionAsync(entity, cancellationToken)
            .ConfigureAwait(false);

    // Every method follows this exact delegation pattern — no logic, no branching.
}
```

### Key Rules

1. Every method is a single `await` delegation with `.ConfigureAwait(false)`
2. No conditional logic, no mapping, no error handling — pure passthrough
3. Method signatures accept a single `IItrEntity` param plus `CancellationToken`
4. Return type is always `Task<IOperationResponse<IOufEntity>>`

## Method Contracts

All methods on the public `I{Domain}AggregatorService` MUST:
- Accept a single `IItrEntity` parameter plus `CancellationToken`
- Return `Task<IOperationResponse<IOufEntity>>`

Any specific CQRS behavior interfaces do not belong here.

## Reference Implementation

`Lib.Aggregator.Collections/Apis/` follows this pattern exactly and is the canonical reference.
