using Lib.Shared.DataModels.Entities.Args.Sets;

namespace Lib.MtgDiscovery.Entry.Entities;

internal sealed class AllSetsEntity : IAllSetsArgEntity
{
    public string UserId { get; init; }
}
