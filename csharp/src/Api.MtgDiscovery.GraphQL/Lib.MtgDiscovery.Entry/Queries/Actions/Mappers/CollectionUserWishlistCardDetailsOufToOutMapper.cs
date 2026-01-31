using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class CollectionUserWishlistCardDetailsOufToOutMapper : ICollectionUserWishlistCardDetailsOufToOutMapper
{
    private readonly IUserWishlistCardDetailsOufToOutMapper _mapper;

    public CollectionUserWishlistCardDetailsOufToOutMapper() : this(new UserWishlistCardDetailsOufToOutMapper())
    { }
    private CollectionUserWishlistCardDetailsOufToOutMapper(IUserWishlistCardDetailsOufToOutMapper mapper) => _mapper = mapper;

    public async Task<ICollection<WishlistItemOutEntity>> Map(IEnumerable<IUserWishlistCardDetailsOufEntity> source)
    {
        WishlistItemOutEntity[] wishlistItemOutEntities = await Task.WhenAll(
            source.Select(x => _mapper.Map(x))
        ).ConfigureAwait(false);
        return wishlistItemOutEntities;
    }
}
