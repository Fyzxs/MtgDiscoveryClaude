using System.Collections.Generic;

namespace Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;

public sealed class UserSetCardOutEntity
{
    public string UserId { get; init; }
    public string SetId { get; init; }
    public int TotalCards { get; init; }
    public int UniqueCards { get; init; }
    public IReadOnlyCollection<UserSetCardRarityGroupOutEntity> Groups { get; init; }
    public IReadOnlyCollection<UserSetCardCollectingOutEntity> Collecting { get; init; }
}
