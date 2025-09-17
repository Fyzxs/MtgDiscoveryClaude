using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis;
using Lib.Adapter.Artists.Entities;
using Lib.Adapter.Artists.Exceptions;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;
using Lib.Aggregator.Scryfall.Shared.Entities;
using Lib.Aggregator.Scryfall.Shared.Mappers;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities;
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
    private readonly ScryfallCardItemToCardItemItrEntityMapper _cardMapper;

    public ArtistCosmosQueryAdapter(ILogger logger) : this(
        new ArtistNameTrigramsInquisitor(logger),
        new ScryfallArtistCardsInquisitor(logger),
        new ScryfallCardItemToCardItemItrEntityMapper())
    {
    }

    private ArtistCosmosQueryAdapter(
        ICosmosInquisitor artistNameTrigramsInquisitor,
        ICosmosInquisitor artistCardsInquisitor,
        ScryfallCardItemToCardItemItrEntityMapper cardMapper)
    {
        _artistNameTrigramsInquisitor = artistNameTrigramsInquisitor;
        _artistCardsInquisitor = artistCardsInquisitor;
        _cardMapper = cardMapper;
    }

    public async Task<IOperationResponse<IArtistSearchResultCollectionItrEntity>> SearchArtistsAsync([NotNull] IArtistSearchTermItrEntity searchTerm)
    {
        // Extract primitives for external system interface
        string searchTermValue = searchTerm.SearchTerm;

        // Normalize the search term: lowercase and remove non-alphabetic characters
        string normalized = new([.. searchTermValue
            .ToLowerInvariant()
            .Where(char.IsLetter)]);

        if (normalized.Length < 3)
        {
            return new FailureOperationResponse<IArtistSearchResultCollectionItrEntity>(
                new ArtistAdapterException("Search term must contain at least 3 letters"));
        }

        // Generate trigrams from the normalized search term
        List<string> trigrams = [];
        for (int i = 0; i <= normalized.Length - 3; i++)
        {
            trigrams.Add(normalized.Substring(i, 3));
        }

        // Query each trigram and collect all matching artist names
        HashSet<string> matchingArtistIds = [];
        Dictionary<string, int> artistMatchCounts = [];
        Dictionary<string, string> artistIdToName = [];

        foreach (string trigram in trigrams)
        {
            // Query for this trigram with server-side filtering
            string firstChar = trigram[..1];
            QueryDefinition queryDefinition = new QueryDefinition(
                "SELECT * FROM c WHERE c.id = @trigram AND c.partition = @partition AND EXISTS(SELECT VALUE artist FROM artist IN c.artists WHERE CONTAINS(artist.norm, @normalized))")
                .WithParameter("@trigram", trigram)
                .WithParameter("@partition", firstChar)
                .WithParameter("@normalized", normalized);

            OpResponse<IEnumerable<ArtistNameTrigramExtArg>> trigramResponse = await _artistNameTrigramsInquisitor.QueryAsync<ArtistNameTrigramExtArg>(
                queryDefinition,
                new PartitionKey(firstChar)).ConfigureAwait(false);

            if (trigramResponse.IsSuccessful() && trigramResponse.Value != null)
            {
                foreach (ArtistNameTrigramExtArg trigramDoc in trigramResponse.Value)
                {
                    foreach (ArtistNameTrigramDataExtArg entry in trigramDoc.Artists)
                    {
                        // Server-side filtering should have already filtered, but double-check
                        if (entry.Normalized.Contains(normalized))
                        {
                            matchingArtistIds.Add(entry.ArtistId);
                            artistIdToName[entry.ArtistId] = entry.Name;

                            // Track how many trigrams matched for ranking
                            if (artistMatchCounts.ContainsKey(entry.ArtistId) is false)
                            {
                                artistMatchCounts[entry.ArtistId] = 0;
                            }
                            artistMatchCounts[entry.ArtistId]++;
                        }
                    }
                }
            }
        }

        // Sort by match count (artists matching more trigrams appear first) and convert to result entities
        List<IArtistSearchResultItrEntity> sortedResults = [.. matchingArtistIds
            .OrderByDescending(artistId => artistMatchCounts[artistId])
            .ThenBy(artistId => artistIdToName[artistId])
            .Select(artistId => new ArtistSearchResultItrEntity
            {
                ArtistId = artistId,
                Name = artistIdToName[artistId]
            } as IArtistSearchResultItrEntity)];

        return new SuccessOperationResponse<IArtistSearchResultCollectionItrEntity>(
            new ArtistSearchResultCollectionItrEntity { Artists = sortedResults });
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> GetCardsByArtistIdAsync([NotNull] IArtistIdItrEntity artistId)
    {
        // Extract primitives for external system interface
        string artistIdValue = artistId.ArtistId;

        // Query all cards for this artist ID using the artist ID as partition key
        QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.partition = @artistId")
            .WithParameter("@artistId", artistIdValue);

        OpResponse<IEnumerable<ScryfallArtistCardExtArg>> cardsResponse = await _artistCardsInquisitor.QueryAsync<ScryfallArtistCardExtArg>(
            queryDefinition,
            new PartitionKey(artistIdValue)).ConfigureAwait(false);

        if (cardsResponse.IsSuccessful() is false)
        {
            return new FailureOperationResponse<ICardItemCollectionItrEntity>(
                new ArtistAdapterException($"Failed to retrieve cards for artist '{artistIdValue}'", cardsResponse.Exception()));
        }

        // Convert ScryfallArtistCard to ScryfallCardItem for mapping
        List<ICardItemItrEntity> cards = [];
        foreach (ScryfallArtistCardExtArg artistCard in cardsResponse.Value)
        {
            ScryfallCardExtArg cardItem = new() { Data = artistCard.Data };
            ICardItemItrEntity mappedCard = _cardMapper.Map(cardItem);
            if (mappedCard != null)
            {
                cards.Add(mappedCard);
            }
        }

        return new SuccessOperationResponse<ICardItemCollectionItrEntity>(new CardItemCollectionItrEntity { Data = cards });
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> GetCardsByArtistNameAsync([NotNull] IArtistNameItrEntity artistName)
    {
        // Extract primitives for external system interface
        string artistNameValue = artistName.ArtistName;

        // Normalize the artist name: lowercase and remove non-alphabetic characters
        string normalized = new([.. artistNameValue
            .ToLowerInvariant()
            .Where(char.IsLetter)]);

        if (normalized.Length < 3)
        {
            return new FailureOperationResponse<ICardItemCollectionItrEntity>(
                new ArtistAdapterException("Artist name must contain at least 3 letters"));
        }

        // Generate trigrams from the normalized artist name
        List<string> trigrams = [];
        for (int i = 0; i <= normalized.Length - 3; i++)
        {
            trigrams.Add(normalized.Substring(i, 3));
        }

        // Query each trigram and collect all matching artist names
        HashSet<string> matchingArtistIds = [];
        Dictionary<string, int> artistMatchCounts = [];
        Dictionary<string, string> artistIdToName = [];

        foreach (string trigram in trigrams)
        {
            // Query for this trigram with server-side filtering
            string firstChar = trigram[..1];
            QueryDefinition queryDefinition = new QueryDefinition(
                "SELECT * FROM c WHERE c.id = @trigram AND c.partition = @partition AND EXISTS(SELECT VALUE artist FROM artist IN c.artists WHERE CONTAINS(artist.norm, @normalized))")
                .WithParameter("@trigram", trigram)
                .WithParameter("@partition", firstChar)
                .WithParameter("@normalized", normalized);

            OpResponse<IEnumerable<ArtistNameTrigramExtArg>> trigramResponse = await _artistNameTrigramsInquisitor.QueryAsync<ArtistNameTrigramExtArg>(
                queryDefinition,
                new PartitionKey(firstChar)).ConfigureAwait(false);

            if (trigramResponse.IsSuccessful() && trigramResponse.Value != null)
            {
                foreach (ArtistNameTrigramExtArg trigramDoc in trigramResponse.Value)
                {
                    foreach (ArtistNameTrigramDataExtArg entry in trigramDoc.Artists)
                    {
                        // Server-side filtering should have already filtered, but double-check
                        if (entry.Normalized.Contains(normalized))
                        {
                            matchingArtistIds.Add(entry.ArtistId);
                            artistIdToName[entry.ArtistId] = entry.Name;

                            // Track how many trigrams matched for ranking
                            if (artistMatchCounts.ContainsKey(entry.ArtistId) is false)
                            {
                                artistMatchCounts[entry.ArtistId] = 0;
                            }
                            artistMatchCounts[entry.ArtistId]++;
                        }
                    }
                }
            }
        }

        // Check if we found any matches
        if (matchingArtistIds.Count == 0)
        {
            return new FailureOperationResponse<ICardItemCollectionItrEntity>(
                new ArtistAdapterException($"Artist '{artistNameValue}' not found"));
        }

        // Sort by match count to get best matches
        List<string> sortedMatches = [.. matchingArtistIds
            .OrderByDescending(artistId => artistMatchCounts[artistId])
            .ThenBy(artistId => artistIdToName[artistId])];

        string topMatchArtistId = sortedMatches.First();
        int topMatchScore = artistMatchCounts[topMatchArtistId];
        int totalTrigrams = trigrams.Count;

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
            return new FailureOperationResponse<ICardItemCollectionItrEntity>(
                new ArtistAdapterException($"Multiple artists found for '{artistNameValue}'. Consider using artist search for: {alternatives}"));
        }

        // We have a high confidence match - use existing GetCardsByArtistIdAsync for complete card data
        // Debug: Log which artist ID was selected
        Console.WriteLine($"DEBUG: Selected artist ID '{topMatchArtistId}' for name '{artistNameValue}' with confidence {confidenceScore:F2}");

        // Use existing GetCardsByArtistIdAsync method to get complete card data
        ArtistIdItrEntity artistIdEntity = new() { ArtistId = topMatchArtistId };
        return await GetCardsByArtistIdAsync(artistIdEntity).ConfigureAwait(false);
    }
}
