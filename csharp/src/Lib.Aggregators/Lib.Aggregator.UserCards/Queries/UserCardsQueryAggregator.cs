using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Aggregator.UserCards.Queries.UserCardsForSigning;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserCards.Queries;

internal sealed class UserCardsQueryAggregator : IUserCardsQueryAggregatorService
{
    private readonly IUserCardAggregatorService _userCardOperations;
    private readonly IUserCardsBySetAggregatorService _userCardsBySetOperations;
    private readonly IUserCardsByIdsAggregatorService _userCardsByIdsOperations;
    private readonly IUserCardsByArtistAggregatorService _userCardsByArtistOperations;
    private readonly IUserCardsByNameAggregatorService _userCardsByNameOperations;
    private readonly IUserCardsForSigningAggregatorService _userCardsForSigningOperations;

    public UserCardsQueryAggregator(ILogger logger) : this(
        new UserCardAggregatorService(logger),
        new UserCardsBySetAggregatorService(logger),
        new UserCardsByIdsAggregatorService(logger),
        new UserCardsByArtistAggregatorService(logger),
        new UserCardsByNameAggregatorService(logger),
        new UserCardsForSigningAggregatorService(logger))
    { }

    private UserCardsQueryAggregator(
        IUserCardAggregatorService userCardOperations,
        IUserCardsBySetAggregatorService userCardsBySetOperations,
        IUserCardsByIdsAggregatorService userCardsByIdsOperations,
        IUserCardsByArtistAggregatorService userCardsByArtistOperations,
        IUserCardsByNameAggregatorService userCardsByNameOperations,
        IUserCardsForSigningAggregatorService userCardsForSigningOperations)
    {
        _userCardOperations = userCardOperations;
        _userCardsBySetOperations = userCardsBySetOperations;
        _userCardsByIdsOperations = userCardsByIdsOperations;
        _userCardsByArtistOperations = userCardsByArtistOperations;
        _userCardsByNameOperations = userCardsByNameOperations;
        _userCardsForSigningOperations = userCardsForSigningOperations;
    }

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardAsync(
        IUserCardItrEntity userCard,
        CancellationToken cancellationToken)
        => await _userCardOperations.Execute(userCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(
        IUserCardsSetItrEntity userCardsSet,
        CancellationToken cancellationToken)
        => await _userCardsBySetOperations.Execute(userCardsSet, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByIdsAsync(
        IUserCardsByIdsItrEntity userCards,
        CancellationToken cancellationToken)
        => await _userCardsByIdsOperations.Execute(userCards, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByArtistAsync(
        IUserCardsArtistItrEntity userCardsArtist,
        CancellationToken cancellationToken)
        => await _userCardsByArtistOperations.Execute(userCardsArtist, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByNameAsync(
        IUserCardsNameItrEntity userCardsName,
        CancellationToken cancellationToken)
        => await _userCardsByNameOperations.Execute(userCardsName, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ISigningResultOufEntity>> UserCardsForSigningAsync(
        IUserCardsForSigningItrEntity userCardsForSigning,
        CancellationToken cancellationToken)
        => await _userCardsForSigningOperations.Execute(userCardsForSigning, cancellationToken).ConfigureAwait(false);
}
