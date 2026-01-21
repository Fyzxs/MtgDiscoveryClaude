using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Exceptions;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserCards.Queries;

/// <summary>
/// Cosmos DB implementation of the user cards query adapter.
///
/// This class handles all Cosmos DB-specific user cards query operations,
/// implementing the specialized IUserCardsQueryAdapter interface.
/// The main UserCardsAdapterService delegates to this implementation.
/// </summary>
internal sealed class UserCardsQueryAdapter : IUserCardsQueryAdapter
{
    private readonly ICosmosInquisition<UserCardItemsBySetExtEntitys> _userCardsInquisition;

    public UserCardsQueryAdapter(ILogger logger) : this(new UserCardItemsBySetInquisition(logger))
    { }

    private UserCardsQueryAdapter(ICosmosInquisition<UserCardItemsBySetExtEntitys> userCardsInquisition)
    {
        _userCardsInquisition = userCardsInquisition;
    }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet)
    {
        //TODO: This needs to be a mapper
        UserCardItemsBySetExtEntitys args = new() { SetId = userCardsSet.SetId, UserId = userCardsSet.UserId };

        OpResponse<IEnumerable<UserCardExtEntity>> response = await _userCardsInquisition.QueryAsync<UserCardExtEntity>(args).ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<UserCardExtEntity>>(
                new UserCardsAdapterException($"Failed to retrieve [user={userCardsSet.UserId}] cards for [set]{userCardsSet.SetId}]", response.Exception()));
        }

        return new SuccessOperationResponse<IEnumerable<UserCardExtEntity>>(response.Value);
    }
}
