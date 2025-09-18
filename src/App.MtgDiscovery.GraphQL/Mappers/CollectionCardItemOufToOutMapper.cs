using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Itrs;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal sealed class CollectionCardItemOufToOutMapper : ICollectionCardItemOufToOutMapper
{
    private readonly ICardItemOufToOutMapper _mapper;

    public CollectionCardItemOufToOutMapper() : this(new CardItemOufToOutMapper())
    { }

    private CollectionCardItemOufToOutMapper(ICardItemOufToOutMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<ICollection<CardItemOutEntity>> Map(IEnumerable<ICardItemItrEntity> cardItems)
    {
        CardItemOutEntity[] mappedCards = await Task.WhenAll(
            cardItems.Select(cardItem => _mapper.Map(cardItem))
        ).ConfigureAwait(false);

        return mappedCards;
    }
}
