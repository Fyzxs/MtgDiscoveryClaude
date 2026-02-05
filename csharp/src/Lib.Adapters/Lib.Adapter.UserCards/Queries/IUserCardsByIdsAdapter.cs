using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserCards.Queries;

/// <summary>
/// Adapter for retrieving multiple user cards using parallel point reads.
/// </summary>
internal interface IUserCardsByIdsAdapter
{
    Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> Execute(
        IUserCardsByIdsXfrEntity input,
        CancellationToken cancellationToken);
}
