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

```csharp
internal sealed class CollectionCommandAggregator : ICollectionCommandAggregatorService
{
    private readonly ICreateCollectionAggregator _createCollection;
    private readonly IDeleteCollectionAggregator _deleteCollection;

    public CollectionCommandAggregator(ILogger logger) : this(
        new CreateCollectionAggregator(logger),
        new DeleteCollectionAggregator(logger))
    { }

    private CollectionCommandAggregator(
        ICreateCollectionAggregator createCollection,
        IDeleteCollectionAggregator deleteCollection)
    {
        _createCollection = createCollection;
        _deleteCollection = deleteCollection;
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(
        ICollectionItrEntity entity, CancellationToken cancellationToken)
        => await _createCollection.Execute(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> DeleteCollectionAsync(
        ICollectionIdItrEntity entity, CancellationToken cancellationToken)
        => await _deleteCollection.Execute(entity, cancellationToken).ConfigureAwait(false);
}
```

Each method delegates to the corresponding behavior's `Execute(input, cancellationToken)` — no logic in the router.

## Behavior Classes

These are targeted classes following single responsibility — each implements a single command behavior.

### Naming

The class and interfaces use `{Behavior}Aggregator` with the `I` prefix for interfaces. The `Aggregator` suffix (not `AggregatorService`) signals internal scope.

### Interface Pattern

All interfaces must inherit from `IOperationResponseService<TInput, TOutput>` — never define `Execute` manually.

```csharp
internal interface IAddUserWishlistCardAggregator
    : IOperationResponseService<IUserWishlistCardItrEntity, IUserWishlistCardOufEntity>;
```

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

```csharp
public async Task<IOperationResponse<IUserCardOufEntity>> Execute(
    IUserCardItrEntity input, CancellationToken cancellationToken)
{
    // Step 1: Map input
    IAddUserCardXfrEntity xfrEntity = await _addUserCardItrToXfrMapper
        .Map(input).ConfigureAwait(false);

    // Step 2: Primary adapter call
    IOperationResponse<UserCardExtEntity> response = await _userCardsAdapterService
        .AddUserCardAsync(xfrEntity, cancellationToken).ConfigureAwait(false);

    if (response.IsFailure)
    {
        return new FailureOperationResponse<IUserCardOufEntity>(response.OuterException);
    }

    // Step 3: Secondary adapter call (cross-adapter coordination)
    IAddCardToSetXfrEntity setCardEntity = await _addCardToSetMapper
        .Map(xfrEntity, response.ResponseData.CollectedList).ConfigureAwait(false);
    IOperationResponse<UserSetCardExtEntity> setCardResponse = await _userSetCardsAdapterService
        .AddCardToSetAsync(setCardEntity, cancellationToken).ConfigureAwait(false);

    // Step 4: Rollback on secondary failure
    if (setCardResponse.IsFailure)
    {
        IAddUserCardXfrEntity rollbackEntity = await _rollbackMapper
            .Map(xfrEntity).ConfigureAwait(false);
        await _userCardsAdapterService
            .AddUserCardAsync(rollbackEntity, cancellationToken).ConfigureAwait(false);

        return new FailureOperationResponse<IUserCardOufEntity>(setCardResponse.OuterException);
    }

    // Step 5: Map and return success
    IUserCardOufEntity mappedUserCard = await _userCardMapper
        .Map(response.ResponseData).ConfigureAwait(false);
    return new SuccessOperationResponse<IUserCardOufEntity>(mappedUserCard);
}
```

### When to Use

- The command must coordinate writes across 2+ adapters
- Partial failure of secondary operations requires compensating the primary
- Dependencies will exceed the standard 3 (typically 5-6: multiple adapter services + mappers + rollback mapper)

### Reference

`Lib.Aggregator.UserCards/Commands/AddUserCardAggregatorService.cs` demonstrates this pattern.
