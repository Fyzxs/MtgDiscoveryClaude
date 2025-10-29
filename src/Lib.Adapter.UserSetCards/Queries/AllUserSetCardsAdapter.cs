using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Adapter.UserSetCards.Exceptions;
using Lib.Adapter.UserSetCards.Queries.Mappers;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserSetCards.Queries;

internal sealed class AllUserSetCardsAdapter : IAllUserSetCardsAdapter
{
    private readonly ICosmosInquisition<AllUserSetCardsExtEntitys> _inquisition;
    private readonly IAllUserSetCardsXfrToArgsMapper _xfrToArgsMapper;

    public AllUserSetCardsAdapter(ILogger logger) : this(
        new AllUserSetCardsInquisition(logger),
        new AllUserSetCardsXfrToArgsMapper())
    {
    }

    private AllUserSetCardsAdapter(
        ICosmosInquisition<AllUserSetCardsExtEntitys> inquisition,
        IAllUserSetCardsXfrToArgsMapper xfrToArgsMapper)
    {
        _inquisition = inquisition;
        _xfrToArgsMapper = xfrToArgsMapper;
    }

    public async Task<IOperationResponse<IEnumerable<UserSetCardExtEntity>>> Execute(
        IAllUserSetCardsXfrEntity userSetCards)
    {
        AllUserSetCardsExtEntitys args =
            await _xfrToArgsMapper.Map(userSetCards).ConfigureAwait(false);

        OpResponse<IEnumerable<UserSetCardExtEntity>> response =
            await _inquisition.QueryAsync<UserSetCardExtEntity>(args).ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<UserSetCardExtEntity>>(
                new UserSetCardsAdapterException(
                    $"Failed to query all user set cards for user {userSetCards.UserId}",
                    response.Exception()));
        }

        return new SuccessOperationResponse<IEnumerable<UserSetCardExtEntity>>(response.Value);
    }
}
