using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSetCards.Apis;
using Lib.Aggregator.UserSetCards.Apis;
using Lib.Aggregator.UserSetCards.Commands.Mappers;
using Lib.Aggregator.UserSetCards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSetCards.Commands;

internal sealed class UserSetCardsCommandAggregator : IUserSetCardsCommandAggregator
{
    private readonly IUserSetCardsAdapterService _adapter;
    private readonly IAddSetGroupItrToXfrMapper _itrToXfrMapper;
    private readonly IUserSetCardExtToItrMapper _extToOufMapper;

    public UserSetCardsCommandAggregator(ILogger logger) : this(
        new UserSetCardsAdapterService(logger),
        new AddSetGroupItrToXfrMapper(),
        new UserSetCardExtToItrMapper())
    { }

    private UserSetCardsCommandAggregator(
        IUserSetCardsAdapterService adapter,
        IAddSetGroupItrToXfrMapper itrToXfrMapper,
        IUserSetCardExtToItrMapper extToOufMapper)
    {
        _adapter = adapter;
        _itrToXfrMapper = itrToXfrMapper;
        _extToOufMapper = extToOufMapper;
    }

    public async Task<IOperationResponse<IUserSetCardOufEntity>> AddSetGroupToUserSetCardAsync(IAddSetGroupToUserSetCardItrEntity entity)
    {
        IAddSetGroupToUserSetCardXfrEntity xfrEntity = await _itrToXfrMapper.Map(entity).ConfigureAwait(false);
        IOperationResponse<UserSetCardExtEntity> response = await _adapter.AddSetGroupToUserSetCardAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IUserSetCardOufEntity>(response.OuterException);
        }

        IUserSetCardOufEntity oufEntity = await _extToOufMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IUserSetCardOufEntity>(oufEntity);
    }
}
