using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SetItems;
using Lib.Shared.DataModels.Entities.Xfrs.Sets;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.Sets.Queries;

/// <summary>
/// Single-method adapter for retrieving all sets.
/// </summary>
internal interface IAllSetsAdapter
{
    Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> Execute(
        IAllSetsXfrEntity input,
        CancellationToken cancellationToken);
}
