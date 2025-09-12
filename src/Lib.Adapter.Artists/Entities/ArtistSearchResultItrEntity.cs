using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.Artists.Entities;

/// <summary>
/// Adapter-specific implementation of IArtistSearchResultItrEntity.
/// 
/// This internal entity represents individual artist search results within the adapter.
/// It is created by the query adapter during search operations.
/// </summary>
internal sealed class ArtistSearchResultItrEntity : IArtistSearchResultItrEntity
{
    public string ArtistId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
}
