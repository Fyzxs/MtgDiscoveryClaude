using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Commands.Mappers;
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
    private readonly ICosmosInquisition<UserCardItemsBySetExtArgs> _userCardsInquisition;
    private readonly IUserCardCollectionItrEntityMapper _mapper;

    public UserCardsQueryAdapter(ILogger logger) : this(
        new UserCardItemsBySetInquisition(logger),
        new UserCardCollectionItrEntityMapper())
    { }

    private UserCardsQueryAdapter(
        ICosmosInquisition<UserCardItemsBySetExtArgs> userCardsInquisition,
        IUserCardCollectionItrEntityMapper mapper)
    {
        _userCardsInquisition = userCardsInquisition;
        _mapper = mapper;
    }

    public async Task<IOperationResponse<IEnumerable<IUserCardCollectionItrEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet)
    {
        //TODO: This needs to be a mapper
        UserCardItemsBySetExtArgs args = new() { SetId = userCardsSet.SetId, UserId = userCardsSet.UserId };

        OpResponse<IEnumerable<UserCardItem>> response = await _userCardsInquisition.QueryAsync<UserCardItem>(args).ConfigureAwait(false);

        if (response.IsSuccessful() is false)
        {
            return new FailureOperationResponse<IEnumerable<IUserCardCollectionItrEntity>>(
                new UserCardsAdapterException($"Failed to retrieve [user={userCardsSet.UserId}] cards for [set]{userCardsSet.SetId}]", response.Exception()));
        }

        //TODO: This needs a 'wrapper' mapper to handle the collection aspect.
        IEnumerable<IUserCardCollectionItrEntity> userCards = response.Value
            .Select(x => _mapper.Map(x))
            .Select(x => x.Result)
            .Where(x => x is not null);

        return new SuccessOperationResponse<IEnumerable<IUserCardCollectionItrEntity>>(userCards);
    }
}
