using System.Collections.Generic;

namespace Lib.MtgDiscovery.Entry.Entities.Outs.UserWishlistCards;

public sealed class UserWishlistCardOutEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public string CardName { get; init; }
    public string SetName { get; init; }
    public string SetCode { get; init; }
    public IEnumerable<string> ArtistIds { get; init; } = [];
    public string CardNameGuid { get; init; }
    public string CreatedAt { get; init; }
    public string UpdatedAt { get; init; }
    public ICollection<WishlistItemOutEntity> WishlistItems { get; init; }
}
