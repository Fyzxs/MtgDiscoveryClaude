---
paths:
  - "csharp/src/**/Enrichments/**"
---

# Enrichment Pattern

## Purpose

Enrichments **add supplementary data to query results** after the primary query completes. They enrich OutEntities in-place with data from secondary domain services (e.g., adding user collection ownership data to card query results).

## Key Characteristics

- Run **after** OufToOut mapping, on OutEntity lists
- **Fail silently** — enrichment failure does not fail the parent query
- **Mutate in-place** — modify target list items directly
- **Check preconditions** — skip enrichment when context data is absent
- Use **Integrators** to merge enrichment data into the target list

## Naming Convention

| Type | Pattern | Example |
|------|---------|---------|
| Composite interface | `I{Concern}Enrichment` | `IUserCardEnrichment` |
| Composite implementation | `{Concern}Enrichment` | `UserCardEnrichment` |
| Specialized interface | `I{Concern}By{Strategy}Enrichment` | `IUserCardByIdsEnrichment` |
| Specialized implementation | `{Concern}By{Strategy}Enrichment` | `UserCardByIdsEnrichment` |

## Specialized Enrichment

Each specialized enrichment handles a single data fetch strategy:

### Interface

```csharp
internal interface IUserCardByIdsEnrichment
{
    Task Enrich(List<CardItemOutEntity> target, IUserIdArgEntity context,
        CancellationToken cancellationToken);
}
```

**Key points:**
- Return type is `Task` (void) — enrichment modifies `target` in-place
- Parameters: target list to enrich, context with lookup keys, CancellationToken

### Implementation

```csharp
internal sealed class UserCardByIdsEnrichment : IUserCardByIdsEnrichment
{
    private readonly IUserCardsDomainService _userCardsDomainService;
    private readonly ICollectionCardItemToByIdsItrMapper _collectionCardItemToByIdsItrMapper;
    private readonly IUserCardCollectionIntegrator _integrator;

    public UserCardByIdsEnrichment(ILogger logger) : this(
        new UserCardsDomainService(logger),
        new CollectionCardItemToByIdsItrMapper(),
        new UserCardCollectionIntegrator())
    { }

    private UserCardByIdsEnrichment(
        IUserCardsDomainService userCardsDomainService,
        ICollectionCardItemToByIdsItrMapper collectionCardItemToByIdsItrMapper,
        IUserCardCollectionIntegrator integrator)
    {
        _userCardsDomainService = userCardsDomainService;
        _collectionCardItemToByIdsItrMapper = collectionCardItemToByIdsItrMapper;
        _integrator = integrator;
    }

    public async Task Enrich(List<CardItemOutEntity> target, IUserIdArgEntity args,
        CancellationToken cancellationToken)
    {
        if (args.DoesNotHaveUserId)
            return;

        IUserCardsByIdsItrEntity itrEntity =
            await _collectionCardItemToByIdsItrMapper.Map(target, args).ConfigureAwait(false);

        IOperationResponse<IEnumerable<IUserCardOufEntity>> response =
            await _userCardsDomainService.UserCardsByIdsAsync(itrEntity, cancellationToken)
                .ConfigureAwait(false);
        if (response.IsFailure)
            return;

        _ = await _integrator.Integrate(target, response.ResponseData).ConfigureAwait(false);
    }
}
```

### Execute Flow

1. **Check preconditions** — return early if context doesn't have required data
2. **Map** target list + context to ItrEntity for secondary domain query
3. **Call** secondary domain service
4. **Check failure** — return silently on failure (do not propagate error)
5. **Integrate** enrichment data into target list via Integrator

### Standard Dependencies (3)

1. Domain service (secondary data source)
2. Mapper (target + context → ItrEntity)
3. Integrator (merge enrichment data into target)

## Composite Enrichment

When a concern has multiple strategies, a composite delegates to specialized enrichments:

