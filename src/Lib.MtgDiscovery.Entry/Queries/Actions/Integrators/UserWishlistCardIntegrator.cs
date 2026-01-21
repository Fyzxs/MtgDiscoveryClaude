using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserWishlistCards;
using Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Integrators;

internal sealed class UserWishlistCardIntegrator : IUserWishlistCardIntegrator
{
    private readonly ICollectionUserWishlistCardDetailsOufToOutMapper _wishlistCardDetailsOufToOutMapper;

    public UserWishlistCardIntegrator() : this(new CollectionUserWishlistCardDetailsOufToOutMapper()) { }

    private UserWishlistCardIntegrator(ICollectionUserWishlistCardDetailsOufToOutMapper wishlistCardDetailsOufToOutMapper) =>
        _wishlistCardDetailsOufToOutMapper = wishlistCardDetailsOufToOutMapper;

    public async Task<List<CardItemOutEntity>> Integrate(List<CardItemOutEntity> current, IEnumerable<IUserWishlistCardOufEntity> change)
    {
        Dictionary<string, Task<ICollection<WishlistItemOutEntity>>> dictionary = change.ToDictionary(
            uwc => uwc.CardId,
            uwc => _wishlistCardDetailsOufToOutMapper.Map(uwc.WishlistItems)
        );

        foreach (CardItemOutEntity card in current)
        {
            if (dictionary.TryGetValue(card.Id, out Task<ICollection<WishlistItemOutEntity>> wishlistItems) is false) continue;
            card.UserWishlist = await wishlistItems.ConfigureAwait(false);
        }

        return current;
    }
}
