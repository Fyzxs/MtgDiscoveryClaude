using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Artists;
using Lib.Shared.DataModels.Entities.Args;

namespace Lib.MtgDiscovery.Entry.Queries.Artists;

internal interface IArtistSearchEntryService : IOperationResponseService<IArtistSearchTermArgEntity, List<ArtistSearchResultOutEntity>>
{
}