```csharp
internal interface IUserCardEnrichment
{
    Task Enrich(List<CardItemOutEntity> outEntities, IUserIdArgEntity args,
        CancellationToken cancellationToken);
    Task EnrichBySet(List<CardItemOutEntity> outEntities, IUserCardsSetItrEntity context,
        CancellationToken cancellationToken);
    Task EnrichByArtist(List<CardItemOutEntity> outEntities, IUserCardsArtistItrEntity context,
        CancellationToken cancellationToken);
    Task EnrichByName(List<CardItemOutEntity> outEntities, IUserCardsNameItrEntity context,
        CancellationToken cancellationToken);
}

internal sealed class UserCardEnrichment : IUserCardEnrichment
{
    private readonly IUserCardByIdsEnrichment _byIdsEnrichment;
    private readonly IUserCardBySetEnrichment _bySetEnrichment;
    private readonly IUserCardByArtistEnrichment _byArtistEnrichment;
    private readonly IUserCardByNameEnrichment _byNameEnrichment;

    public UserCardEnrichment(ILogger logger) : this(
        new UserCardByIdsEnrichment(logger),
        new UserCardBySetEnrichment(logger),
        new UserCardByArtistEnrichment(logger),
        new UserCardByNameEnrichment(logger))
    { }

    // Each method delegates to the corresponding specialized enrichment
    public async Task Enrich(List<CardItemOutEntity> outEntities, IUserIdArgEntity args,
        CancellationToken cancellationToken)
        => await _byIdsEnrichment.Enrich(outEntities, args, cancellationToken)
            .ConfigureAwait(false);
}
```

## Entry-Layer Integrator

Enrichments use **Entry-layer Integrators** to merge data. These are distinct from Adapter-layer Integrators:

| | Entry Integrator | Adapter Integrator |
|---|---|---|
| Purpose | Merge query enrichment data into OutEntities | Merge delta into state for persistence |
| Input | `List<OutEntity>` + `IEnumerable<OufEntity>` | `ExtEntity` + `XfrEntity` |
| Output | Modified `List<OutEntity>` | Modified `ExtEntity` |
| Location | `Queries/Actions/Integrators/` | `{Adapter}/Commands/Integrators/` |

```csharp
internal interface IUserCardCollectionIntegrator
    : IIntegrator<List<CardItemOutEntity>, IEnumerable<IUserCardOufEntity>>;

internal sealed class UserCardCollectionIntegrator : IUserCardCollectionIntegrator
{
    private readonly ICollectionUserCardDetailsOufToOutMapper _cardDetailsOufToOutMapper;

    public UserCardCollectionIntegrator()
        : this(new CollectionUserCardDetailsOufToOutMapper()) { }

    private UserCardCollectionIntegrator(
        ICollectionUserCardDetailsOufToOutMapper cardDetailsOufToOutMapper)
        => _cardDetailsOufToOutMapper = cardDetailsOufToOutMapper;

    public async Task<List<CardItemOutEntity>> Integrate(
        List<CardItemOutEntity> current, IEnumerable<IUserCardOufEntity> change)
    {
        Dictionary<string, Task<ICollection<CollectedItemOutEntity>>> dictionary =
            change.ToDictionary(
                uc => uc.CardId,
                uc => _cardDetailsOufToOutMapper.Map(uc.CollectedList));

        foreach (CardItemOutEntity card in current)
        {
            if (dictionary.TryGetValue(card.Id,
                out Task<ICollection<CollectedItemOutEntity>> collectedItems) is false)
                continue;
            card.UserCollection = await collectedItems.ConfigureAwait(false);
        }

        return current;
    }
}
```

## Location

All enrichment files live in `Queries/Actions/Enrichments/`.

## Existing Implementations

| Enrichment | Enriches | With |
|------------|----------|------|
| `UserCardByIdsEnrichment` | Card list | User collection data (by card IDs) |
| `UserCardBySetEnrichment` | Card list | User collection data (by set) |
| `UserCardByArtistEnrichment` | Card list | User collection data (by artist) |
| `UserCardByNameEnrichment` | Card list | User collection data (by name) |
| `UserWishlistCardByIdsEnrichment` | Card list | User wishlist data |
| `UserSetEnrichment` | Set list | User set tracking data |
| `UserSealedProductEnrichment` | Product list | User sealed product data |

## Related Patterns

- **Integrator** (Entry): Merges enrichment data into target — this file
- **Integrator** (Adapter): Merges delta into state — see `actions/integrators.md`
- **Mapper**: Transforms entities at boundaries — see `actions/mappers.md`
