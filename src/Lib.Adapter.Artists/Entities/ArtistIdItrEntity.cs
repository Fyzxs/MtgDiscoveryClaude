using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.Artists.Entities;

/// <summary>
/// Adapter-specific implementation of IArtistIdItrEntity.
/// 
/// This internal entity is used for creating artist ID requests during recursive
/// operations within the adapter layer, such as when CardsByArtistNameAsync
/// needs to call CardsByArtistIdAsync internally.
/// </summary>
internal sealed class ArtistIdItrEntity : IArtistIdItrEntity
{
    public string ArtistId { get; init; } = string.Empty;
}
