using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Args.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Cards;

internal interface ICardNameSearchEntryService
{
    Task<IOperationResponse<List<CardNameSearchResultOutEntity>>> Execute(
        ICardSearchTermArgEntity input,
        CancellationToken cancellationToken);
}
