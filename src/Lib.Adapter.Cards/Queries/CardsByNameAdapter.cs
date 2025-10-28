using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Adapter.Cards.Exceptions;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;
using Lib.Cosmos.Apis.Operators;
using Lib.Scryfall.Shared.Entities;
using Lib.Shared.Abstractions.Identifiers;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Cards.Queries;

/// <summary>
/// Retrieves cards by exact name using name GUID partitioning.
/// </summary>
internal sealed class CardsByNameAdapter : ICardsByNameAdapter
{
    private readonly ICosmosInquisition<CardsByNameGuidInquisitionArgs> _cardsByNameGuidInquisition;

    public CardsByNameAdapter(ILogger logger) : this(new CardsByNameGuidInquisition(logger)) { }

    private CardsByNameAdapter(ICosmosInquisition<CardsByNameGuidInquisitionArgs> cardsByNameGuidInquisition) =>
        _cardsByNameGuidInquisition = cardsByNameGuidInquisition;

    public async Task<IOperationResponse<IEnumerable<ScryfallCardByNameExtEntity>>> Execute(ICardNameXfrEntity input)
    {
        string cardNameValue = input.CardName;
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
}
