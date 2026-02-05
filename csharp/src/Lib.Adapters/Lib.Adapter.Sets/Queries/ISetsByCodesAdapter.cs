using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Sets.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.Sets.Queries;

/// <summary>
/// Single-method adapter for retrieving sets by their codes.
/// </summary>
internal interface ISetsByCodesAdapter
{
    Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> Execute(
        ISetCodesXfrEntity input,
        CancellationToken cancellationToken);
}
