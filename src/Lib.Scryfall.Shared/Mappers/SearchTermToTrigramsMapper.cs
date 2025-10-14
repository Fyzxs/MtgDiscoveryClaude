using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Scryfall.Shared.Entities;

namespace Lib.Scryfall.Shared.Mappers;

public sealed class SearchTermToTrigramsMapper : ISearchTermToTrigramsMapper
{
    public Task<ITrigramCollectionEntity> Map([NotNull] string searchTerm)
    {
        // Normalize: lowercase and letters only
        string normalized = new([.. searchTerm
            .ToLowerInvariant()
            .Where(char.IsLetter)]);

        // Generate trigrams (3-character substrings)
        List<string> trigrams = [];
        for (int i = 0; i <= normalized.Length - 3; i++)
        {
            trigrams.Add(normalized.Substring(i, 3));
        }

        ITrigramCollectionEntity result = new TrigramCollectionEntity
        {
            Normalized = normalized,
            Trigrams = trigrams
        };

        return Task.FromResult(result);
    }
}
