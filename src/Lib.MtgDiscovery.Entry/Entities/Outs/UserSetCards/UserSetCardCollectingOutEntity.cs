namespace Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;

public sealed class UserSetCardCollectingOutEntity
{
    public string SetGroupId { get; init; }
    public bool Collecting { get; init; }
    public int Count { get; init; }
}
