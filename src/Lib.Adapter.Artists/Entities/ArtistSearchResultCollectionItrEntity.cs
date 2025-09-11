using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.Artists.Entities;

/// <summary>
/// Adapter-specific implementation of IArtistSearchResultCollectionItrEntity.
/// 
/// This internal entity represents a collection of artist search results within the adapter.
/// It aggregates individual search results for return to the aggregator layer.
/// </summary>
internal sealed class ArtistSearchResultCollectionItrEntity : IArtistSearchResultCollectionItrEntity
{
    public ICollection<IArtistSearchResultItrEntity> Artists { get; init; } = [];
}