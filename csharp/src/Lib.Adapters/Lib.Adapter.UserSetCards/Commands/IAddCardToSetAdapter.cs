using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserSetCards.Commands;

/// <summary>
/// Adapter for adding or removing a card from a user's set collection.
/// Implements atomic read-modify-write pattern.
/// </summary>
internal interface IAddCardToSetAdapter
{
    Task<IOperationResponse<UserSetCardExtEntity>> Execute(
        IAddCardToSetXfrEntity input,
        CancellationToken cancellationToken);
}
