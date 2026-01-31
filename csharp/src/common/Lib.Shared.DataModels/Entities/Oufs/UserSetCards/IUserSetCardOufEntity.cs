using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Oufs.UserSetCards;

public interface IUserSetCardOufEntity
{
    string UserId { get; }
    string SetId { get; }
    int TotalCards { get; }
    int UniqueCards { get; }
    IReadOnlyDictionary<string, IUserSetCardGroupOufEntity> Groups { get; }
    IReadOnlyCollection<IUserSetCardCollectingOufEntity> Collecting { get; }
}
