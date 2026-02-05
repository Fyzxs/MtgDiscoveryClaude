using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSetCards;
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserSetCards.Commands;

/// <summary>
/// Adapter for adding a set group to a user's set collection tracking.
/// Implements atomic read-modify-write pattern.
/// </summary>
internal interface IAddSetGroupToUserSetCardAdapter
{
    Task<IOperationResponse<UserSetCardExtEntity>> Execute(
        IAddSetGroupToUserSetCardXfrEntity input,
        CancellationToken cancellationToken);
}
