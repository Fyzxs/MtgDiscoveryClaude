---
paths:
  - "csharp/src/Lib.Domains/**/Commands/*"
---

# Domain Command Services

## Router Class: `{Domain}CommandDomainService`

The `{Domain}CommandDomainService` implements the `I{Domain}CommandDomainService` interface defined in `Apis/`. It is always `internal sealed`.

### Naming

- Class: `{Domain}CommandDomainService` (internal)
- Implements: `I{Domain}CommandDomainService` (public interface from `Apis/`)

## Two Delegation Patterns

Domain command routers use one of two patterns depending on complexity.

### Pattern A: Direct Passthrough

The router injects a **single aggregator service** and delegates every method call directly to the corresponding aggregator method. No specialized operation services exist.

**Use when**: Each command is a simple 1:1 delegation to an aggregator method.

```csharp
internal sealed class CollectionCommandDomainService : ICollectionCommandDomainService
{
    private readonly ICollectionsAggregatorService _aggregatorService;

    public CollectionCommandDomainService(ILogger logger)
        : this(new CollectionsAggregatorService(logger))
    { }

    private CollectionCommandDomainService(
        ICollectionsAggregatorService aggregatorService)
        => _aggregatorService = aggregatorService;

    public async Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(
        ICollectionItrEntity entity, CancellationToken cancellationToken)
        => await _aggregatorService.CreateCollectionAsync(entity, cancellationToken)
            .ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> DeleteCollectionAsync(
        IDeleteCollectionItrEntity entity, CancellationToken cancellationToken)
        => await _aggregatorService.DeleteCollectionAsync(entity, cancellationToken)
            .ConfigureAwait(false);

    // ... all methods follow same direct delegation
}
```

**Key characteristics:**
- Single dependency: the aggregator service
- Every method is `await _aggregatorService.{Method}(...).ConfigureAwait(false)`
- No specialized operation files in `Commands/`

### Pattern B: Specialized Operations

The router injects **multiple specialized single-method services** and delegates each method to the corresponding service's `Execute()`.

**Use when**: Commands need individual service isolation (separate aggregator dependencies per behavior, or multiple behaviors from different aggregators).

**Router:**

```csharp
internal sealed class UserCardsCommandDomainService : IUserCardsCommandDomainService
{
    private readonly IAddUserCardDomainService _addUserCard;
    private readonly IAddUserCardOnlyDomainService _addUserCardOnly;

    public UserCardsCommandDomainService(ILogger logger) : this(
        new AddUserCardDomainService(logger),
        new AddUserCardOnlyDomainService(logger))
    { }

    private UserCardsCommandDomainService(
        IAddUserCardDomainService addUserCard,
        IAddUserCardOnlyDomainService addUserCardOnly)
    {
        _addUserCard = addUserCard;
        _addUserCardOnly = addUserCardOnly;
    }

    public async Task<IOperationResponse<IUserCardOufEntity>> AddUserCardAsync(
        IUserCardItrEntity entity, CancellationToken cancellationToken)
        => await _addUserCard.Execute(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IUserCardOufEntity>> AddUserCardOnlyAsync(
        IUserCardItrEntity entity, CancellationToken cancellationToken)
        => await _addUserCardOnly.Execute(entity, cancellationToken).ConfigureAwait(false);
}
```

**Specialized operation service:**

```csharp
internal interface IAddUserCardDomainService
{
    Task<IOperationResponse<IUserCardOufEntity>> Execute(
        IUserCardItrEntity input,
        CancellationToken cancellationToken);
}

internal sealed class AddUserCardDomainService : IAddUserCardDomainService
{
    private readonly IUserCardsAggregatorService _aggregatorService;

    public AddUserCardDomainService(ILogger logger)
        : this(new UserCardsAggregatorService(logger))
    { }

    private AddUserCardDomainService(
        IUserCardsAggregatorService aggregatorService)
        => _aggregatorService = aggregatorService;

    public async Task<IOperationResponse<IUserCardOufEntity>> Execute(
        IUserCardItrEntity input, CancellationToken cancellationToken)
        => await _aggregatorService.AddUserCardAsync(input, cancellationToken)
            .ConfigureAwait(false);
}
```

**Key characteristics:**
- Router depends on multiple specialized services (1 per behavior)
- Each specialized service has a single `Execute()` method
- Each specialized service has its own interface (`I{Behavior}DomainService`)
- Specialized services inject the aggregator directly
- Router delegates via `_service.Execute(input, cancellationToken)`

## Common Rules

Regardless of pattern:

1. **Constructor Chain**: Public `ILogger` → private dependencies
2. **Always Logic**: Any logic that must always be applied; is applied here. (no instances of yet)
3. **ConfigureAwait(false)**: All async calls
4. **ItrEntity in, IOperationResponse out**: No intermediate types
5. **No exceptions**: Aggregator failures pass through as `IOperationResponse`

## Reference Implementations

- **Direct passthrough**: `Lib.Domain.Collections/Commands/CollectionCommandDomainService.cs`
- **Specialized operations**: `Lib.Domain.UserCards/Commands/`
