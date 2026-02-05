using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.Cards.Queries;

/// <summary>
/// Single-method adapter for retrieving cards by set code.
/// </summary>
internal interface ICardsBySetCodeAdapter
{
    Task<IOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>>> Execute(
        ISetCodeXfrEntity input,
        CancellationToken cancellationToken);
}
