namespace Lib.Aggregator.UserSetCards.Entities;

public interface IUserSetCardCollectingOufEntity
{
    string SetGroupId { get; }
    bool Collecting { get; }
    int Count { get; }
}
