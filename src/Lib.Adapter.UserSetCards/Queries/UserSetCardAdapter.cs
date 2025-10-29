using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Adapter.UserSetCards.Exceptions;
using Lib.Adapter.UserSetCards.Queries.Mappers;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserSetCards.Queries;

/// <summary>
/// Retrieves user set card data from Cosmos DB storage.
/// Returns empty defaults if record not found.
/// </summary>
internal sealed class UserSetCardAdapter : IUserSetCardAdapter
{
    private readonly ICosmosGopher _userSetCardsGopher;
    private readonly IUserSetCardsGetXfrToExtMapper _readPointMapper;

    public UserSetCardAdapter(ILogger logger) : this(new UserSetCardsGopher(logger), new UserSetCardsGetXfrToExtMapper()) { }

    private UserSetCardAdapter(ICosmosGopher userSetCardsGopher, IUserSetCardsGetXfrToExtMapper readPointMapper)
    {
        _userSetCardsGopher = userSetCardsGopher;
        _readPointMapper = readPointMapper;
    }

    public async Task<IOperationResponse<UserSetCardExtEntity>> Execute([NotNull] IUserSetCardGetXfrEntity input)
    {
        ReadPointItem readPoint = await _readPointMapper.Map(input).ConfigureAwait(false);

        OpResponse<UserSetCardExtEntity> readResponse = await _userSetCardsGopher.ReadAsync<UserSetCardExtEntity>(readPoint).ConfigureAwait(false);

        if (readResponse.IsNotSuccessful())
        {
            // If document not found, return empty UserSetCard instead of failure
            if (readResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                UserSetCardExtEntity emptyCard = new()
                {
                    UserId = input.UserId,
                    SetId = input.SetId,
                    TotalCards = 0,
                    UniqueCards = 0,
                    Collecting = [],
                    Groups = []
                };
                return new SuccessOperationResponse<UserSetCardExtEntity>(emptyCard);
            }

            return new FailureOperationResponse<UserSetCardExtEntity>(new UserSetCardsAdapterException());
        }

        return new SuccessOperationResponse<UserSetCardExtEntity>(readResponse.Value);
    }
}
