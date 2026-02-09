---
paths:
  - "csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Queries/**"
---

# Entry Query Services

## Router Class: `{Domain}EntryService`

The router implements the domain-specific query interface from `Apis/` and delegates each method to the appropriate operation service's `Execute()`.

### Pattern

```csharp
internal sealed class CardEntryService : ICardEntryService
{
    private readonly ICardsByIdsEntryService _cardsByIds;
    private readonly ICardsBySetCodeEntryService _cardsBySetCode;
    private readonly ICardsByNameEntryService _cardsByName;
    private readonly ICardNameSearchEntryService _cardNameSearch;

    public CardEntryService(ILogger logger) : this(
        new CardsByIdsEntryService(logger),
        new CardsBySetCodeEntryService(logger),
        new CardsByNameEntryService(logger),
        new CardNameSearchEntryService(logger))
    { }

    private CardEntryService(
        ICardsByIdsEntryService cardsByIds,
        ICardsBySetCodeEntryService cardsBySetCode,
        ICardsByNameEntryService cardsByName,
        ICardNameSearchEntryService cardNameSearch)
    {
        _cardsByIds = cardsByIds;
        _cardsBySetCode = cardsBySetCode;
        _cardsByName = cardsByName;
        _cardNameSearch = cardNameSearch;
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByIdsAsync(
        ICardIdsArgEntity args, CancellationToken cancellationToken)
        => await _cardsByIds.Execute(args, cancellationToken).ConfigureAwait(false);

    // Each method delegates to the corresponding operation's Execute()
}
```

Each method delegates to the corresponding operation's `Execute(input, cancellationToken)` — no logic in the router.

## Operation Services

These are targeted classes following single responsibility — each implements a single query behavior.

### Interface Pattern

All operation interfaces MUST inherit from `IOperationResponseService<TInput, TOutput>` — never define `Execute` manually.

```csharp
internal interface ICardsByIdsEntryService
    : IOperationResponseService<ICardIdsArgEntity, List<CardItemOutEntity>>;
```

### Standard Execute Flow

All query operation services follow this sequence:

1. **Validate** input via injected validator container
2. **Map** ArgEntity to ItrEntity via injected mapper
3. **Call** domain service with ItrEntity + CancellationToken
4. Check `response.IsFailure` — return `FailureOperationResponse` if failed
5. **Map** OufEntity to OutEntity via injected mapper
6. Optionally **enrich** OutEntity with additional data
7. Return `SuccessOperationResponse<TOutEntity>`

### Implementation

```csharp
internal sealed class CardsByIdsEntryService : ICardsByIdsEntryService
{
    private readonly ICardDomainService _cardDomainService;
    private readonly ICardIdsArgEntityValidator _cardIdsArgEntityValidator;
    private readonly ICardIdsArgToItrMapper _cardIdsArgToItrMapper;
    private readonly ICollectionCardItemOufToOutMapper _cardItemOufToOutMapper;
    private readonly IUserCardEnrichment _userCardEnrichment;
    private readonly IUserWishlistCardByIdsEnrichment _userWishlistCardEnrichment;

    public CardsByIdsEntryService(ILogger logger) : this(
        new CardDomainService(logger),
        new CardIdsArgEntityValidatorContainer(),
        new CardIdsArgToItrMapper(),
        new CollectionCardItemOufToOutMapper(),
        new UserCardEnrichment(logger),
        new UserWishlistCardByIdsEnrichment(logger))
    { }

    private CardsByIdsEntryService(
        ICardDomainService cardDomainService,
        ICardIdsArgEntityValidator cardIdsArgEntityValidator,
        ICardIdsArgToItrMapper cardIdsArgToItrMapper,
        ICollectionCardItemOufToOutMapper cardItemOufToOutMapper,
        IUserCardEnrichment userCardEnrichment,
        IUserWishlistCardByIdsEnrichment userWishlistCardEnrichment)
    {
        _cardDomainService = cardDomainService;
        _cardIdsArgEntityValidator = cardIdsArgEntityValidator;
        _cardIdsArgToItrMapper = cardIdsArgToItrMapper;
        _cardItemOufToOutMapper = cardItemOufToOutMapper;
        _userCardEnrichment = userCardEnrichment;
        _userWishlistCardEnrichment = userWishlistCardEnrichment;
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> Execute(
        ICardIdsArgEntity args, CancellationToken cancellationToken)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionOufEntity>> validatorResult =
            await _cardIdsArgEntityValidator.Validate(args).ConfigureAwait(false);
        if (validatorResult.IsNotValid())
            return new FailureOperationResponse<List<CardItemOutEntity>>(
                validatorResult.FailureStatus().OuterException);

        ICardIdsItrEntity itrEntity = await _cardIdsArgToItrMapper.Map(args).ConfigureAwait(false);

        IOperationResponse<ICardItemCollectionOufEntity> opResponse =
            await _cardDomainService.CardsByIdsAsync(itrEntity, cancellationToken).ConfigureAwait(false);
        if (opResponse.IsFailure)
            return new FailureOperationResponse<List<CardItemOutEntity>>(opResponse.OuterException);

        List<CardItemOutEntity> outEntities =
            await _cardItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);

        await _userCardEnrichment.Enrich(outEntities, args, cancellationToken).ConfigureAwait(false);
        await _userWishlistCardEnrichment.Enrich(outEntities, args, cancellationToken).ConfigureAwait(false);

        return new SuccessOperationResponse<List<CardItemOutEntity>>(outEntities);
    }
}
```

### Standard Dependencies

Query operation services typically inject:

1. Domain service
2. Validator container
3. ArgToItr mapper
4. OufToOut mapper
5. Enrichment(s) — when user-specific data is needed

### Enrichment Step

Some queries enrich the core response with user-specific data (collection ownership, wishlist status). Enrichments:

- Run **after** the OufToOut mapping
- Are **optional** — only present when user context exists
- **Fail silently** — enrichment failure does not fail the query
- See `enrichments.md` for full pattern details

## Common Rules

1. **Constructor Chain**: Public `ILogger` → private dependencies
2. **ConfigureAwait(false)**: All async calls
3. **ArgEntity in, IOperationResponse<OutEntity> out**: Validate → Map → Domain → Map → Return
4. **No exceptions**: Domain failures pass through as `IOperationResponse`
5. **All operation interfaces inherit IOperationResponseService**: Never manually define `Execute`

## Reference Implementations

- **Router**: `Queries/CardEntryService.cs`
- **Operation with enrichment**: `Queries/Cards/CardsByIdsEntryService.cs`
- **Operation without enrichment**: `Queries/Sets/SetsByIdsEntryService.cs`
- **Collection queries**: `Queries/Collections/CollectionEntryQueryService.cs`
