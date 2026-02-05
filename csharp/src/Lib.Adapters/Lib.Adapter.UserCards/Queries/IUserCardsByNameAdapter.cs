using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserCards.Queries;

/// <summary>
/// Adapter for retrieving all user cards with a specific card name.
/// </summary>
internal interface IUserCardsByNameAdapter
{
    Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> Execute(
        IUserCardsNameXfrEntity input,
        CancellationToken cancellationToken);
}
