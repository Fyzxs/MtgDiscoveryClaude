using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
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
    private readonly IGetUserSetCardAdapter _getUserSetCardAdapter;

    public UserSetCardsQueryAdapter(ILogger logger) : this(new GetUserSetCardAdapter(logger)) { }

    private UserSetCardsQueryAdapter(IGetUserSetCardAdapter getUserSetCardAdapter) => _getUserSetCardAdapter = getUserSetCardAdapter;

    public Task<IOperationResponse<UserSetCardExtEntity>> GetUserSetCardAsync(IUserSetCardGetXfrEntity readParams) => _getUserSetCardAdapter.Execute(readParams);
}
