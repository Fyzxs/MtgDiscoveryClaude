using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Lib.Scryfall.Ingestion.Apis.Aggregation;

namespace Lib.Scryfall.Ingestion.Aggregation;

internal sealed class MonoStateArtistTrigramAggregator : IArtistTrigramAggregator
{
    private static readonly ConcurrentDictionary<string, ArtistTrigramAggregate> s_trigrams = new();

    public void TrackArtist(string artistId, IEnumerable<string> artistNames)
    {
        foreach (string artistName in artistNames)
        {
            ProcessArtistName(artistId, artistName);
        }
    }

    private static void ProcessArtistName(string artistId, string artistName)
    {
        if (string.IsNullOrWhiteSpace(artistName)) return;

        // Normalize: lowercase and remove non-alphabetic characters
        string normalized = new(artistName
            .ToLowerInvariant()
            .Where(char.IsLetter)
            .ToArray());

        if (normalized.Length < 3) return;

        // Generate all 3-character trigrams
        for (int position = 0; position <= normalized.Length - 3; position++)
        {
            string trigram = normalized.Substring(position, 3);

            ArtistTrigramAggregate aggregate = s_trigrams.GetOrAdd(
                trigram,
                t => new ArtistTrigramAggregate(t));

            aggregate.AddArtist(artistId, artistName, normalized, position);
        }
    }

    public IEnumerable<IArtistTrigramAggregate> GetTrigrams()
    {
        return s_trigrams.Values.ToList();
    }

    public void Clear()
    {
        s_trigrams.Clear();
    }
}