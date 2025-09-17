using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Artists.Queries.Entities;

internal sealed class ArtistSearchResultCollectionItrEntity : IArtistSearchResultCollectionItrEntity
{
    public ICollection<IArtistSearchResultItrEntity> Artists { get; init; }
}
