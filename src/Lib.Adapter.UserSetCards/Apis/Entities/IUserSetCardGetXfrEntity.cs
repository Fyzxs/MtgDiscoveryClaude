namespace Lib.Adapter.UserSetCards.Apis.Entities;

/// <summary>
/// Transfer entity for getting user set card data from storage.
/// Contains the parameters needed to locate a specific user set card record.
/// </summary>
public interface IUserSetCardGetXfrEntity
{
    /// <summary>
    /// User identifier (used as partition key in Cosmos)
    /// </summary>
    string UserId { get; }

    /// <summary>
    /// Set identifier (used as document ID in Cosmos)
    /// </summary>
    string SetId { get; }
}
