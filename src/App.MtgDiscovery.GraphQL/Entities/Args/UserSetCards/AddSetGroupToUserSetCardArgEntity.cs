using Lib.Shared.DataModels.Entities.Args.UserSetCards;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserSetCards;

public sealed class AddSetGroupToUserSetCardArgEntity : IAddSetGroupToUserSetCardArgEntity
{
    public string SetId { get; init; }
    public string SetGroupId { get; init; }
    public bool Collecting { get; init; }
    public int Count { get; init; }
}
