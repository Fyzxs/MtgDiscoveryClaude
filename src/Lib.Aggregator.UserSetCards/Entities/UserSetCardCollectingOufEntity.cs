namespace Lib.Aggregator.UserSetCards.Entities;

internal sealed class UserSetCardCollectingOufEntity : IUserSetCardCollectingOufEntity
{
    public string SetGroupId { get; init; }
    public bool Collecting { get; init; }
    public int Count { get; init; }
}
