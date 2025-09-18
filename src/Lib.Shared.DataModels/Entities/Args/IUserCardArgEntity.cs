namespace Lib.Shared.DataModels.Entities.Args;

public interface IAddUserCardArgEntity
{
    string CardId { get; }
    string SetId { get; }
    IUserCardDetailsArgEntity UserCardDetails { get; }
}

public interface IUserCardDetailsArgEntity
{
    string Finish { get; }
    string Special { get; }
    int Count { get; }
}
