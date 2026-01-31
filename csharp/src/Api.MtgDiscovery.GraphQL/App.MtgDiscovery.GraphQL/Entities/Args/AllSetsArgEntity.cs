using Lib.Shared.DataModels.Entities.Args.Sets;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

internal sealed class AllSetsArgEntity : IAllSetsArgEntity
{
    public string UserId { get; set; }
}
