namespace Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

/// <summary>
/// Internal transfer entity for adding a card to a user's set collection.
/// Used by migration tools to directly update UserSetCards aggregation.
/// </summary>
public interface IAddCardToSetItrEntity
{
    string UserId { get; }
    string SetId { get; }
    string CardId { get; }
    string SetGroupId { get; }
    string FinishType { get; }
    int Count { get; }
}
