using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Entities.Outs.Sets;

public sealed class SetInformationOutEntity
{
    public int TotalCards { get; init; }
    public int UniqueCards { get; init; }
    public IReadOnlyCollection<UserSetCardRarityGroupOutEntity> Groups { get; init; }
    public IReadOnlyCollection<UserSetCardCollectingOutEntity> Collecting { get; init; }
}
