using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Lib.Scryfall.Ingestion.Apis.Aggregation;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Aggregation;

internal sealed class MonoStateCardNameTrigramAggregator : ICardNameTrigramAggregator
{
    private static readonly ConcurrentDictionary<string, CardNameTrigramAggregate> s_trigrams = new();

    public void Track(IScryfallCard card)
    {
        string actualName = card.Name();

        // Process card name
        ProcessText(actualName, actualName);

        // Process flavor name if present
        string flavorName = card.FlavorName();
        if (string.IsNullOrWhiteSpace(flavorName) is false)
        {
            ProcessText(flavorName, actualName);
        }
    }

    private static void ProcessText(string textToIndex, string nameToStore)
    {
        if (string.IsNullOrWhiteSpace(textToIndex)) return;
        if (string.IsNullOrWhiteSpace(nameToStore)) return;

        // Normalize: lowercase and remove non-alphabetic characters
        string normalized = new string(textToIndex
            .ToLowerInvariant()
            .Where(char.IsLetter)
            .ToArray());

        if (normalized.Length < 3) return;

        // Generate all 3-character trigrams
        for (int position = 0; position <= normalized.Length - 3; position++)
        {
            string trigram = normalized.Substring(position, 3);

            CardNameTrigramAggregate aggregate = s_trigrams.GetOrAdd(
                trigram,
                t => new CardNameTrigramAggregate(t));

            // Store the normalized version of the name for server-side filtering
            string normalizedName = new string(nameToStore
                .ToLowerInvariant()
                .Where(char.IsLetter)
                .ToArray());

            aggregate.AddCard(nameToStore, normalizedName, position);
        }
    }

    public IEnumerable<ICardNameTrigramAggregate> GetTrigrams()
    {
        return s_trigrams.Values.ToList();
    }

    public void Clear()
    {
        s_trigrams.Clear();
    }
}