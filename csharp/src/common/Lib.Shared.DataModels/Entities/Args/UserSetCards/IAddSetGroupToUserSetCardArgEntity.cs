using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Models;

namespace Lib.Shared.DataModels.Entities.Args.UserSetCards;

public interface IAddSetGroupToUserSetCardArgEntity : IUserIdArgModel
{
    string SetId { get; }
    string SetGroupId { get; }
    bool Collecting { get; }
    IFinishCountsArgEntity Counts { get; }
    IReadOnlyCollection<string> CollectingFinishes { get; }
}
