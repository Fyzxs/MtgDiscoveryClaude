using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class CollectionUserSealedProductItrToOutMapper : ICollectionUserSealedProductItrToOutMapper
{
    private readonly IUserSealedProductItrToOutMapper _mapper;

    public CollectionUserSealedProductItrToOutMapper() : this(new UserSealedProductItrToOutMapper()) { }

    internal CollectionUserSealedProductItrToOutMapper(IUserSealedProductItrToOutMapper mapper) => _mapper = mapper;

    public async Task<List<UserSealedProductOutEntity>> Map(IEnumerable<IUserSealedProductOufEntity> source)
    {
        UserSealedProductOutEntity[] mappedResults = await Task.WhenAll(
            source.Select(item => _mapper.Map(item))
        ).ConfigureAwait(false);

        return [.. mappedResults];
    }
}
