using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Itrs;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal sealed class CollectionCardItemItrToOutMapper : ICollectionCardItemItrToOutMapper
{
    private readonly ICardItemItrToOutMapper _mapper;

    public CollectionCardItemItrToOutMapper() : this(new CardItemItrToOutMapper())
    { }

    private CollectionCardItemItrToOutMapper(ICardItemItrToOutMapper mapper)
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
