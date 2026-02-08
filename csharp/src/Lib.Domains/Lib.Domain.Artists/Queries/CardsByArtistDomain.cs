using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.Artists.Apis;
using Lib.Shared.DataModels.Entities.Itrs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Artists.Queries;

/// <summary>
/// Single-method service for retrieving cards by artist ID.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class CardsByArtistDomain : ICardsByArtistDomain
{
    private readonly IArtistAggregatorService _artistAggregatorService;

    public CardsByArtistDomain(ILogger logger) : this(new ArtistAggregatorService(logger))
    { }

    private CardsByArtistDomain(IArtistAggregatorService artistAggregatorService) => _artistAggregatorService = artistAggregatorService;

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(
        IArtistIdItrEntity input,
        CancellationToken cancellationToken) => await _artistAggregatorService.CardsByArtistAsync(input, cancellationToken).ConfigureAwait(false);
}
