using System.Collections.Generic;
using Lib.Shared.DataModels.Abstractions;

namespace Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;

public interface IAddSetGroupToUserSetCardXfrEntity : IXfrEntity
{
    string UserId { get; }
    string SetId { get; }
    string SetGroupId { get; }
    bool Collecting { get; }
    IFinishCountsXfrEntity Counts { get; }
    IReadOnlyCollection<string> CollectingFinishes { get; }
}
