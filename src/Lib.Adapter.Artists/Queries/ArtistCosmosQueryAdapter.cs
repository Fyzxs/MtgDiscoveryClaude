using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Adapter.Artists.Exceptions;
using Lib.Adapter.Artists.Queries.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Artists.Queries;

/// <summary>
/// Cosmos DB implementation of the artist query adapter.
/// 
/// This class handles all Cosmos DB-specific artist query operations,
/// implementing complex trigram search and disambiguation logic.
/// The main ArtistAdapterService delegates to this implementation.
/// </summary>
internal sealed class ArtistCosmosQueryAdapter : IArtistQueryAdapter
{
    private readonly ICosmosInquisitor _artistNameTrigramsInquisitor;
    private readonly ICosmosInquisitor _artistCardsInquisitor;

    public ArtistCosmosQueryAdapter(ILogger logger) : this(
        new ArtistNameTrigramsInquisitor(logger),
        new ScryfallArtistCardsInquisitor(logger))
    {
    }

    private ArtistCosmosQueryAdapter(
        ICosmosInquisitor artistNameTrigramsInquisitor,
        ICosmosInquisitor artistCardsInquisitor)
    {
        _artistNameTrigramsInquisitor = artistNameTrigramsInquisitor;
        _artistCardsInquisitor = artistCardsInquisitor;
    }

    public async Task<IOperationResponse<IEnumerable<ArtistNameTrigramDataExtEntity>>> SearchArtistsAsync([NotNull] IArtistSearchTermXfrEntity xfrEntity)
    {
        // Query each trigram and collect all matching artist data entities
        HashSet<string> seenArtistIds = [];
        List<ArtistNameTrigramDataExtEntity> matchingArtists = [];
        Dictionary<string, int> artistMatchCounts = [];

        foreach (string trigram in xfrEntity.SearchTerms)
        {
            // Query for this trigram with server-side filtering
            QueryDefinition queryDefinition = new QueryDefinition(
                "SELECT * FROM c WHERE c.id = @trigram AND c.partition = @partition AND EXISTS(SELECT VALUE artist FROM artist IN c.artists WHERE CONTAINS(artist.norm, @normalized))")
                .WithParameter("@trigram", trigram)
                .WithParameter("@partition", trigram[..1])
                .WithParameter("@normalized", xfrEntity.Normalized);

            OpResponse<IEnumerable<ArtistNameTrigramExtEntity>> trigramResponse = await _artistNameTrigramsInquisitor.QueryAsync<ArtistNameTrigramExtEntity>(
                queryDefinition,
                new PartitionKey(trigram[..1])).ConfigureAwait(false);

            if (trigramResponse.IsNotSuccessful() || trigramResponse.Value == null) continue;

            foreach (ArtistNameTrigramExtEntity trigramDoc in trigramResponse.Value)
            {
                foreach (ArtistNameTrigramDataExtEntity entry in trigramDoc.Artists)
                {
                    // Server-side filtering should have already filtered, but double-check
                    if (entry.Normalized.Contains(xfrEntity.Normalized) is false) continue;

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

    public async Task<IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>>> CardsByArtistIdAsync([NotNull] IArtistIdXfrEntity artistId)
    {
        // Extract primitives for external system interface

        // Query all cards for this artist ID using the artist ID as partition key
        QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.partition = @artistId")
            .WithParameter("@artistId", artistId.ArtistId);

        OpResponse<IEnumerable<ScryfallArtistCardExtEntity>> cardsResponse = await _artistCardsInquisitor.QueryAsync<ScryfallArtistCardExtEntity>(
            queryDefinition,
            new PartitionKey(artistId.ArtistId)).ConfigureAwait(false);

        if (cardsResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>>(
                new ArtistAdapterException($"Failed to retrieve cards for artist '{artistId.ArtistId}'", cardsResponse.Exception()));
        }

        return new SuccessOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>>(cardsResponse.Value);
    }

    public async Task<IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>>> CardsByArtistNameAsync([NotNull] IArtistNameXfrEntity artistName)
    {
        // Query each trigram and collect all matching artist names
        HashSet<string> matchingArtistIds = [];
        Dictionary<string, int> artistMatchCounts = [];
        Dictionary<string, string> artistIdToName = [];

        foreach (string trigram in artistName.Trigrams)
        {
            // Query for this trigram with server-side filtering
            string firstChar = trigram[..1];
            QueryDefinition queryDefinition = new QueryDefinition(
                "SELECT * FROM c WHERE c.id = @trigram AND c.partition = @partition AND EXISTS(SELECT VALUE artist FROM artist IN c.artists WHERE CONTAINS(artist.norm, @normalized))")
                .WithParameter("@trigram", trigram)
                .WithParameter("@partition", firstChar)
                .WithParameter("@normalized", artistName.Normalized);

            OpResponse<IEnumerable<ArtistNameTrigramExtEntity>> trigramResponse = await _artistNameTrigramsInquisitor.QueryAsync<ArtistNameTrigramExtEntity>(
                queryDefinition,
                new PartitionKey(firstChar)).ConfigureAwait(false);

            if (trigramResponse.IsNotSuccessful() || trigramResponse.Value == null) continue;

            foreach (ArtistNameTrigramExtEntity trigramDoc in trigramResponse.Value)
            {
                foreach (ArtistNameTrigramDataExtEntity entry in trigramDoc.Artists)
                {
                    // Server-side filtering should have already filtered, but double-check
                    if (entry.Normalized.Contains(artistName.Normalized) is false)
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
                new ArtistAdapterException($"Artist '{artistName.ArtistName}' not found"));
        }

        // Sort by match count to get best matches
        List<string> sortedMatches = [.. matchingArtistIds
            .OrderByDescending(artistId => artistMatchCounts[artistId])
            .ThenBy(artistId => artistIdToName[artistId])];

        string topMatchArtistId = sortedMatches.First();
        int topMatchScore = artistMatchCounts[topMatchArtistId];
        int totalTrigrams = artistName.Trigrams.Count;

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
                new ArtistAdapterException($"Multiple artists found for '{artistName.ArtistName}'. Consider using artist search for: {alternatives}"));
        }

        // Use existing GetCardsByArtistIdAsync method to get complete card data
        ArtistIdXfrEntity artistIdEntity = new() { ArtistId = topMatchArtistId };
        return await CardsByArtistIdAsync(artistIdEntity).ConfigureAwait(false);
    }
}
