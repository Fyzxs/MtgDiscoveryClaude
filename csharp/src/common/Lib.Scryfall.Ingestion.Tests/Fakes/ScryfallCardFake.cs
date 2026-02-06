using System.Collections.Generic;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Tests.Fakes;

internal sealed class ScryfallCardFake : IScryfallCard
{
    private readonly string _id;
    private readonly string _name;
    private readonly string _flavorName;
    private readonly dynamic _data;
    private readonly IScryfallSet _set;
    private readonly IEnumerable<IArtistIdNamePair> _artistIdNamePairs;

    public ScryfallCardFake(
        string id = "card-id",
        string name = "Test Card",
        string flavorName = null,
        dynamic data = null,
        IScryfallSet set = null,
        IEnumerable<IArtistIdNamePair> artistIdNamePairs = null)
    {
        _id = id;
        _name = name;
        _flavorName = flavorName;
        _data = data ?? new { };
        _set = set ?? new ScryfallSetFake();
        _artistIdNamePairs = artistIdNamePairs ?? [];
    }

    public string Id() => _id;
    public string Name() => _name;
    public string FlavorName() => _flavorName;
    public dynamic Data() => _data;
    public IScryfallSet Set() => _set;
    public ICardImageInfoCollection ImageUris() => throw new System.NotImplementedException();
    public IEnumerable<string> ArtistIds() => throw new System.NotImplementedException();
    public IEnumerable<IArtistIdNamePair> ArtistIdNamePairs() => _artistIdNamePairs;
}
