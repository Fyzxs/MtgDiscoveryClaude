namespace Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;

/// <summary>
/// Internal transfer entity wrapping a user identifier.
/// Used for query operations that filter by user ID.
/// Data flow: Entry → Domain → Aggregator layers
/// </summary>
public interface IUserIdItrEntity : Abstractions.IItrEntity
{
    /// <summary>
    /// The unique identifier of the user.
    /// </summary>
    string UserId { get; }
}
