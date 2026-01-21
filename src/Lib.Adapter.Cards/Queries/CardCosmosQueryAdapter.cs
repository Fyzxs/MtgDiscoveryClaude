using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis;
using Lib.Adapter.Cards.Exceptions;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;
using Lib.Aggregator.Scryfall.Shared.Mappers;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Identifiers;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Cards.Queries;

/// <summary>
/// Cosmos DB implementation of the card query adapter.
/// 
/// This class handles all Cosmos DB-specific card query operations,
/// implementing the specialized ICardQueryAdapter interface.
/// The main CardAdapterService delegates to this implementation.
/// </summary>
internal sealed class CardCosmosQueryAdapter : ICardQueryAdapter
{
    private readonly ICosmosGopher _cardGopher;
    private readonly ICosmosGopher _setCodeIndexGopher;
    private readonly ICosmosInquisitor _setCardsInquisitor;
    private readonly ICosmosInquisitor _cardsByNameInquisitor;
    private readonly ICosmosInquisitor _cardNameTrigramsInquisitor;
    private readonly ScryfallCardItemToCardItemItrEntityMapper _cardMapper;

    public CardCosmosQueryAdapter(ILogger logger) : this(
        new ScryfallCardItemsGopher(logger),
        new ScryfallSetCodeIndexGopher(logger),
        new ScryfallSetCardsInquisitor(logger),
        new ScryfallCardsByNameInquisitor(logger),
        new CardNameTrigramsInquisitor(logger),
        new ScryfallCardItemToCardItemItrEntityMapper())
    { }

    private CardCosmosQueryAdapter(
        ICosmosGopher cardGopher,
        ICosmosGopher setCodeIndexGopher,
        ICosmosInquisitor setCardsInquisitor,
        ICosmosInquisitor cardsByNameInquisitor,
        ICosmosInquisitor cardNameTrigramsInquisitor,
        ScryfallCardItemToCardItemItrEntityMapper cardMapper)
    {
        _cardGopher = cardGopher;
        _setCodeIndexGopher = setCodeIndexGopher;
        _setCardsInquisitor = setCardsInquisitor;
        _cardsByNameInquisitor = cardsByNameInquisitor;
        _cardNameTrigramsInquisitor = cardNameTrigramsInquisitor;
        _cardMapper = cardMapper;
    }

    public async Task<IOperationResponse<IEnumerable<ICardItemItrEntity>>> GetCardsByIdsAsync([NotNull] ICardIdsItrEntity cardIds)
    {
        // Extract primitives for external system interface
        IEnumerable<string> cardIdList = cardIds.CardIds;
        List<Task<OpResponse<ScryfallCardExtArg>>> tasks = [];

        foreach (string cardId in cardIdList)
        {
            ReadPointItem readPoint = new()
            {
                Id = new ProvidedCosmosItemId(cardId),
                Partition = new ProvidedPartitionKeyValue(cardId)
            };
            tasks.Add(_cardGopher.ReadAsync<ScryfallCardExtArg>(readPoint));
        }

        OpResponse<ScryfallCardExtArg>[] responses = await Task.WhenAll(tasks).ConfigureAwait(false);

        IEnumerable<ICardItemItrEntity> successfulCards = responses
            .Where(r => r.IsSuccessful())
            .Select(r => _cardMapper.Map(r.Value))
            .Where(card => card != null);

        return new SuccessOperationResponse<IEnumerable<ICardItemItrEntity>>(successfulCards);
    }

    public async Task<IOperationResponse<IEnumerable<ICardItemItrEntity>>> GetCardsBySetCodeAsync(ISetCodeItrEntity setCode)
    {
        // Extract primitives for external system interface
        string setCodeValue = setCode.SetCode;
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(setCodeValue),
            Partition = new ProvidedPartitionKeyValue(setCodeValue)
        };

        OpResponse<ScryfallSetCodeIndexExtArg> indexResponse = await _setCodeIndexGopher
            .ReadAsync<ScryfallSetCodeIndexExtArg>(readPoint)
            .ConfigureAwait(false);

        if (indexResponse.IsSuccessful() is false || indexResponse.Value == null)
        {
            return new FailureOperationResponse<IEnumerable<ICardItemItrEntity>>(
                new CardAdapterException($"Set code '{setCodeValue}' not found"));
        }

        string setId = indexResponse.Value.SetId;

        QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.partition = @setId")
            .WithParameter("@setId", setId);

        OpResponse<IEnumerable<ScryfallSetCardItemExtArg>> cardsResponse = await _setCardsInquisitor
            .QueryAsync<ScryfallSetCardItemExtArg>(queryDefinition, new PartitionKey(setId))
            .ConfigureAwait(false);

