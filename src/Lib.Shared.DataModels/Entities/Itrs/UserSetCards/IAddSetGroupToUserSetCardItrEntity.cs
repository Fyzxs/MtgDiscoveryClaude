using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

public interface IAddSetGroupToUserSetCardItrEntity
{
    string UserId { get; }
    string SetId { get; }
    string SetGroupId { get; }
    bool Collecting { get; }
    int Count { get; }
    IReadOnlyCollection<string> CollectingFinishes { get; }
}
