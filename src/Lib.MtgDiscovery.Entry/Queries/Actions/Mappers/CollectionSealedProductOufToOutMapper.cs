using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class CollectionSealedProductOufToOutMapper : ICollectionSealedProductOufToOutMapper
{
    private readonly ISealedProductOufToOutMapper _mapper;

    public CollectionSealedProductOufToOutMapper() : this(new SealedProductOufToOutMapper())
    { }

    private CollectionSealedProductOufToOutMapper(ISealedProductOufToOutMapper mapper) => _mapper = mapper;

    public async Task<List<SealedProductOutEntity>> Map(IEnumerable<ISealedProductOufEntity> collection)
    {
        SealedProductOutEntity[] mappedProducts = await Task.WhenAll(
            collection.Select(product => _mapper.Map(product))
        ).ConfigureAwait(false);

        return [.. mappedProducts];
    }
}
