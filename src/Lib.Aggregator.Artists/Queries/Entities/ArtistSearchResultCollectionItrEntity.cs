using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Artists.Queries.Entities;

internal sealed class ArtistSearchResultCollectionOufEntity : IArtistSearchResultCollectionOufEntity
{
    public ICollection<IArtistSearchResultItrEntity> Artists { get; init; }
}
