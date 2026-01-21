using System.Collections.Generic;

namespace Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;

public sealed class UserSetCardFinishGroupOutEntity
{
    public IReadOnlyCollection<string> Cards { get; init; }
}
