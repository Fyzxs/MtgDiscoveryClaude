using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSetCards;
using Lib.Adapter.UserSetCards.Apis;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Aggregator.UserSetCards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSetCards.Queries;

internal sealed class UserSetCardAggregatorService : IUserSetCardAggregatorService
{
    private readonly IUserSetCardsAdapterService _userSetCardsAdapterService;
    private readonly IUserSetCardItrToXfrMapper _itrToXfrMapper;
    private readonly IUserSetCardExtToOufMapper _extToOufMapper;

    public UserSetCardAggregatorService(ILogger logger) : this(
        new UserSetCardsAdapterService(logger),
        new UserSetCardItrToXfrMapper(),
        new UserSetCardExtToOufMapper())
    { }

    private UserSetCardAggregatorService(
        IUserSetCardsAdapterService userSetCardsAdapterService,
        IUserSetCardItrToXfrMapper itrToXfrMapper,
        IUserSetCardExtToOufMapper extToOufMapper)
    {
        _userSetCardsAdapterService = userSetCardsAdapterService;
        _itrToXfrMapper = itrToXfrMapper;
        _extToOufMapper = extToOufMapper;
    }

    public async Task<IOperationResponse<IUserSetCardOufEntity>> Execute(
        IUserSetCardItrEntity input,
        CancellationToken cancellationToken)
    {
        IUserSetCardGetXfrEntity xfrEntity = await _itrToXfrMapper.Map(input).ConfigureAwait(false);

        IOperationResponse<UserSetCardExtEntity> response = await _userSetCardsAdapterService.GetUserSetCardAsync(xfrEntity, cancellationToken).ConfigureAwait(false);

        if (response is not SuccessOperationResponse<UserSetCardExtEntity> successResponse)
        {
            return new FailureOperationResponse<IUserSetCardOufEntity>(response.OuterException);
        }

        IUserSetCardOufEntity oufEntity = await _extToOufMapper.Map(successResponse.ResponseData).ConfigureAwait(false);

        return new SuccessOperationResponse<IUserSetCardOufEntity>(oufEntity);
    }
}
