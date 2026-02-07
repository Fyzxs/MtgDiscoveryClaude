using Lib.Shared.DataModels.Entities.Xfrs.Sets;

namespace Lib.Aggregator.Sets.Queries.Entities;

/// <summary>
/// Concrete implementation of IAllSetsXfrEntity for Aggregator layer.
/// Represents AllSets query operations at the adapter layer.
/// </summary>
internal sealed class AllSetsXfrEntity : IAllSetsXfrEntity
{
    public string CacheKey => $"sets:all";
}
