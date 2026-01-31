using System.Collections.Generic;
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

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardAsync(IUserCardItrEntity userCard) => await _userCardOperations.Execute(userCard);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet) => await _userCardsBySetOperations.Execute(userCardsSet);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByIdsAsync(IUserCardsByIdsItrEntity userCards) => await _userCardsByIdsOperations.Execute(userCards);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByArtistAsync(IUserCardsArtistItrEntity userCardsArtist) => await _userCardsByArtistOperations.Execute(userCardsArtist);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByNameAsync(IUserCardsNameItrEntity userCardsName) => await _userCardsByNameOperations.Execute(userCardsName);

    public async Task<IOperationResponse<ISigningResultOufEntity>> UserCardsForSigningAsync(IUserCardsForSigningItrEntity userCardsForSigning) => await _userCardsForSigningOperations.Execute(userCardsForSigning);
}
