using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;
using Lib.Aggregator.Artists.Apis;
using Lib.Aggregator.Artists.Entities;
using Lib.Aggregator.Artists.Operations;
using Lib.Aggregator.Scryfall.Shared.Entities;
using Lib.Aggregator.Scryfall.Shared.Mappers;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Artists.Queries;

internal sealed class QueryArtistAggregatorService : IArtistAggregatorService
{
    private readonly ICosmosInquisitor _artistNameTrigramsInquisitor;
    private readonly ICosmosInquisitor _artistCardsInquisitor;
    private readonly ScryfallCardItemToCardItemItrEntityMapper _cardMapper;

    public QueryArtistAggregatorService(ILogger logger) : this(
        new ArtistNameTrigramsInquisitor(logger),
        new ScryfallArtistCardsInquisitor(logger),
        new ScryfallCardItemToCardItemItrEntityMapper())
    {
    }

    private QueryArtistAggregatorService(
        ICosmosInquisitor artistNameTrigramsInquisitor,
        ICosmosInquisitor artistCardsInquisitor,
        ScryfallCardItemToCardItemItrEntityMapper cardMapper)
    {
        _artistNameTrigramsInquisitor = artistNameTrigramsInquisitor;
        _artistCardsInquisitor = artistCardsInquisitor;
        _cardMapper = cardMapper;
    }

