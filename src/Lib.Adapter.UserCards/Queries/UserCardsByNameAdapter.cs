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
/// Retrieves all user cards with a specific card name.
/// </summary>
internal sealed class UserCardsByNameAdapter : IUserCardsByNameAdapter
{
    private readonly ICosmosInquisition<UserCardItemsByNameExtEntitys> _userCardsNameInquisition;
    private readonly IUserCardsNameXfrToArgsMapper _nameXfrToArgsMapper;

    public UserCardsByNameAdapter(ILogger logger) : this(new UserCardItemsByNameInquisition(logger), new UserCardsNameXfrToArgsMapper()) { }

    private UserCardsByNameAdapter(ICosmosInquisition<UserCardItemsByNameExtEntitys> userCardsNameInquisition, IUserCardsNameXfrToArgsMapper nameXfrToArgsMapper)
    {
        _userCardsNameInquisition = userCardsNameInquisition;
        _nameXfrToArgsMapper = nameXfrToArgsMapper;
    }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> Execute([NotNull] IUserCardsNameXfrEntity input)
    {
        UserCardItemsByNameExtEntitys args = await _nameXfrToArgsMapper.Map(input).ConfigureAwait(false);

        OpResponse<IEnumerable<UserCardExtEntity>> response = await _userCardsNameInquisition.QueryAsync<UserCardExtEntity>(args).ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<UserCardExtEntity>>(
                new UserCardsAdapterException($"Failed to retrieve [user={input.UserId}] cards for [name]{input.CardName}]", response.Exception()));
        }

        return new SuccessOperationResponse<IEnumerable<UserCardExtEntity>>(response.Value);
    }
}
