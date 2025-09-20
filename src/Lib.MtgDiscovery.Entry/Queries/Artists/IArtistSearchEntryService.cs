using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Outs.Artists;

namespace Lib.MtgDiscovery.Entry.Queries.Artists;

internal interface IArtistSearchEntryService : IOperationResponseService<IArtistSearchTermArgEntity, List<ArtistSearchResultOutEntity>>
{
}