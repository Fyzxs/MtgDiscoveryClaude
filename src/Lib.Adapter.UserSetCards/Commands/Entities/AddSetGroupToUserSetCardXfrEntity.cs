using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;

namespace Lib.Adapter.UserSetCards.Commands.Entities;

internal sealed class AddSetGroupToUserSetCardXfrEntity : IAddSetGroupToUserSetCardXfrEntity
{
    public string UserId { get; init; } = string.Empty;
    public string SetId { get; init; } = string.Empty;
    public string SetGroupId { get; init; } = string.Empty;
    public bool Collecting { get; init; }
    public IFinishCountsXfrEntity Counts { get; init; }
    public IReadOnlyCollection<string> CollectingFinishes { get; init; } = [];
}
