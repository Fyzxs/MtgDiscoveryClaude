using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Artists.Queries;

/// <summary>
/// Searches for artists using trigram matching with ranking by match count.
/// </summary>
internal sealed class SearchArtistsAdapter : ISearchArtistsAdapter
{
    private readonly ICosmosInquisition<ArtistNameTrigramSearchInquisitionArgs> _artistNameTrigramSearchInquisition;

    public SearchArtistsAdapter(ILogger logger) : this(new ArtistNameTrigramSearchInquisition(logger)) { }

    private SearchArtistsAdapter(ICosmosInquisition<ArtistNameTrigramSearchInquisitionArgs> artistNameTrigramSearchInquisition) =>
        _artistNameTrigramSearchInquisition = artistNameTrigramSearchInquisition;

    public async Task<IOperationResponse<IEnumerable<ArtistNameTrigramDataExtEntity>>> Execute([NotNull] IArtistSearchTermXfrEntity input)
    {
        // Query each trigram and collect all matching artist data entities
        HashSet<string> seenArtistIds = [];
        List<ArtistNameTrigramDataExtEntity> matchingArtists = [];
        Dictionary<string, int> artistMatchCounts = [];

        foreach (string trigram in input.SearchTerms)
        {
            // Query for this trigram with server-side filtering
            ArtistNameTrigramSearchInquisitionArgs args = new()
            {
                Trigram = trigram,
                Partition = trigram[..1],
                Normalized = input.Normalized
            };

            OpResponse<IEnumerable<ArtistNameTrigramExtEntity>> trigramResponse = await _artistNameTrigramSearchInquisition
                .QueryAsync<ArtistNameTrigramExtEntity>(args)
                .ConfigureAwait(false);

            if (trigramResponse.IsNotSuccessful() || trigramResponse.Value == null) continue;

            foreach (ArtistNameTrigramExtEntity trigramDoc in trigramResponse.Value)
            {
                foreach (ArtistNameTrigramDataExtEntity entry in trigramDoc.Artists)
                {
                    // Server-side filtering should have already filtered, but double-check
                    if (entry.Normalized.Contains(input.Normalized) is false) continue;

                    // Track unique artists and their match counts for sorting
                    if (seenArtistIds.Add(entry.ArtistId))
                    {
                        matchingArtists.Add(entry);
                        artistMatchCounts[entry.ArtistId] = 1;
                    }
                    else
                    {
                        artistMatchCounts[entry.ArtistId]++;
                    }
                }
            }
        }

        // Sort by match count (artists matching more trigrams appear first), then by name
        List<ArtistNameTrigramDataExtEntity> sortedResults = [.. matchingArtists
            .OrderByDescending(artist => artistMatchCounts[artist.ArtistId])
            .ThenBy(artist => artist.Name)];

        return new SuccessOperationResponse<IEnumerable<ArtistNameTrigramDataExtEntity>>(sortedResults);
    }
}
