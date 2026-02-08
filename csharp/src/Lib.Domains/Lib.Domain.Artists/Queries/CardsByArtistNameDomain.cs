using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.Artists.Apis;
using Lib.Shared.DataModels.Entities.Itrs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Artists.Queries;

/// <summary>
/// Single-method service for retrieving cards by artist name.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class CardsByArtistNameDomain : ICardsByArtistNameDomain
{
    private readonly IArtistAggregatorService _artistAggregatorService;

    public CardsByArtistNameDomain(ILogger logger) : this(new ArtistAggregatorService(logger))
    { }

    private CardsByArtistNameDomain(IArtistAggregatorService artistAggregatorService) => _artistAggregatorService = artistAggregatorService;

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(
        IArtistNameItrEntity input,
        CancellationToken cancellationToken) => await _artistAggregatorService.CardsByArtistNameAsync(input, cancellationToken).ConfigureAwait(false);
}
