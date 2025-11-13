namespace Lib.Shared.DataModels.Entities.Args.UserSetCards;

/// <summary>
/// Argument entity for adding a card to a user's set collection.
/// Used by migration tools to directly update UserSetCards.
/// </summary>
public interface IAddCardToSetArgEntity
{
    string UserId { get; }
    string SetId { get; }
    string CardId { get; }
    string SetGroupId { get; }
    string FinishType { get; }
    int Count { get; }
}
