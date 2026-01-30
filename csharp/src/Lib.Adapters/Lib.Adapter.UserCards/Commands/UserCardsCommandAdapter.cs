using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserCards.Commands;

/// <summary>
/// Cosmos DB implementation of the user cards command adapter.
///
/// This class coordinates all Cosmos DB-specific user cards command operations
/// by delegating to specialized single-method adapters.
/// The main UserCardsAdapterService delegates to this implementation.
/// </summary>
internal sealed class UserCardsCommandAdapter : IUserCardsCommandAdapter
{
    private readonly IAddUserCardAdapter _addUserCardAdapter;

    public UserCardsCommandAdapter(ILogger logger) : this(new AddUserCardAdapter(logger)) { }

    private UserCardsCommandAdapter(IAddUserCardAdapter addUserCardAdapter) => _addUserCardAdapter = addUserCardAdapter;

    public Task<IOperationResponse<UserCardExtEntity>> AddUserCardAsync(IAddUserCardXfrEntity addUserCard) => _addUserCardAdapter.Execute(addUserCard);
}
