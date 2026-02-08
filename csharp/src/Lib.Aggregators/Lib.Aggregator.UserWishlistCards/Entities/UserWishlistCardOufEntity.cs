using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;

namespace Lib.Aggregator.UserWishlistCards.Entities;

internal sealed class UserWishlistCardOufEntity : IUserWishlistCardOufEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public string CardName { get; init; }
    public string SetName { get; init; }
    public string SetCode { get; init; }
    public IEnumerable<string> ArtistIds { get; init; }
    public string CardNameGuid { get; init; }
    public string CreatedAt { get; init; }
    public string UpdatedAt { get; init; }
    public ICollection<IUserWishlistCardDetailsOufEntity> WishlistItems { get; init; }
}
