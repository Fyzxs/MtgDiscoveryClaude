using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Adapter.UserSetCards.Commands;
using Lib.Adapter.UserSetCards.Queries;
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;
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

    public async Task<IOperationResponse<UserSetCardExtEntity>> GetUserSetCardAsync(
        IUserSetCardGetXfrEntity readParams,
        CancellationToken cancellationToken)
        => await _queryAdapter.GetUserSetCardAsync(readParams, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<UserSetCardExtEntity>>> GetAllUserSetCardsAsync(
        IAllUserSetCardsXfrEntity queryParams,
        CancellationToken cancellationToken)
        => await _queryAdapter.GetAllUserSetCardsAsync(queryParams, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<UserSetCardExtEntity>> AddCardToSetAsync(
        IAddCardToSetXfrEntity entity,
        CancellationToken cancellationToken)
        => await _commandAdapter.AddCardToSetAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<UserSetCardExtEntity>> AddSetGroupToUserSetCardAsync(
        IAddSetGroupToUserSetCardXfrEntity entity,
        CancellationToken cancellationToken)
        => await _commandAdapter.AddSetGroupToUserSetCardAsync(entity, cancellationToken).ConfigureAwait(false);
}
