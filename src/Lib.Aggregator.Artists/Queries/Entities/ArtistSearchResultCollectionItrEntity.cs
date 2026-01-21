using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Artists.Queries.Entities;

internal sealed class ArtistSearchResultCollectionItrEntity : IArtistSearchResultCollectionItrEntity
{
    public ICollection<IArtistSearchResultItrEntity> Artists { get; init; }
}
