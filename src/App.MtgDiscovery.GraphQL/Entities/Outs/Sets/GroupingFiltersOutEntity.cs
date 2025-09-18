using System.Collections.Generic;

namespace App.MtgDiscovery.GraphQL.Entities.Outs.Sets;

public sealed class GroupingFiltersOutEntity
{
    public CollectorNumberRangeOutEntity CollectorNumberRange { get; init; }
    public IDictionary<string, object> Properties { get; init; }
}
