using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserCards.Queries;

/// <summary>
/// Single-method service for retrieving all user cards for a specific artist.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class UserCardsByArtistDomainService : IUserCardsByArtistDomainService
{
    private readonly IUserCardsQueryAggregatorService _userCardsAggregatorService;

    public UserCardsByArtistDomainService(ILogger logger) : this(new UserCardsAggregatorService(logger))
    { }

    private UserCardsByArtistDomainService(IUserCardsQueryAggregatorService userCardsAggregatorService) => _userCardsAggregatorService = userCardsAggregatorService;

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> Execute(IUserCardsArtistItrEntity input) => await _userCardsAggregatorService.UserCardsByArtistAsync(input).ConfigureAwait(false);
}
