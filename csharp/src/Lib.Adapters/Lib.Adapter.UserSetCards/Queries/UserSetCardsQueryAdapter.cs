using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSetCards;
using Lib.Adapter.UserSetCards.Apis;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserSetCards.Queries;

/// <summary>
/// Cosmos DB implementation of the user set card query adapter.
///
/// This class coordinates all Cosmos DB-specific user set card query operations
/// by delegating to specialized single-method adapters.
/// The main UserSetCardsAdapterService delegates to this implementation.
/// </summary>
internal sealed class UserSetCardsQueryAdapter : IUserSetCardsQueryAdapter
{
    private readonly IUserSetCardAdapter _getUserSetCardAdapter;
    private readonly IAllUserSetCardsAdapter _getAllUserSetCardsAdapter;

    public UserSetCardsQueryAdapter(ILogger logger) : this(
        new UserSetCardAdapter(logger),
        new AllUserSetCardsAdapter(logger))
    {
    }

    private UserSetCardsQueryAdapter(
        IUserSetCardAdapter getUserSetCardAdapter,
        IAllUserSetCardsAdapter getAllUserSetCardsAdapter)
    {
        _getUserSetCardAdapter = getUserSetCardAdapter;
        _getAllUserSetCardsAdapter = getAllUserSetCardsAdapter;
    }

    public async Task<IOperationResponse<UserSetCardExtEntity>> GetUserSetCardAsync(
        IUserSetCardGetXfrEntity readParams,
        CancellationToken cancellationToken)
        => await _getUserSetCardAdapter.Execute(readParams, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<UserSetCardExtEntity>>> GetAllUserSetCardsAsync(
        IAllUserSetCardsXfrEntity queryParams,
        CancellationToken cancellationToken)
        => await _getAllUserSetCardsAdapter.Execute(queryParams, cancellationToken).ConfigureAwait(false);
}
