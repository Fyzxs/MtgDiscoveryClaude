using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.UserCards;

/// <summary>
/// Entry service for adding user cards without updating UserSetCards.
/// Used by migration tools to separately track individual cards and set-level aggregation.
/// </summary>
internal interface IAddUserCardOnlyEntryService
{
    Task<IOperationResponse<List<CardItemOutEntity>>> Execute(IAddCardToCollectionArgsEntity input, CancellationToken cancellationToken);
}
