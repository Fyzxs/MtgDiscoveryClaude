using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Adapter.Cards.Exceptions;
using Lib.Adapter.Cards.Queries.Mappers;
using Lib.Scryfall.Shared.Entities;
using Lib.Scryfall.Shared.Mappers;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Identifiers;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Cards.Queries;

/// <summary>
/// Cosmos DB implementation of the card query adapter.
/// 
/// This class handles all Cosmos DB-specific card query operations,
/// implementing the specialized ICardQueryAdapter interface.
/// The main CardAdapterService delegates to this implementation.
/// </summary>
internal sealed class CardsQueryAdapter : ICardQueryAdapter
{
    private readonly ICosmosGopher _cardGopher;
    private readonly ICosmosGopher _setCodeIndexGopher;
    private readonly ICosmosInquisition<CardsBySetIdInquisitionArgs> _cardsBySetIdInquisition;
    private readonly ICosmosInquisition<CardsByNameGuidInquisitionArgs> _cardsByNameGuidInquisition;
    private readonly ICosmosInquisition<CardNameTrigramSearchInquisitionArgs> _cardNameTrigramSearchInquisition;
    private readonly ICollectionCardIdToReadPointItemMapper _cardIdsToReadPointMapper;
    private readonly ISearchTermToTrigramsMapper _trigramMapper;

    public CardsQueryAdapter(ILogger logger) : this(
        new ScryfallCardItemsGopher(logger),
        new ScryfallSetCodeIndexGopher(logger),
        new CardsBySetIdInquisition(logger),
        new CardsByNameGuidInquisition(logger),
        new CardNameTrigramSearchInquisition(logger),
        new CollectionCardIdToReadPointItemMapper(),
        new SearchTermToTrigramsMapper())
    { }

    private CardsQueryAdapter(
        ICosmosGopher cardGopher,
        ICosmosGopher setCodeIndexGopher,
        ICosmosInquisition<CardsBySetIdInquisitionArgs> cardsBySetIdInquisition,
        ICosmosInquisition<CardsByNameGuidInquisitionArgs> cardsByNameGuidInquisition,
        ICosmosInquisition<CardNameTrigramSearchInquisitionArgs> cardNameTrigramSearchInquisition,
        ICollectionCardIdToReadPointItemMapper cardIdsToReadPointMapper,
        ISearchTermToTrigramsMapper trigramMapper)
    {
        _cardGopher = cardGopher;
        _setCodeIndexGopher = setCodeIndexGopher;
        _cardsBySetIdInquisition = cardsBySetIdInquisition;
        _cardsByNameGuidInquisition = cardsByNameGuidInquisition;
        _cardNameTrigramSearchInquisition = cardNameTrigramSearchInquisition;
        _cardIdsToReadPointMapper = cardIdsToReadPointMapper;
        _trigramMapper = trigramMapper;
    }

    public async Task<IOperationResponse<IEnumerable<ScryfallCardItemExtEntity>>> GetCardsByIdsAsync([NotNull] ICardIdsXfrEntity cardIds)
    {
        ICollection<ReadPointItem> items = await _cardIdsToReadPointMapper.Map(cardIds.CardIds).ConfigureAwait(false);
        IEnumerable<Task<OpResponse<ScryfallCardItemExtEntity>>> collection = items.Select(readPointItem => _cardGopher.ReadAsync<ScryfallCardItemExtEntity>(readPointItem));

        OpResponse<ScryfallCardItemExtEntity>[] responses = await Task.WhenAll(collection).ConfigureAwait(false);

        IEnumerable<ScryfallCardItemExtEntity> successfulCards = responses
            .Where(task => task.IsSuccessful())
            .Select(task => task.Value)
            .Where(card => card is not null);

        return new SuccessOperationResponse<IEnumerable<ScryfallCardItemExtEntity>>(successfulCards);
    }

    public async Task<IOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>>> GetCardsBySetCodeAsync(ISetCodeXfrEntity setCode)
    {
        string setCodeValue = setCode.SetCode;
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(setCodeValue),
            Partition = new ProvidedPartitionKeyValue(setCodeValue)
        };

        OpResponse<ScryfallSetCodeIndexExtEntity> indexResponse = await _setCodeIndexGopher
            .ReadAsync<ScryfallSetCodeIndexExtEntity>(readPoint)
            .ConfigureAwait(false);

        if (indexResponse.IsSuccessful() is false || indexResponse.Value == null)
        {
            return new FailureOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>>(
                new CardAdapterException($"Set code '{setCodeValue}' not found"));
        }

        string setId = indexResponse.Value.SetId;

        CardsBySetIdInquisitionArgs args = new() { SetId = setId };

        OpResponse<IEnumerable<ScryfallSetCardItemExtEntity>> cardsResponse = await _cardsBySetIdInquisition
            .QueryAsync<ScryfallSetCardItemExtEntity>(args)
            .ConfigureAwait(false);

        if (cardsResponse.IsSuccessful() is false)
        {
            return new FailureOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>>(
                new CardAdapterException($"Failed to retrieve cards for set '{setCodeValue}'", cardsResponse.Exception()));
        }

        return new SuccessOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>>(cardsResponse.Value);
    }

    public async Task<IOperationResponse<IEnumerable<ScryfallCardByNameExtEntity>>> GetCardsByNameAsync(ICardNameXfrEntity cardName)
    {
        string cardNameValue = cardName.CardName;
        ICardNameGuidGenerator guidGenerator = new CardNameGuidGenerator();
        CardNameGuid nameGuid = guidGenerator.GenerateGuid(cardNameValue);

        CardsByNameGuidInquisitionArgs args = new() { NameGuid = nameGuid.AsSystemType().ToString() };

        OpResponse<IEnumerable<ScryfallCardByNameExtEntity>> cardsResponse = await _cardsByNameGuidInquisition
            .QueryAsync<ScryfallCardByNameExtEntity>(args)
            .ConfigureAwait(false);

        if (cardsResponse.IsSuccessful() is false)
        {
            return new FailureOperationResponse<IEnumerable<ScryfallCardByNameExtEntity>>(
                new CardAdapterException($"Failed to retrieve cards for name '{cardNameValue}'", cardsResponse.Exception()));
        }

        return new SuccessOperationResponse<IEnumerable<ScryfallCardByNameExtEntity>>(cardsResponse.Value);
    }

    public async Task<IOperationResponse<IEnumerable<string>>> SearchCardNamesAsync([NotNull] ICardSearchTermXfrEntity searchTerm)
    {
        // Extract primitives for external system interface
        string searchTermValue = searchTerm.SearchTerm;

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
