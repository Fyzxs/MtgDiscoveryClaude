using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Artists;
using Lib.Shared.DataModels.Entities.Args.Artists;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Artists;

internal interface IArtistSearchEntryService
{
    Task<IOperationResponse<List<ArtistSearchResultOutEntity>>> Execute(
        IArtistSearchTermArgEntity searchTerm,
        CancellationToken cancellationToken);
}
