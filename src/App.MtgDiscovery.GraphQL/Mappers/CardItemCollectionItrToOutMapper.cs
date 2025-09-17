using System.Collections.Generic;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal sealed class CardItemCollectionItrToOutMapper : ICardItemCollectionItrToOutMapper
{
    private readonly ICardItemItrToOutMapper _mapper;

    public CardItemCollectionItrToOutMapper() : this(new CardItemItrToOutMapper())
    { }

    private CardItemCollectionItrToOutMapper(ICardItemItrToOutMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<List<CardItemOutEntity>> Map(IEnumerable<ICardItemItrEntity> cardItems)
    {
        List<CardItemOutEntity> results = [];

        foreach (ICardItemItrEntity cardItem in cardItems)
        {
            CardItemOutEntity outEntity = await _mapper.Map(cardItem).ConfigureAwait(false);
            results.Add(outEntity);
        }

        return results;
    }
}
