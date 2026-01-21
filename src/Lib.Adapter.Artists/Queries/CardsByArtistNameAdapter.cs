using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Adapter.Artists.Exceptions;
using Lib.Adapter.Artists.Queries.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Artists.Queries;

/// <summary>
/// Retrieves cards by artist name with smart disambiguation logic.
/// </summary>
internal sealed class CardsByArtistNameAdapter : ICardsByArtistNameAdapter
{
    private readonly ICosmosInquisition<ArtistNameTrigramSearchInquisitionArgs> _artistNameTrigramSearchInquisition;
    private readonly ICardsByArtistIdAdapter _cardsByArtistIdAdapter;

    public CardsByArtistNameAdapter(ILogger logger) : this(
        new ArtistNameTrigramSearchInquisition(logger),
        new CardsByArtistIdAdapter(logger))
    { }

    private CardsByArtistNameAdapter(
        ICosmosInquisition<ArtistNameTrigramSearchInquisitionArgs> artistNameTrigramSearchInquisition,
        ICardsByArtistIdAdapter cardsByArtistIdAdapter)
    {
        _artistNameTrigramSearchInquisition = artistNameTrigramSearchInquisition;
        _cardsByArtistIdAdapter = cardsByArtistIdAdapter;
    }

    public async Task<IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>>> Execute([NotNull] IArtistNameXfrEntity input)
    {
        // Query each trigram and collect all matching artist names
        HashSet<string> matchingArtistIds = [];
        Dictionary<string, int> artistMatchCounts = [];
        Dictionary<string, string> artistIdToName = [];

        foreach (string trigram in input.Trigrams)
        {
            // Query for this trigram with server-side filtering
            string firstChar = trigram[..1];
            ArtistNameTrigramSearchInquisitionArgs args = new()
            {
                Trigram = trigram,
                Partition = firstChar,
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
                    if (entry.Normalized.Contains(input.Normalized) is false)
                    {
                        continue;
                    }

                    matchingArtistIds.Add(entry.ArtistId);
                    artistIdToName[entry.ArtistId] = entry.Name;

                    artistMatchCounts.TryAdd(entry.ArtistId, 0);
                    artistMatchCounts[entry.ArtistId]++;
                }
            }
        }

        // Check if we found any matches
        if (matchingArtistIds.Count == 0)
        {
            return new FailureOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>>(
                new ArtistAdapterException($"Artist '{input.ArtistName}' not found"));
        }

        // Sort by match count to get best matches
        List<string> sortedMatches = [.. matchingArtistIds
            .OrderByDescending(artistId => artistMatchCounts[artistId])
            .ThenBy(artistId => artistIdToName[artistId])];

        string topMatchArtistId = sortedMatches.First();
        int topMatchScore = artistMatchCounts[topMatchArtistId];
        int totalTrigrams = input.Trigrams.Count;

        // Smart disambiguation logic
        double confidenceScore = (double)topMatchScore / totalTrigrams;

        // Check if we have a second match and how it compares
        bool hasSecondMatch = sortedMatches.Count > 1;
        double secondMatchScore = 0.0;
        if (hasSecondMatch)
        {
            string secondMatchArtistId = sortedMatches[1];
            secondMatchScore = (double)artistMatchCounts[secondMatchArtistId] / totalTrigrams;
        }

        // High confidence: good match score and either no second match or significant gap
        bool isHighConfidence = confidenceScore >= 0.6 && (hasSecondMatch is false || confidenceScore - secondMatchScore >= 0.2);

        if (isHighConfidence is false)
        {
            List<string> ambiguousMatches = [.. sortedMatches
                .Take(3) // Show up to 3 alternatives
                .Select(id => artistIdToName[id])];

            string alternatives = string.Join(", ", ambiguousMatches);
            return new FailureOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>>(
                new ArtistAdapterException($"Multiple artists found for '{input.ArtistName}'. Consider using artist search for: {alternatives}"));
        }

        // Use existing GetCardsByArtistIdAsync method to get complete card data
        ArtistIdXfrEntity artistIdEntity = new() { ArtistId = topMatchArtistId };
        return await _cardsByArtistIdAdapter.Execute(artistIdEntity).ConfigureAwait(false);
    }
}
