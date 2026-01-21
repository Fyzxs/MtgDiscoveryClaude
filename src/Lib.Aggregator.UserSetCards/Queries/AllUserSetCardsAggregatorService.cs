using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSetCards.Apis;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Aggregator.UserSetCards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSetCards.Queries;

internal sealed class AllUserSetCardsAggregatorService : IAllUserSetCardsAggregatorService
{
    private readonly IAllUserSetCardsItrToXfrMapper _itrToXfrMapper;
    private readonly IUserSetCardsAdapterService _adapterService;
    private readonly ICollectionUserSetCardExtToOufMapper _extToOufMapper;

    public AllUserSetCardsAggregatorService(ILogger logger) : this(
        new AllUserSetCardsItrToXfrMapper(),
        new UserSetCardsAdapterService(logger),
        new CollectionUserSetCardExtToOufMapper())
    {
    }

    private AllUserSetCardsAggregatorService(
        IAllUserSetCardsItrToXfrMapper itrToXfrMapper,
        IUserSetCardsAdapterService adapterService,
        ICollectionUserSetCardExtToOufMapper extToOufMapper)
    {
        _itrToXfrMapper = itrToXfrMapper;
        _adapterService = adapterService;
        _extToOufMapper = extToOufMapper;
    }

    public async Task<IOperationResponse<IEnumerable<IUserSetCardOufEntity>>> Execute(
        IAllUserSetCardsItrEntity userSetCards)
    {
        IAllUserSetCardsXfrEntity xfrEntity =
            await _itrToXfrMapper.Map(userSetCards).ConfigureAwait(false);

        IOperationResponse<IEnumerable<UserSetCardExtEntity>> response =
            await _adapterService.GetAllUserSetCardsAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<IUserSetCardOufEntity>>(
                response.OuterException);
        }

        IEnumerable<IUserSetCardOufEntity> mappedUserSetCards =
            await _extToOufMapper.Map(response.ResponseData).ConfigureAwait(false);

        return new SuccessOperationResponse<IEnumerable<IUserSetCardOufEntity>>(mappedUserSetCards);
    }
}
