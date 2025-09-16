namespace Lib.Shared.DataModels.Entities;

public interface IAddCardToCollectionArgEntity
{
    string CardId { get; }
    string SetId { get; }
    ICollectedItemArgEntity CollectedItem { get; }
}

public interface ICollectedItemArgEntity
{
    string Finish { get; }
    string Special { get; }
    int Count { get; }
}
