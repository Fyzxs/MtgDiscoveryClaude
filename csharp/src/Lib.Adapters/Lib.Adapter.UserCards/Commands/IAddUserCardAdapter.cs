using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserCards.Commands;

/// <summary>
/// Adapter for adding or updating a card in a user's collection.
/// </summary>
internal interface IAddUserCardAdapter
{
    Task<IOperationResponse<UserCardExtEntity>> Execute(
        IAddUserCardXfrEntity input,
        CancellationToken cancellationToken);
}
