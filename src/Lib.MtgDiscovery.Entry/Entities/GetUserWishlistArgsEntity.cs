namespace Lib.MtgDiscovery.Entry.Entities;

public sealed class GetUserWishlistArgsEntity : IGetUserWishlistArgsEntity
{
    public required string TargetUserId { get; init; }
}
