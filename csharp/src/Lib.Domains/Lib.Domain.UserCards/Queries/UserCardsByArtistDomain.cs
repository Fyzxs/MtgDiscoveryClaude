using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserCards.Queries;

/// <summary>
/// Single-method service for retrieving all user cards for a specific artist.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class UserCardsByArtistDomain : IUserCardsByArtistDomain
{
    private readonly IUserCardsQueryAggregatorService _userCardsAggregatorService;

    public UserCardsByArtistDomain(ILogger logger) : this(new UserCardsAggregatorService(logger))
    { }

    private UserCardsByArtistDomain(IUserCardsQueryAggregatorService userCardsAggregatorService) => _userCardsAggregatorService = userCardsAggregatorService;

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> Execute(
        IUserCardsArtistItrEntity input,
        CancellationToken cancellationToken) => await _userCardsAggregatorService.UserCardsByArtistAsync(input, cancellationToken).ConfigureAwait(false);
}