    public async Task<IOperationResponse<IArtistSearchResultCollectionItrEntity>> ArtistSearchAsync(IArtistSearchTermItrEntity searchTerm)
    {
        // Normalize the search term: lowercase and remove non-alphabetic characters
        string normalized = new(searchTerm.SearchTerm
            .ToLowerInvariant()
            .Where(char.IsLetter)
            .ToArray());

        if (normalized.Length < 3)
        {
            return new FailureOperationResponse<IArtistSearchResultCollectionItrEntity>(
                new ArtistAggregatorOperationException("Search term must contain at least 3 letters"));
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
            string firstChar = trigram.Substring(0, 1);
            QueryDefinition queryDefinition = new QueryDefinition(
                "SELECT * FROM c WHERE c.id = @trigram AND c.partition = @partition AND EXISTS(SELECT VALUE artist FROM artist IN c.artists WHERE CONTAINS(artist.norm, @normalized))")
                .WithParameter("@trigram", trigram)
                .WithParameter("@partition", firstChar)
                .WithParameter("@normalized", normalized);

            OpResponse<IEnumerable<ArtistNameTrigramItem>> trigramResponse = await _artistNameTrigramsInquisitor.QueryAsync<ArtistNameTrigramItem>(
                queryDefinition,
                new PartitionKey(firstChar)).ConfigureAwait(false);

            if (trigramResponse.IsSuccessful() && trigramResponse.Value != null)
            {
                foreach (ArtistNameTrigramItem trigramDoc in trigramResponse.Value)
                {
                    foreach (ArtistNameTrigramDataItem entry in trigramDoc.Artists)
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
        List<IArtistSearchResultItrEntity> sortedResults = matchingArtistIds
            .OrderByDescending(artistId => artistMatchCounts[artistId])
            .ThenBy(artistId => artistIdToName[artistId])
            .Select(artistId => new ArtistSearchResultItrEntity
            {
                ArtistId = artistId,
                Name = artistIdToName[artistId]
            } as IArtistSearchResultItrEntity)
            .ToList();

        return new SuccessOperationResponse<IArtistSearchResultCollectionItrEntity>(
            new ArtistSearchResultCollectionItrEntity { Artists = sortedResults });
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistAsync(IArtistIdItrEntity artistId)
    {
        // Query all cards for this artist ID using the artist ID as partition key
        QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.partition = @artistId")
            .WithParameter("@artistId", artistId.ArtistId);

        OpResponse<IEnumerable<ScryfallArtistCardItem>> cardsResponse = await _artistCardsInquisitor.QueryAsync<ScryfallArtistCardItem>(
            queryDefinition,
            new PartitionKey(artistId.ArtistId)).ConfigureAwait(false);

        if (cardsResponse.IsSuccessful() is false)
        {
            return new FailureOperationResponse<ICardItemCollectionItrEntity>(
                new ArtistAggregatorOperationException($"Failed to retrieve cards for artist '{artistId.ArtistId}'", cardsResponse.Exception()));
        }

        // Convert ScryfallArtistCard to ScryfallCardItem for mapping
        List<ICardItemItrEntity> cards = [];
        foreach (ScryfallArtistCardItem artistCard in cardsResponse.Value)
        {
            ScryfallCardItem cardItem = new() { Data = artistCard.Data };
            ICardItemItrEntity mappedCard = _cardMapper.Map(cardItem);
            if (mappedCard != null)
            {
                cards.Add(mappedCard);
            }
        }

        return new SuccessOperationResponse<ICardItemCollectionItrEntity>(new CardItemCollectionItrEntity { Data = cards });
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistNameAsync(IArtistNameItrEntity artistName)
    {
        // Normalize the artist name: lowercase and remove non-alphabetic characters
        string normalized = new(artistName.ArtistName
            .ToLowerInvariant()
            .Where(char.IsLetter)
            .ToArray());

        if (normalized.Length < 3)
        {
            return new FailureOperationResponse<ICardItemCollectionItrEntity>(
                new ArtistAggregatorOperationException("Artist name must contain at least 3 letters"));
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
            string firstChar = trigram.Substring(0, 1);
            QueryDefinition queryDefinition = new QueryDefinition(
                "SELECT * FROM c WHERE c.id = @trigram AND c.partition = @partition AND EXISTS(SELECT VALUE artist FROM artist IN c.artists WHERE CONTAINS(artist.norm, @normalized))")
                .WithParameter("@trigram", trigram)
                .WithParameter("@partition", firstChar)
                .WithParameter("@normalized", normalized);

            OpResponse<IEnumerable<ArtistNameTrigramItem>> trigramResponse = await _artistNameTrigramsInquisitor.QueryAsync<ArtistNameTrigramItem>(
                queryDefinition,
                new PartitionKey(firstChar)).ConfigureAwait(false);

            if (trigramResponse.IsSuccessful() && trigramResponse.Value != null)
            {
                foreach (ArtistNameTrigramItem trigramDoc in trigramResponse.Value)
                {
                    foreach (ArtistNameTrigramDataItem entry in trigramDoc.Artists)
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
                new ArtistAggregatorOperationException($"Artist '{artistName.ArtistName}' not found"));
        }

        // Sort by match count to get best matches
        List<string> sortedMatches = matchingArtistIds
            .OrderByDescending(artistId => artistMatchCounts[artistId])
            .ThenBy(artistId => artistIdToName[artistId])
            .ToList();

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
            List<string> ambiguousMatches = sortedMatches
                .Take(3) // Show up to 3 alternatives
                .Select(id => artistIdToName[id])
                .ToList();

            string alternatives = string.Join(", ", ambiguousMatches);
            return new FailureOperationResponse<ICardItemCollectionItrEntity>(
                new ArtistAggregatorOperationException($"Multiple artists found for '{artistName.ArtistName}'. Consider using artist search for: {alternatives}"));
        }

        // We have a high confidence match - use existing CardsByArtistAsync for complete card data
        // Debug: Log which artist ID was selected
        Console.WriteLine($"DEBUG: Selected artist ID '{topMatchArtistId}' for name '{artistName.ArtistName}' with confidence {confidenceScore:F2}");

        // Use existing CardsByArtistAsync method to get complete card data
        ArtistIdItrEntity artistIdEntity = new() { ArtistId = topMatchArtistId };
        return await CardsByArtistAsync(artistIdEntity).ConfigureAwait(false);
    }
}
