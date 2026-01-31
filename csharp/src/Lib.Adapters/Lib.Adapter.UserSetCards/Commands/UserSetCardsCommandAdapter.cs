using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSetCards.Apis;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserSetCards.Commands;

/// <summary>
/// Cosmos DB implementation of the user set cards command adapter.
///
/// This class coordinates all Cosmos DB-specific user set cards command operations
/// by delegating to specialized single-method adapters.
/// The main UserSetCardsAdapterService delegates to this implementation.
/// </summary>
internal sealed class UserSetCardsCommandAdapter : IUserSetCardsCommandAdapter
{
    private readonly IAddCardToSetAdapter _addCardToSetAdapter;
    private readonly IAddSetGroupToUserSetCardAdapter _addSetGroupAdapter;

    public UserSetCardsCommandAdapter(ILogger logger) : this(new AddCardToSetAdapter(logger), new AddSetGroupToUserSetCardAdapter(logger)) { }

    private UserSetCardsCommandAdapter(IAddCardToSetAdapter addCardToSetAdapter, IAddSetGroupToUserSetCardAdapter addSetGroupAdapter)
    {
        _addCardToSetAdapter = addCardToSetAdapter;
        _addSetGroupAdapter = addSetGroupAdapter;
    }

    public async Task<IOperationResponse<UserSetCardExtEntity>> AddCardToSetAsync(IAddCardToSetXfrEntity entity) => await _addCardToSetAdapter.Execute(entity);

    public async Task<IOperationResponse<UserSetCardExtEntity>> AddSetGroupToUserSetCardAsync(IAddSetGroupToUserSetCardXfrEntity addSetGroup) => await _addSetGroupAdapter.Execute(addSetGroup);
}
