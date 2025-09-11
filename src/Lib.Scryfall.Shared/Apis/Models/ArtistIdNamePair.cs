namespace Lib.Scryfall.Shared.Apis.Models;

public sealed class ArtistIdNamePair : IArtistIdNamePair
{
    public string ArtistId { get; init; }
    public string ArtistName { get; init; }
}
