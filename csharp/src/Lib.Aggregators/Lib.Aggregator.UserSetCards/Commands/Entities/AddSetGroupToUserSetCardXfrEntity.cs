using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Commands.Entities;

internal sealed class AddSetGroupToUserSetCardXfrEntity : IAddSetGroupToUserSetCardXfrEntity
{
    public string UserId { get; init; }
    public string SetId { get; init; }
    public string SetGroupId { get; init; }
    public bool Collecting { get; init; }
    public IFinishCountsXfrEntity Counts { get; init; }
    public IReadOnlyCollection<string> CollectingFinishes { get; init; }
    public string CacheKey => $"add_set_group:{UserId}:{SetId}:{SetGroupId}";
}
