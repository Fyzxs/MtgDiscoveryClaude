using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Args.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards.Entities;

internal sealed class AddSetGroupToUserSetCardArgEntity : IAddSetGroupToUserSetCardArgEntity
{
    public required string UserId { get; init; }
    public required string SetId { get; init; }
    public required string SetGroupId { get; init; }
    public required bool Collecting { get; init; }
    public required IFinishCountsArgEntity Counts { get; init; }
    public required IReadOnlyCollection<string> CollectingFinishes { get; init; }
}
