using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Aggregator.UserCards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserCards.Queries.UserCardsByArtist;

internal sealed class UserCardsByArtistAggregatorService : IUserCardsByArtistAggregatorService
{
    private readonly IUserCardsAdapterService _userCardsAdapterService;
    private readonly ICollectionUserCardExtToItrMapper _collectionMapper;
    private readonly IUserCardsArtistItrToXfrMapper _userCardsArtistItrToXfrMapper;

    public UserCardsByArtistAggregatorService(ILogger logger) : this(
        new UserCardsAdapterService(logger),
        new CollectionUserCardExtToItrMapper(),
        new UserCardsArtistItrToXfrMapper())
    { }

    private UserCardsByArtistAggregatorService(
        IUserCardsAdapterService userCardsAdapterService,
        ICollectionUserCardExtToItrMapper collectionMapper,
        IUserCardsArtistItrToXfrMapper userCardsArtistItrToXfrMapper)
    {
        _userCardsAdapterService = userCardsAdapterService;
        _collectionMapper = collectionMapper;
        _userCardsArtistItrToXfrMapper = userCardsArtistItrToXfrMapper;
    }

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> Execute(IUserCardsArtistItrEntity input)
    {
        IUserCardsArtistXfrEntity xfrEntity = await _userCardsArtistItrToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<UserCardExtEntity>> response = await _userCardsAdapterService.UserCardsByArtistAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<IUserCardOufEntity>>(response.OuterException);
        }

        IEnumerable<IUserCardOufEntity> mappedUserCards = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IEnumerable<IUserCardOufEntity>>(mappedUserCards);
    }

}
