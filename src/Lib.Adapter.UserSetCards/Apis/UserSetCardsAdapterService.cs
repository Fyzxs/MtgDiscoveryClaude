using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Adapter.UserSetCards.Commands;
using Lib.Adapter.UserSetCards.Queries;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserSetCards.Apis;

/// <summary>
/// Main user set cards adapter service implementation following the passthrough pattern.
///
/// This service coordinates all user set cards adapter operations by delegating
/// to specialized adapters (query and command).
///
/// Pattern Consistency:
/// Matches SetAdapterService, CardAdapterService patterns
/// to maintain predictable architecture across all layers.
/// </summary>
public sealed class UserSetCardsAdapterService : IUserSetCardsAdapterService
{
    private readonly IUserSetCardsQueryAdapter _queryAdapter;
    private readonly IUserSetCardsCommandAdapter _commandAdapter;

    public UserSetCardsAdapterService(ILogger logger) : this(
        new UserSetCardsQueryAdapter(logger),
        new UserSetCardsCommandAdapter(logger))
    { }

    private UserSetCardsAdapterService(
        IUserSetCardsQueryAdapter queryAdapter,
        IUserSetCardsCommandAdapter commandAdapter)
    {
        _queryAdapter = queryAdapter;
        _commandAdapter = commandAdapter;
    }

    public async Task<IOperationResponse<UserSetCardExtEntity>> GetUserSetCardAsync(IUserSetCardGetXfrEntity readParams) =>
        await _queryAdapter.GetUserSetCardAsync(readParams).ConfigureAwait(false);

    public async Task<IOperationResponse<UserSetCardExtEntity>> AddCardToSetAsync(IAddCardToSetXfrEntity entity) =>
        await _commandAdapter.AddCardToSetAsync(entity).ConfigureAwait(false);
}
