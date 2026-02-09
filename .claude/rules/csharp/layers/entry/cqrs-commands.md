---
paths:
  - "csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Commands/**"
---

# Entry Command Services

## Router Class: `{Domain}EntryService`

The router implements the domain-specific command interface from `Apis/` and delegates each method to the appropriate operation service's `Execute()`.

### Pattern

```csharp
internal sealed class UserCardsEntryService : IUserCardsEntryService
{
    private readonly IAddCardToCollectionEntryService _addCardToCollection;
    private readonly IAddUserCardOnlyEntryService _addUserCardOnly;

    public UserCardsEntryService(ILogger logger) : this(
        new AddCardToCollectionEntryService(logger),
        new AddUserCardOnlyEntryService(logger))
    { }

    private UserCardsEntryService(
        IAddCardToCollectionEntryService addCardToCollection,
        IAddUserCardOnlyEntryService addUserCardOnly)
    {
        _addCardToCollection = addCardToCollection;
        _addUserCardOnly = addUserCardOnly;
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> AddCardToCollectionAsync(
        IAddCardToCollectionArgsEntity args, CancellationToken cancellationToken)
        => await _addCardToCollection.Execute(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> AddUserCardOnlyAsync(
        IAddCardToCollectionArgsEntity args, CancellationToken cancellationToken)
        => await _addUserCardOnly.Execute(args, cancellationToken).ConfigureAwait(false);
}
```

Each method delegates to the corresponding operation's `Execute(input, cancellationToken)` — no logic in the router.

## Operation Services

These are targeted classes following single responsibility — each implements a single command behavior.

### Interface Pattern

All operation interfaces MUST inherit from `IOperationResponseService<TInput, TOutput>` — never define `Execute` manually.

```csharp
internal interface IAddCardToCollectionEntryService
    : IOperationResponseService<IAddCardToCollectionArgsEntity, List<CardItemOutEntity>>;
```

### Standard Execute Flow

All command operation services follow this sequence:

1. **Validate** input via injected validator container
2. **Map** ArgEntity/ArgsEntity to ItrEntity via injected mapper
3. **Call** domain service with ItrEntity + CancellationToken
4. Check `response.IsFailure` — return `FailureOperationResponse` if failed
5. **Map** OufEntity to OutEntity via injected mapper
6. Return `SuccessOperationResponse<TOutEntity>`

### Standard Dependencies

Command operation services typically inject:

1. Domain service
2. Validator container
3. ArgToItr mapper
4. OufToOut mapper

## Combined ArgsEntity Pattern

Mutations that require authenticated user context combine `ClaimsPrincipal`-derived auth data with the GraphQL input into a single combined `IArgsEntity`:

```csharp
// Combined args interface — wraps auth user + operation-specific input
public interface IAddCardToCollectionArgsEntity
{
    IAuthUserArgEntity AuthUser { get; }
    IAddUserCardArgEntity AddUserCard { get; }
}
```

The GraphQL layer creates the combined entity via a dedicated mapper:

```csharp
internal sealed class AddCardToCollectionArgsMapper : IAddCardToCollectionArgsMapper
{
    public Task<IAddCardToCollectionArgsEntity> Map(
        ClaimsPrincipal claimsPrincipal, AddUserCardArgEntity args)
    {
        IAddCardToCollectionArgsEntity result = new AddCardToCollectionArgsEntity
        {
            AuthUser = new AuthUserArgEntity(claimsPrincipal),
            AddUserCard = args
        };
        return Task.FromResult(result);
    }
}
```

The command operation service then validates the combined args and extracts what it needs:

```csharp
// In operation service Execute():
IValidatorActionResult<...> validatorResult =
    await _validator.Validate(input).ConfigureAwait(false);
// Validator checks both AuthUser and AddUserCard properties

string userId = input.AuthUser.UserId;
IItrEntity itrEntity = await _mapper.Map(input).ConfigureAwait(false);
```

## Cross-Domain Coordination

Some commands coordinate across multiple domain services (e.g., fetching card details before updating user cards). When a command needs data from another domain:

1. Call the secondary domain service
2. Extract needed metadata
3. Map to an enriched ItrEntity via a dedicated mapper
4. Call the primary domain service with the enriched entity

All mapping to ItrEntities MUST go through dedicated mapper classes — no inline entity construction.

## Collection Command Service Pattern

For domains with many command operations, a dedicated command service class handles all operations with their validators and mappers:

```csharp
internal sealed class CollectionEntryCommandService : ICollectionEntryCommandService
{
    private readonly ICollectionsDomainService _domainService;
    private readonly ICreateCollectionArgEntityValidator _createValidator;
    private readonly ICreateCollectionArgToItrMapper _createArgToItrMapper;
    private readonly ICollectionOufToOutMapper _oufToOutMapper;
    // ... validators and mappers for each operation

    // Each method follows: validate → map → domain call → map → return
    public async Task<IOperationResponse<CollectionOutEntity>> CreateCollectionAsync(
        ICreateCollectionArgsEntity argsEntity, CancellationToken cancellationToken)
    {
        IValidatorActionResult<...> validatorResult =
            await _createValidator.Validate(argsEntity.CreateCollection).ConfigureAwait(false);
        if (validatorResult.IsNotValid())
            return new FailureOperationResponse<CollectionOutEntity>(
                validatorResult.FailureStatus().OuterException);

        ICollectionItrEntity itrEntity =
            await _createArgToItrMapper.Map(argsEntity.CreateCollection, userId).ConfigureAwait(false);

        IOperationResponse<ICollectionOufEntity> opResponse =
            await _domainService.CreateCollectionAsync(itrEntity, cancellationToken).ConfigureAwait(false);
        if (opResponse.IsFailure)
            return new FailureOperationResponse<CollectionOutEntity>(opResponse.OuterException);

        CollectionOutEntity outEntity =
            await _oufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<CollectionOutEntity>(outEntity);
    }
}
```

## Common Rules

1. **Constructor Chain**: Public `ILogger` → private dependencies
2. **ConfigureAwait(false)**: All async calls
3. **ArgsEntity in, IOperationResponse<OutEntity> out**: Validate → Map → Domain → Map → Return
4. **No exceptions**: Domain failures pass through as `IOperationResponse`
5. **All operation interfaces inherit IOperationResponseService**: Never manually define `Execute`
6. **All ItrEntity creation via mappers**: No inline entity construction in service methods

## Reference Implementations

- **Router**: `Commands/UserCardsEntryService.cs`
- **Operation service**: `Commands/UserCards/AddCardToCollectionEntryService.cs`
- **Collection commands**: `Commands/Collections/CollectionEntryCommandService.cs`
- **Default resource creation**: `Commands/Collections/DefaultCollectionCreator.cs`
