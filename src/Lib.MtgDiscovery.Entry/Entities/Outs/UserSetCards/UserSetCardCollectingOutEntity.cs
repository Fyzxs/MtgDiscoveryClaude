using System.Collections.Generic;

namespace Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;

public sealed class UserSetCardCollectingOutEntity
{
    public string SetGroupId { get; init; }
    public bool Collecting { get; init; }
    public FinishCountsOutEntity Counts { get; init; }
    public IReadOnlyCollection<string> CollectingFinishes { get; init; }
}
