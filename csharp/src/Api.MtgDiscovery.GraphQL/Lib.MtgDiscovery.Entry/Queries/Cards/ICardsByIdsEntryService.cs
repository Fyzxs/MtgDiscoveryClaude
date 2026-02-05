using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Cards;

internal interface ICardsByIdsEntryService
{
    Task<IOperationResponse<List<CardItemOutEntity>>> Execute(
        ICardIdsArgEntity input,
        CancellationToken cancellationToken);
}
