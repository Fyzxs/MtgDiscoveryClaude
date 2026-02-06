using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Tests.Fakes;

internal sealed class ArtistIdNamePairFake : IArtistIdNamePair
{
    public ArtistIdNamePairFake(string artistId, string artistName)
    {
        ArtistId = artistId;
        ArtistName = artistName;
    }

    public string ArtistId { get; }
    public string ArtistName { get; }
}
