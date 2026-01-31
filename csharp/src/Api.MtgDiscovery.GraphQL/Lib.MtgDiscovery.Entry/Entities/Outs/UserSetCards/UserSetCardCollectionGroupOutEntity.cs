namespace Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;

public sealed class UserSetCardCollectionGroupOutEntity
{
    public string SetGroupId { get; init; }
    public UserSetCardGroupOutEntity Group { get; init; }
}
