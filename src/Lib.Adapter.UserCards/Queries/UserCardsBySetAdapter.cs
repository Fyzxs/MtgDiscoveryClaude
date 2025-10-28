using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Adapter.UserCards.Exceptions;
using Lib.Adapter.UserCards.Queries.Mappers;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserCards.Queries;

/// <summary>
/// Retrieves all user cards within a specific set.
/// </summary>
internal sealed class UserCardsBySetAdapter : IUserCardsBySetAdapter
{
    private readonly ICosmosInquisition<UserCardItemsBySetExtEntitys> _userCardsSetInquisition;
    private readonly IUserCardsSetXfrToArgsMapper _setXfrToArgsMapper;

    public UserCardsBySetAdapter(ILogger logger) : this(new UserCardItemsBySetInquisition(logger), new UserCardsSetXfrToArgsMapper()) { }

    private UserCardsBySetAdapter(ICosmosInquisition<UserCardItemsBySetExtEntitys> userCardsSetInquisition, IUserCardsSetXfrToArgsMapper setXfrToArgsMapper)
    {
        _userCardsSetInquisition = userCardsSetInquisition;
        _setXfrToArgsMapper = setXfrToArgsMapper;
    }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> Execute([NotNull] IUserCardsSetXfrEntity input)
    {
        UserCardItemsBySetExtEntitys args = await _setXfrToArgsMapper.Map(input).ConfigureAwait(false);

        OpResponse<IEnumerable<UserCardExtEntity>> response = await _userCardsSetInquisition.QueryAsync<UserCardExtEntity>(args).ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<UserCardExtEntity>>(
                new UserCardsAdapterException($"Failed to retrieve [user={input.UserId}] cards for [set]{input.SetId}]", response.Exception()));
        }

        return new SuccessOperationResponse<IEnumerable<UserCardExtEntity>>(response.Value);
    }
}
