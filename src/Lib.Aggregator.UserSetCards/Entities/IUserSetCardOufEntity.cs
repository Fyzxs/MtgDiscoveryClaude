using System.Collections.Generic;

namespace Lib.Aggregator.UserSetCards.Entities;

public interface IUserSetCardOufEntity
{
    string UserId { get; }
    string SetId { get; }
    int TotalCards { get; }
    int UniqueCards { get; }
    IReadOnlyDictionary<string, IUserSetCardGroupOufEntity> Groups { get; }
    IReadOnlyCollection<IUserSetCardCollectingOufEntity> Collecting { get; }
}
