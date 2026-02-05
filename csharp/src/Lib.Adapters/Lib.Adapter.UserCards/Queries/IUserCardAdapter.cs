using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserCards.Queries;

/// <summary>
/// Adapter for retrieving a specific user card using point read.
/// </summary>
internal interface IUserCardAdapter
{
    Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> Execute(
        IUserCardXfrEntity input,
        CancellationToken cancellationToken);
}
