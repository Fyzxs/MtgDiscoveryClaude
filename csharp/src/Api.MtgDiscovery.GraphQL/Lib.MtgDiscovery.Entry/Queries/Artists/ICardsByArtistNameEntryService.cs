using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Args.Artists;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Artists;

internal interface ICardsByArtistNameEntryService
{
    Task<IOperationResponse<List<CardItemOutEntity>>> Execute(
        IArtistNameArgEntity artistName,
        CancellationToken cancellationToken);
}
