using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.Cards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class CollectionCardItemOufToOutMapper : ICollectionCardItemOufToOutMapper
{
    private readonly ICardItemOufToOutMapper _mapper;

    public CollectionCardItemOufToOutMapper() : this(new CardItemOufToOutMapper())
    { }

    private CollectionCardItemOufToOutMapper(ICardItemOufToOutMapper mapper) => _mapper = mapper;

    public async Task<List<CardItemOutEntity>> Map(ICardItemCollectionOufEntity collection)
    {
        CardItemOutEntity[] mappedCards = await Task.WhenAll(
            collection.Data.Select(cardItem => _mapper.Map(cardItem))
        ).ConfigureAwait(false);

        return [.. mappedCards];
    }
}
