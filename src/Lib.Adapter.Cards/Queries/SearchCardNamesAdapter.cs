using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Adapter.Cards.Exceptions;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;
using Lib.Cosmos.Apis.Operators;
using Lib.Scryfall.Shared.Entities;
using Lib.Scryfall.Shared.Mappers;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Cards.Queries;

/// <summary>
/// Searches card names using trigram matching and ranking.
/// </summary>
internal sealed class SearchCardNamesAdapter : ISearchCardNamesAdapter
{
    private readonly ICosmosInquisition<CardNameTrigramSearchInquisitionArgs> _cardNameTrigramSearchInquisition;
    private readonly ISearchTermToTrigramsMapper _trigramMapper;

    public SearchCardNamesAdapter(ILogger logger) : this(
        new CardNameTrigramSearchInquisition(logger),
        new SearchTermToTrigramsMapper())
    { }

    private SearchCardNamesAdapter(
        ICosmosInquisition<CardNameTrigramSearchInquisitionArgs> cardNameTrigramSearchInquisition,
        ISearchTermToTrigramsMapper trigramMapper)
    {
        _cardNameTrigramSearchInquisition = cardNameTrigramSearchInquisition;
        _trigramMapper = trigramMapper;
    }

    public async Task<IOperationResponse<IEnumerable<string>>> Execute([NotNull] ICardSearchTermXfrEntity input)
    {
        // Extract primitives for external system interface
        string searchTermValue = input.SearchTerm;

        // Generate normalized string and trigrams
        ITrigramCollectionEntity trigramCollection = await _trigramMapper.Map(searchTermValue).ConfigureAwait(false);
        string normalized = trigramCollection.Normalized;
        IReadOnlyList<string> trigrams = trigramCollection.Trigrams;

        if (normalized.Length < 3)
        {
            return new FailureOperationResponse<IEnumerable<string>>(new CardAdapterException("Search term must contain at least 3 letters"));
        }

        HashSet<string> matchingCardNames = [];
        Dictionary<string, int> cardNameMatchCounts = [];

        foreach (string trigram in trigrams)
        {
            string firstChar = trigram[..1];
            CardNameTrigramSearchInquisitionArgs args = new()
            {
                Trigram = trigram,
                Partition = firstChar,
                Normalized = normalized
            };

            OpResponse<IEnumerable<CardNameTrigramExtEntity>> trigramResponse = await _cardNameTrigramSearchInquisition
                .QueryAsync<CardNameTrigramExtEntity>(args)
                .ConfigureAwait(false);

            if (trigramResponse.IsSuccessful() && trigramResponse.Value != null)
            {
                foreach (CardNameTrigramExtEntity trigramDoc in trigramResponse.Value)
                {
                    foreach (CardNameTrigramDataExtEntity entry in trigramDoc.Cards)
                    {
                        if (entry.Normalized.Contains(normalized) is false) continue;
                        _ = matchingCardNames.Add(entry.Name);

                        _ = cardNameMatchCounts.TryAdd(entry.Name, 0);
                        cardNameMatchCounts[entry.Name]++;
                    }
                }
            }
        }

        ICollection<string> sortedResults = [.. matchingCardNames
            .OrderByDescending(name => cardNameMatchCounts[name])
            .ThenBy(name => name)];

        return new SuccessOperationResponse<IEnumerable<string>>(sortedResults);
    }
}
