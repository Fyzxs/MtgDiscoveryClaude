using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserCards.Queries;

/// <summary>
/// Adapter for retrieving all user cards within a specific set.
/// </summary>
internal interface IUserCardsBySetAdapter
{
    Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> Execute(
        IUserCardsSetXfrEntity input,
        CancellationToken cancellationToken);
}
