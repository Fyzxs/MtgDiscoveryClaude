using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Args.UserSetCards;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserSetCards;

public sealed class AddSetGroupToUserSetCardArgEntity : IAddSetGroupToUserSetCardArgEntity
{
    public string UserId { get; init; }
    public string SetId { get; init; }
    public string SetGroupId { get; init; }
    public bool Collecting { get; init; }
    public IFinishCountsArgEntity Counts { get; init; }
    public IReadOnlyCollection<string> CollectingFinishes { get; init; }
}
