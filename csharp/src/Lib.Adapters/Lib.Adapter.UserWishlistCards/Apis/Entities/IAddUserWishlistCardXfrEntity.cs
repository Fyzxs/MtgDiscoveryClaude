using System.Collections.Generic;

namespace Lib.Adapter.UserWishlistCards.Apis.Entities;

public interface IAddUserWishlistCardXfrEntity
{
    string UserId { get; }
    string CardId { get; }
    string SetId { get; }
    string CardName { get; }
    string SetName { get; }
    string SetCode { get; }
    IEnumerable<string> ArtistIds { get; }
    string CardNameGuid { get; }
    IUserWishlistCardDetailsXfrEntity Details { get; }
}
