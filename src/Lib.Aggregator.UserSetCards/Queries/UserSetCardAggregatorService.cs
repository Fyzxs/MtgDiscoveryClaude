using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSetCards.Apis;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Aggregator.UserSetCards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSetCards.Queries;

internal sealed class UserSetCardAggregatorService : IUserSetCardAggregatorService
{
    private readonly IUserSetCardsAdapterService _userSetCardsAdapterService;
    private readonly IUserSetCardItrToXfrMapper _itrToXfrMapper;
    private readonly IUserSetCardExtToItrMapper _extToItrMapper;

    public UserSetCardAggregatorService(ILogger logger) : this(
        new UserSetCardsAdapterService(logger),
        new UserSetCardItrToXfrMapper(),
        new UserSetCardExtToItrMapper())
    { }

    private UserSetCardAggregatorService(
        IUserSetCardsAdapterService userSetCardsAdapterService,
        IUserSetCardItrToXfrMapper itrToXfrMapper,
        IUserSetCardExtToItrMapper extToItrMapper)
    {
        _userSetCardsAdapterService = userSetCardsAdapterService;
        _itrToXfrMapper = itrToXfrMapper;
        _extToItrMapper = extToItrMapper;
    }

    public async Task<IOperationResponse<IUserSetCardOufEntity>> Execute(IUserSetCardItrEntity input)
    {
        IUserSetCardGetXfrEntity xfrEntity = await _itrToXfrMapper.Map(input).ConfigureAwait(false);

        IOperationResponse<UserSetCardExtEntity> response = await _userSetCardsAdapterService.GetUserSetCardAsync(xfrEntity).ConfigureAwait(false);

        if (response is not SuccessOperationResponse<UserSetCardExtEntity> successResponse)
        {
            return new FailureOperationResponse<IUserSetCardOufEntity>(response.OuterException);
        }

        IUserSetCardOufEntity itrEntity = await _extToItrMapper.Map(successResponse.ResponseData).ConfigureAwait(false);

        return new SuccessOperationResponse<IUserSetCardOufEntity>(itrEntity);
    }
}
