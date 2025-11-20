using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;

public interface IAddSetGroupToUserSetCardXfrEntity
{
    string UserId { get; }
    string SetId { get; }
    string SetGroupId { get; }
    bool Collecting { get; }
    IFinishCountsXfrEntity Counts { get; }
    IReadOnlyCollection<string> CollectingFinishes { get; }
}