        if (cardsResponse.IsSuccessful() is false)
        {
            return new FailureOperationResponse<IEnumerable<ICardItemItrEntity>>(
                new CardAdapterException($"Failed to retrieve cards for set '{setCodeValue}'", cardsResponse.Exception()));
        }

        List<ICardItemItrEntity> cards = [];
        foreach (ScryfallSetCardItemExtArg setCard in cardsResponse.Value)
        {
            ScryfallCardExtArg cardItem = new() { Data = setCard.Data };
            ICardItemItrEntity mappedCard = _cardMapper.Map(cardItem);
            if (mappedCard != null)
            {
                cards.Add(mappedCard);
            }
        }

        return new SuccessOperationResponse<IEnumerable<ICardItemItrEntity>>(cards);
    }

    public async Task<IOperationResponse<IEnumerable<ICardItemItrEntity>>> GetCardsByNameAsync(ICardNameItrEntity cardName)
    {
        // Extract primitives for external system interface
        string cardNameValue = cardName.CardName;
        ICardNameGuidGenerator guidGenerator = new CardNameGuidGenerator();
        CardNameGuid nameGuid = guidGenerator.GenerateGuid(cardNameValue);

        QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.partition = @nameGuid")
            .WithParameter("@nameGuid", nameGuid.AsSystemType().ToString());

        OpResponse<IEnumerable<ScryfallCardByNameExtArg>> cardsResponse = await _cardsByNameInquisitor
            .QueryAsync<ScryfallCardByNameExtArg>(queryDefinition, new PartitionKey(nameGuid.AsSystemType().ToString()))
            .ConfigureAwait(false);

        if (cardsResponse.IsSuccessful() is false)
        {
            return new FailureOperationResponse<IEnumerable<ICardItemItrEntity>>(
                new CardAdapterException($"Failed to retrieve cards for name '{cardNameValue}'", cardsResponse.Exception()));
        }

        List<ICardItemItrEntity> cards = [];
        foreach (ScryfallCardByNameExtArg cardByName in cardsResponse.Value)
        {
            ScryfallCardExtArg cardItem = new() { Data = cardByName.Data };
            ICardItemItrEntity mappedCard = _cardMapper.Map(cardItem);
            if (mappedCard != null)
            {
                cards.Add(mappedCard);
            }
        }

        return new SuccessOperationResponse<IEnumerable<ICardItemItrEntity>>(cards);
    }

    public async Task<IOperationResponse<IEnumerable<string>>> SearchCardNamesAsync([NotNull] ICardSearchTermItrEntity searchTerm)
    {
        // Extract primitives for external system interface
        string searchTermValue = searchTerm.SearchTerm;

        string normalized = new([.. searchTermValue
            .ToLowerInvariant()
            .Where(char.IsLetter)]);

        if (normalized.Length < 3)
        {
            return new FailureOperationResponse<IEnumerable<string>>(new CardAdapterException("Search term must contain at least 3 letters"));
        }

        List<string> trigrams = [];
        for (int i = 0; i <= normalized.Length - 3; i++)
        {
            trigrams.Add(normalized.Substring(i, 3));
        }

        HashSet<string> matchingCardNames = [];
        Dictionary<string, int> cardNameMatchCounts = [];

        foreach (string trigram in trigrams)
        {
            string firstChar = trigram[..1];
            QueryDefinition queryDefinition = new QueryDefinition(
                "SELECT * FROM c WHERE c.id = @trigram AND c.partition = @partition AND EXISTS(SELECT VALUE card FROM card IN c.cards WHERE CONTAINS(card.norm, @normalized))")
                .WithParameter("@trigram", trigram)
                .WithParameter("@partition", firstChar)
                .WithParameter("@normalized", normalized);

            OpResponse<IEnumerable<CardNameTrigramExtArg>> trigramResponse = await _cardNameTrigramsInquisitor
                .QueryAsync<CardNameTrigramExtArg>(queryDefinition, new PartitionKey(firstChar))
                .ConfigureAwait(false);

            if (trigramResponse.IsSuccessful() && trigramResponse.Value != null)
            {
                foreach (CardNameTrigramExtArg trigramDoc in trigramResponse.Value)
                {
                    foreach (CardNameTrigramDataExtArg entry in trigramDoc.Cards)
                    {
                        if (entry.Normalized.Contains(normalized) is false) continue;
                        matchingCardNames.Add(entry.Name);

                        cardNameMatchCounts.TryAdd(entry.Name, 0);
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
