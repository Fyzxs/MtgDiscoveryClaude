using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Entities;

internal sealed class UserSetCardCollectingOufEntity : IUserSetCardCollectingOufEntity
{
    public string SetGroupId { get; init; }
    public bool Collecting { get; init; }
    public IFinishCountsOufEntity Counts { get; init; }
    public IReadOnlyCollection<string> CollectingFinishes { get; init; } = [];
}
