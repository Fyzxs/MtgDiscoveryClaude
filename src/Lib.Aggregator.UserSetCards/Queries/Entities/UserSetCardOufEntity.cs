using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Entities;

internal sealed class UserSetCardOufEntity : IUserSetCardOufEntity
{
    public string UserId { get; init; }
    public string SetId { get; init; }
    public int TotalCards { get; init; }
    public int UniqueCards { get; init; }
    public IReadOnlyDictionary<string, IUserSetCardGroupOufEntity> Groups { get; init; }
    public IReadOnlyCollection<IUserSetCardCollectingOufEntity> Collecting { get; init; }
}
