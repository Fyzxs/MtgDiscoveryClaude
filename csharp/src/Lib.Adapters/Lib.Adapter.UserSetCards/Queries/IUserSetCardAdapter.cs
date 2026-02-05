using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSetCards;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserSetCards.Queries;

/// <summary>
/// Adapter for retrieving user set card data from storage.
/// </summary>
internal interface IUserSetCardAdapter
{
    Task<IOperationResponse<UserSetCardExtEntity>> Execute(
        IUserSetCardGetXfrEntity input,
        CancellationToken cancellationToken);
}
