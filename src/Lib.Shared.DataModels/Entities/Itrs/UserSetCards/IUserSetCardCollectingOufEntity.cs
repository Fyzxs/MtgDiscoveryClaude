using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

public interface IUserSetCardCollectingOufEntity
{
    string SetGroupId { get; }
    bool Collecting { get; }
    IFinishCountsOufEntity Counts { get; }
    IReadOnlyCollection<string> CollectingFinishes { get; }
}
