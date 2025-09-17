using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis;
using Lib.Adapter.Cards.Exceptions;
using Lib.Adapter.Cards.Queries.Mappers;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;
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
    private readonly ICollectionCardIdToReadPointItemMapper _cardIdsToReadPointMapper;

    public CardCosmosQueryAdapter(ILogger logger) : this(
        new ScryfallCardItemsGopher(logger),
        new ScryfallSetCodeIndexGopher(logger),
        new ScryfallSetCardsInquisitor(logger),
        new ScryfallCardsByNameInquisitor(logger),
        new CardNameTrigramsInquisitor(logger),
        new CollectionCardIdToReadPointItemMapper())
    { }

    private CardCosmosQueryAdapter(
        ICosmosGopher cardGopher,
        ICosmosGopher setCodeIndexGopher,
        ICosmosInquisitor setCardsInquisitor,
        ICosmosInquisitor cardsByNameInquisitor,
        ICosmosInquisitor cardNameTrigramsInquisitor,
        ICollectionCardIdToReadPointItemMapper cardIdsToReadPointMapper)
    {
        _cardGopher = cardGopher;
        _setCodeIndexGopher = setCodeIndexGopher;
        _setCardsInquisitor = setCardsInquisitor;
        _cardsByNameInquisitor = cardsByNameInquisitor;
        _cardNameTrigramsInquisitor = cardNameTrigramsInquisitor;
        _cardIdsToReadPointMapper = cardIdsToReadPointMapper;
    }

    public async Task<IOperationResponse<IEnumerable<ScryfallCardItemExtEntity>>> GetCardsByIdsAsync([NotNull] ICardIdsItrEntity cardIds)
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

    public async Task<IOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>>> GetCardsBySetCodeAsync(ISetCodeItrEntity setCode)
    {
        // Extract primitives for external system interface
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

        QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.partition = @setId")
            .WithParameter("@setId", setId);

        OpResponse<IEnumerable<ScryfallSetCardItemExtEntity>> cardsResponse = await _setCardsInquisitor
            .QueryAsync<ScryfallSetCardItemExtEntity>(queryDefinition, new PartitionKey(setId))
            .ConfigureAwait(false);

        if (cardsResponse.IsSuccessful() is false)
        {
            return new FailureOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>>(
                new CardAdapterException($"Failed to retrieve cards for set '{setCodeValue}'", cardsResponse.Exception()));
        }

        return new SuccessOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>>(cardsResponse.Value);
    }

    public async Task<IOperationResponse<IEnumerable<ScryfallCardByNameExtEntity>>> GetCardsByNameAsync(ICardNameItrEntity cardName)
    {
        // Extract primitives for external system interface
        string cardNameValue = cardName.CardName;
        ICardNameGuidGenerator guidGenerator = new CardNameGuidGenerator();
        CardNameGuid nameGuid = guidGenerator.GenerateGuid(cardNameValue);

        QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.partition = @nameGuid")
            .WithParameter("@nameGuid", nameGuid.AsSystemType().ToString());

        OpResponse<IEnumerable<ScryfallCardByNameExtEntity>> cardsResponse = await _cardsByNameInquisitor
            .QueryAsync<ScryfallCardByNameExtEntity>(queryDefinition, new PartitionKey(nameGuid.AsSystemType().ToString()))
            .ConfigureAwait(false);

        if (cardsResponse.IsSuccessful() is false)
        {
            return new FailureOperationResponse<IEnumerable<ScryfallCardByNameExtEntity>>(
                new CardAdapterException($"Failed to retrieve cards for name '{cardNameValue}'", cardsResponse.Exception()));
        }

        return new SuccessOperationResponse<IEnumerable<ScryfallCardByNameExtEntity>>(cardsResponse.Value);
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

            OpResponse<IEnumerable<CardNameTrigramExtEntity>> trigramResponse = await _cardNameTrigramsInquisitor
                .QueryAsync<CardNameTrigramExtEntity>(queryDefinition, new PartitionKey(firstChar))
                .ConfigureAwait(false);

            if (trigramResponse.IsSuccessful() && trigramResponse.Value != null)
            {
                foreach (CardNameTrigramExtEntity trigramDoc in trigramResponse.Value)
                {
                    foreach (CardNameTrigramDataExtEntity entry in trigramDoc.Cards)
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
