using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Aggregator.UserWishlistCards.Entities;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;

namespace Lib.Aggregator.UserWishlistCards.Commands.Mappers;

/// <summary>
/// Maps UserWishlistCardExtEntity to IUserWishlistCardOufEntity.
/// </summary>
internal sealed class UserWishlistCardExtToOufEntityMapper : IUserWishlistCardExtToOufEntityMapper
{
    private readonly IUserWishlistCardDetailsExtToOufMapper _mapper;

    public UserWishlistCardExtToOufEntityMapper() : this(new UserWishlistCardDetailsExtToOufMapper())
    { }

    internal UserWishlistCardExtToOufEntityMapper(IUserWishlistCardDetailsExtToOufMapper mapper) => _mapper = mapper;

    public async Task<IUserWishlistCardOufEntity> Map([NotNull] UserWishlistCardExtEntity source)
    {
        IUserWishlistCardDetailsOufEntity[] mappedDetails = await Task.WhenAll(
            source.WishlistItems.Select(detail => _mapper.Map(detail))
        ).ConfigureAwait(false);

        return new UserWishlistCardOufEntity
        {
            UserId = source.UserId,
            CardId = source.CardId,
            SetId = source.SetId,
            WishlistItems = mappedDetails
        };
    }
}
