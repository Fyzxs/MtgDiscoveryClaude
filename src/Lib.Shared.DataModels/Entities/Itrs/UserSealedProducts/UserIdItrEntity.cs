namespace Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;

/// <summary>
/// Internal transfer entity wrapping a user identifier.
/// Used for query operations that filter by user ID.
/// </summary>
public sealed class UserIdItrEntity : IUserIdItrEntity
{
    public string UserId { get; init; }
}
