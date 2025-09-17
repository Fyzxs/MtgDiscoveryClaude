namespace Lib.Shared.DataModels.Entities;

public interface IUserCardArgEntity
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
