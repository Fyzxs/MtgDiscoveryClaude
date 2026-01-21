using Lib.Shared.DataModels.Entities.Args.Sets;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

public sealed class AllSetsArgEntity : IAllSetsArgEntity
{
    public string UserId { get; set; }
}
