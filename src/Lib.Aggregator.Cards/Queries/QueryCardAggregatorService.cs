using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Aggregator.Cards.Apis;
using Lib.Aggregator.Cards.Entities;
using Lib.Aggregator.Cards.Exceptions;
using Lib.Aggregator.Cards.Queries.Mappers;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Identifiers;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Cards.Queries;

internal sealed class QueryCardAggregatorService : ICardAggregatorService
{
    private readonly ICosmosGopher _cardGopher;
    private readonly ICosmosGopher _setCodeIndexGopher;
    private readonly ICosmosInquisitor _setCardsInquisitor;
    private readonly ICosmosInquisitor _cardsByNameInquisitor;
    private readonly QueryCardsIdsToReadPointItemsMapper _mapper;
    private readonly ScryfallCardItemToCardItemItrEntityMapper _cardMapper;

    public QueryCardAggregatorService(ILogger logger) : this(
        new ScryfallCardItemsGopher(logger),
        new ScryfallSetCodeIndexGopher(logger),
        new ScryfallSetCardsInquisitor(logger),
        new ScryfallCardsByNameInquisitor(logger),
        new QueryCardsIdsToReadPointItemsMapper(),
        new ScryfallCardItemToCardItemItrEntityMapper())
    {
    }

    private QueryCardAggregatorService(
        ICosmosGopher cardGopher,
        ICosmosGopher setCodeIndexGopher,
        ICosmosInquisitor setCardsInquisitor,
        ICosmosInquisitor cardsByNameInquisitor,
        QueryCardsIdsToReadPointItemsMapper mapper,
        ScryfallCardItemToCardItemItrEntityMapper cardMapper)
    {
        _cardGopher = cardGopher;
        _setCodeIndexGopher = setCodeIndexGopher;
        _setCardsInquisitor = setCardsInquisitor;
        _cardsByNameInquisitor = cardsByNameInquisitor;
        _mapper = mapper;
        _cardMapper = cardMapper;
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsItrEntity args)
    {
        IEnumerable<ReadPointItem> readPointItems = _mapper.Map(args);
        List<Task<OpResponse<ScryfallCardItem>>> tasks = [];

        tasks.AddRange(readPointItems.Select(readPointItem => _cardGopher.ReadAsync<ScryfallCardItem>(readPointItem)));

        OpResponse<ScryfallCardItem>[] responses = await Task.WhenAll(tasks).ConfigureAwait(false);

        ICollection<ICardItemItrEntity> successfulCards = responses
            .Where(r => r.IsSuccessful())
            .Select(r => _cardMapper.Map(r.Value))
            .Where(card => card != null)
            .ToList();

        return new SuccessOperationResponse<ICardItemCollectionItrEntity>(new CardItemCollectionItrEntity { Data = successfulCards });
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsBySetCodeAsync(ISetCodeItrEntity setCode)
    {
        //TODO: The inquisitor pattern and coupling with the query needs to be established.
        // First, get the set ID from the set code index
        ReadPointItem readPoint = new() { Id = new ProvidedCosmosItemId(setCode.SetCode), Partition = new ProvidedPartitionKeyValue(setCode.SetCode) };
        OpResponse<ScryfallSetCodeIndexItem> indexResponse = await _setCodeIndexGopher.ReadAsync<ScryfallSetCodeIndexItem>(readPoint).ConfigureAwait(false);

        if (indexResponse.IsSuccessful() is false || indexResponse.Value == null)
        {
            return new FailureOperationResponse<ICardItemCollectionItrEntity>(
                new CardAggregatorOperationException($"Set code '{setCode.SetCode}' not found"));
        }

        string setId = indexResponse.Value.SetId;

        // Query all cards for this set ID
        QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.partition = @setId")
            .WithParameter("@setId", setId);

        OpResponse<IEnumerable<ScryfallSetCard>> cardsResponse = await _setCardsInquisitor.QueryAsync<ScryfallSetCard>(
            queryDefinition,
            new PartitionKey(setId)).ConfigureAwait(false);

        if (cardsResponse.IsSuccessful() is false)
        {
            return new FailureOperationResponse<ICardItemCollectionItrEntity>(
                new CardAggregatorOperationException($"Failed to retrieve cards for set '{setCode.SetCode}'", cardsResponse.Exception()));
        }

        // Convert ScryfallSetCard to ScryfallCardItem for mapping
        List<ICardItemItrEntity> cards = [];
        foreach (ScryfallSetCard setCard in cardsResponse.Value)
        {
            ScryfallCardItem cardItem = new() { Scryfall = setCard.Scryfall ?? setCard.Data };
            ICardItemItrEntity mappedCard = _cardMapper.Map(cardItem);
            if (mappedCard != null)
            {
                cards.Add(mappedCard);
            }
        }

        return new SuccessOperationResponse<ICardItemCollectionItrEntity>(new CardItemCollectionItrEntity { Data = cards });
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByNameAsync(ICardNameItrEntity cardName)
    {
        // Generate the GUID for the card name
        ICardNameGuidGenerator guidGenerator = new CardNameGuidGenerator();
        CardNameGuid nameGuid = guidGenerator.GenerateGuid(cardName.CardName);
        // Query all cards with this name GUID as partition key
        QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.partition = @nameGuid")
            .WithParameter("@nameGuid", nameGuid.AsSystemType().ToString());

        OpResponse<IEnumerable<ScryfallCardByName>> cardsResponse = await _cardsByNameInquisitor.QueryAsync<ScryfallCardByName>(
            queryDefinition,
            new PartitionKey(nameGuid.AsSystemType().ToString())).ConfigureAwait(false);

        if (cardsResponse.IsSuccessful() is false)
        {
            return new FailureOperationResponse<ICardItemCollectionItrEntity>(
                new CardAggregatorOperationException($"Failed to retrieve cards for name '{cardName.CardName}'", cardsResponse.Exception()));
        }

        // Convert ScryfallCardByName to ScryfallCardItem for mapping
        List<ICardItemItrEntity> cards = [];
        foreach (ScryfallCardByName cardByName in cardsResponse.Value)
        {
            ScryfallCardItem cardItem = new() { Scryfall = cardByName.Scryfall ?? cardByName.Data };
            ICardItemItrEntity mappedCard = _cardMapper.Map(cardItem);
            if (mappedCard != null)
            {
                cards.Add(mappedCard);
            }
        }

        return new SuccessOperationResponse<ICardItemCollectionItrEntity>(new CardItemCollectionItrEntity { Data = cards });
    }
}
