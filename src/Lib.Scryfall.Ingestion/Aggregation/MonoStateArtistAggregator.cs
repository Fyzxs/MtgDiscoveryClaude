using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Lib.Scryfall.Ingestion.Apis.Aggregation;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Aggregation;

internal sealed class MonoStateArtistAggregator : IArtistAggregator
{
    private static readonly ConcurrentDictionary<string, ArtistAggregate> s_artists = new();

    public void Track(IScryfallCard card)
    {
        string cardId = card.Id();
        string setId = card.Set().Id();

        // Process each artist ID with its associated name
        foreach (IArtistIdNamePair pair in card.ArtistIdNamePairs())
        {
            string artistId = pair.ArtistId();
            string artistName = pair.ArtistName();

            ArtistAggregate aggregate = s_artists.GetOrAdd(artistId, id => new ArtistAggregate(id));

            aggregate.AddName(artistName);
            aggregate.AddCard(cardId);
            aggregate.AddSet(setId);
        }
    }

    public IEnumerable<IArtistAggregate> GetArtists()
    {
        return s_artists.Values.ToList();
    }

    public IEnumerable<IArtistAggregate> GetDirtyArtists()
    {
        return s_artists.Values.Where(a => a.IsDirty()).ToList();
    }

    public void MarkAllClean()
    {
        foreach (ArtistAggregate artist in s_artists.Values)
        {
            artist.MarkClean();
        }
    }

    public void Clear()
    {
        s_artists.Clear();
    }
}
