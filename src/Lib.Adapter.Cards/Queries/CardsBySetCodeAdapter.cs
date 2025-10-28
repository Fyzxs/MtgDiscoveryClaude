using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Adapter.Cards.Exceptions;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Cards.Queries;

/// <summary>
/// Retrieves cards for a specific set using set code index lookup.
/// </summary>
internal sealed class CardsBySetCodeAdapter : ICardsBySetCodeAdapter
{
    private readonly ICosmosGopher _setCodeIndexGopher;
    private readonly ICosmosInquisition<CardsBySetIdInquisitionArgs> _cardsBySetIdInquisition;

    public CardsBySetCodeAdapter(ILogger logger) : this(
        new ScryfallSetCodeIndexGopher(logger),
        new CardsBySetIdInquisition(logger))
    { }

    private CardsBySetCodeAdapter(
        ICosmosGopher setCodeIndexGopher,
        ICosmosInquisition<CardsBySetIdInquisitionArgs> cardsBySetIdInquisition)
    {
        _setCodeIndexGopher = setCodeIndexGopher;
        _cardsBySetIdInquisition = cardsBySetIdInquisition;
    }

    public async Task<IOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>>> Execute(ISetCodeXfrEntity input)
    {
        string setCodeValue = input.SetCode;
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
}
