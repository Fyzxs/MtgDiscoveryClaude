using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Aggregator.UserWishlistCards.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;

namespace Lib.Aggregator.UserWishlistCards.Commands.Mappers;

internal sealed class UserWishlistCardExtToOufEntityMapper
    : ChildCollectionMapper<UserWishlistCardDetailsExtEntity, IUserWishlistCardDetailsOufEntity>,
      IUserWishlistCardExtToOufEntityMapper
{
    public UserWishlistCardExtToOufEntityMapper() : this(new UserWishlistCardDetailsExtToOufMapper()) { }

    internal UserWishlistCardExtToOufEntityMapper(IUserWishlistCardDetailsExtToOufMapper mapper) : base(mapper) { }

    public async Task<IUserWishlistCardOufEntity> Map([NotNull] UserWishlistCardExtEntity source)
    {
        IUserWishlistCardDetailsOufEntity[] mappedDetails = await MapChildren(source.WishlistItems).ConfigureAwait(false);

        return new UserWishlistCardOufEntity
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
            WishlistItems = mappedDetails
        };
    }
}
