namespace Lib.Shared.DataModels.Entities.Args;

public interface IAddUserCardArgEntity
{
    string CardId { get; }
    string SetId { get; }
    string UserId { get; }
    IUserCardDetailsArgEntity UserCardDetails { get; }
}
