using System.Collections.Generic;

namespace Lib.Aggregator.UserSetCards.Entities;

internal sealed class UserSetCardOufEntity : IUserSetCardOufEntity
{
    public string UserId { get; init; }
    public string SetId { get; init; }
    public int TotalCards { get; init; }
    public int UniqueCards { get; init; }
    public IReadOnlyDictionary<string, IUserSetCardGroupOufEntity> Groups { get; init; }
    public IReadOnlyCollection<string> GroupsCollecting { get; init; }
}
