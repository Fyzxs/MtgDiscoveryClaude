using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class UserWishlistCardOufToOutMapper : IUserWishlistCardOufToOutMapper
{
    private readonly IUserWishlistCardDetailsOufToOutMapper _mapper;

    public UserWishlistCardOufToOutMapper() : this(new UserWishlistCardDetailsOufToOutMapper()) { }

    internal UserWishlistCardOufToOutMapper(IUserWishlistCardDetailsOufToOutMapper mapper) => _mapper = mapper;

    public async Task<UserWishlistCardOutEntity> Map(IUserWishlistCardOufEntity source)
    {
        WishlistItemOutEntity[] mappedDetails = await Task.WhenAll(
            source.WishlistItems.Select(detail => _mapper.Map(detail))
        ).ConfigureAwait(false);

        UserWishlistCardOutEntity result = new()
        {
            UserId = source.UserId,
            CardId = source.CardId,
            SetId = source.SetId,
            CardName = source.CardName,
            SetName = source.SetName,
            SetCode = source.SetCode,
            ArtistIds = source.ArtistIds,
            CardNameGuid = source.CardNameGuid,
            CreatedAt = source.CreatedAt,
            UpdatedAt = source.UpdatedAt,
            WishlistItems = [.. mappedDetails]
        };

        return result;
    }
}
