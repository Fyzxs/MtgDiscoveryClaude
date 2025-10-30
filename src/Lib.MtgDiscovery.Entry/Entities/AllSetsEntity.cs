using Lib.Shared.DataModels.Entities.Args;

namespace Lib.MtgDiscovery.Entry.Entities;

internal sealed class AllSetsEntity : IAllSetsArgEntity
{
    public string UserId { get; init; }
}
