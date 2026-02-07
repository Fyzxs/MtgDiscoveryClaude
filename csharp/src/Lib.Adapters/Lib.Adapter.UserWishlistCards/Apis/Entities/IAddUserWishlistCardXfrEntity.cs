using System.Collections.Generic;
using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.UserWishlistCards.Apis.Entities;

public interface IAddUserWishlistCardXfrEntity : IXfrEntity
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
