using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.Cards.Queries;

/// <summary>
/// Single-method adapter for retrieving cards by exact name.
/// </summary>
internal interface ICardsByNameAdapter
{
    Task<IOperationResponse<IEnumerable<ScryfallCardByNameExtEntity>>> Execute(
        ICardNameXfrEntity input,
        CancellationToken cancellationToken);
}
