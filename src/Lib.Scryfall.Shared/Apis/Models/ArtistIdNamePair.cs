namespace Lib.Scryfall.Shared.Apis.Models;

//TODO: this one doesn't feel right.
public sealed class ArtistIdNamePair : IArtistIdNamePair
{
    private readonly string _artistId;
    private readonly string _artistName;

    public ArtistIdNamePair(string artistId, string artistName)
    {
        _artistId = artistId;
        _artistName = artistName;
    }

    public string ArtistId() => _artistId;
    public string ArtistName() => _artistName;
}
