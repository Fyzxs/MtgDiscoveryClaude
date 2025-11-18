using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Args.UserSetCards;

public interface IAddSetGroupToUserSetCardArgEntity
{
    string SetId { get; }
    string SetGroupId { get; }
    bool Collecting { get; }
    int Count { get; }
    IReadOnlyCollection<string> CollectingFinishes { get; }
}
