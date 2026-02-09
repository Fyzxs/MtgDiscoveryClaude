---
paths:
  - "csharp/src/Lib.Domains/**/Queries/*"
---

# Domain Query Services

## Router Class: `{Domain}QueryDomainService`

The `{Domain}QueryDomainService` implements the `I{Domain}QueryDomainService` interface defined in `Apis/`. It is always `internal sealed`.

### Naming

- Class: `{Domain}QueryDomainService` (internal)
- Implements: `I{Domain}QueryDomainService` (public interface from `Apis/`)

## Two Delegation Patterns

Domain query routers use one of two patterns depending on complexity.

### Pattern A: Direct Passthrough

The router injects a **single aggregator service** and delegates every method call directly to the corresponding aggregator method. No specialized operation services exist.

**Use when**: Each query is a simple 1:1 delegation to an aggregator method.

```csharp
internal sealed class CollectionQueryDomainService : ICollectionQueryDomainService
{
    private readonly ICollectionsAggregatorService _aggregatorService;

    public CollectionQueryDomainService(ILogger logger)
        : this(new CollectionsAggregatorService(logger))
    { }

    private CollectionQueryDomainService(
        ICollectionsAggregatorService aggregatorService)
        => _aggregatorService = aggregatorService;

    public async Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(
        IOwnerIdItrEntity args, CancellationToken cancellationToken)
        => await _aggregatorService.GetDefaultCollectionAsync(args, cancellationToken)
            .ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(
        IOwnerIdItrEntity args, CancellationToken cancellationToken)
        => await _aggregatorService.GetCollectionsByOwnerAsync(args, cancellationToken)
            .ConfigureAwait(false);

    // ... all methods follow same direct delegation
}
```

**Key characteristics:**
- Single dependency: the aggregator service
- Every method is `await _aggregatorService.{Method}(...).ConfigureAwait(false)`
- No specialized operation files in `Queries/`

### Pattern B: Specialized Operations

The router injects **multiple specialized single-method services** and delegates each method to the corresponding service's `Execute()`.

**Use when**: Queries need individual service isolation (separate aggregator dependencies per behavior, or multiple behaviors from different aggregators).

**Router:**

```csharp
internal sealed class ArtistsQueryDomainService : IArtistsQueryDomainService
{
    private readonly IArtistSearchDomain _artistSearch;
    private readonly ICardsByArtistDomain _cardsByArtist;
    private readonly ICardsByArtistNameDomain _cardsByArtistName;

    public ArtistsQueryDomainService(ILogger logger) : this(
        new ArtistSearchDomain(logger),
        new CardsByArtistDomain(logger),
        new CardsByArtistNameDomain(logger))
    { }

    private ArtistsQueryDomainService(
        IArtistSearchDomain artistSearch,
        ICardsByArtistDomain cardsByArtist,
        ICardsByArtistNameDomain cardsByArtistName)
    {
        _artistSearch = artistSearch;
        _cardsByArtist = cardsByArtist;
        _cardsByArtistName = cardsByArtistName;
    }

    public async Task<IOperationResponse<IArtistSearchResultCollectionOufEntity>> ArtistSearchAsync(
        IArtistSearchTermItrEntity searchTerm, CancellationToken cancellationToken)
        => await _artistSearch.Execute(searchTerm, cancellationToken)
            .ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistAsync(
        IArtistIdItrEntity artistId, CancellationToken cancellationToken)
        => await _cardsByArtist.Execute(artistId, cancellationToken)
            .ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistNameAsync(
        IArtistNameItrEntity artistName, CancellationToken cancellationToken)
        => await _cardsByArtistName.Execute(artistName, cancellationToken)
            .ConfigureAwait(false);
}
```

**Specialized operation:**

```csharp
internal interface IArtistSearchDomain
    : IOperationResponseService<IArtistSearchTermItrEntity, IArtistSearchResultCollectionOufEntity>;

internal sealed class ArtistSearchDomain : IArtistSearchDomain
{
    private readonly IArtistAggregatorService _artistAggregatorService;

    public ArtistSearchDomain(ILogger logger)
        : this(new ArtistAggregatorService(logger))
    { }

    private ArtistSearchDomain(
        IArtistAggregatorService artistAggregatorService)
        => _artistAggregatorService = artistAggregatorService;

    public async Task<IOperationResponse<IArtistSearchResultCollectionOufEntity>> Execute(
        IArtistSearchTermItrEntity input, CancellationToken cancellationToken)
        => await _artistAggregatorService.ArtistSearchAsync(input, cancellationToken)
            .ConfigureAwait(false);
}
```

**Key characteristics:**
- Router depends on multiple specialized operations (1 per behavior)
- Each specialized operation inherits `IOperationResponseService<TInput, TOutput>` — never define `Execute` manually
- Naming: `I{Behavior}Domain` / `{Behavior}Domain` (`Domain` suffix, not `DomainService`)
- Specialized operations inject the aggregator directly
- Router delegates via `_operation.Execute(input, cancellationToken)`

## Common Rules

Regardless of pattern:

1. **Constructor Chain**: Public `ILogger` → private dependencies
2. **Always Logic**: Any logic that must always be applied; is applied here. (no instances of yet)
3. **ConfigureAwait(false)**: All async calls
4. **ItrEntity in, IOperationResponse out**: No intermediate types
5. **No exceptions**: Aggregator failures pass through as `IOperationResponse`

## Reference Implementations

- **Direct passthrough**: `Lib.Domain.Collections/Queries/CollectionQueryDomainService.cs`
- **Specialized operations**: `Lib.Domain.Artists/Queries/`
