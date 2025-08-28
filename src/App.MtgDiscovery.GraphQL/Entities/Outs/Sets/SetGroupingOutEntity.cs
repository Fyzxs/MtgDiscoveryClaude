using System.Collections.Generic;

namespace App.MtgDiscovery.GraphQL.Entities.Outs.Sets;

public sealed class SetGroupingOutEntity
{
    public string Id { get; init; }
    public string DisplayName { get; init; }
    public int Order { get; init; }
    public int CardCount { get; init; }
    public string RawQuery { get; init; }
    public GroupingFiltersOutEntity Filters { get; init; }
}

public sealed class GroupingFiltersOutEntity
{
    public CollectorNumberRangeOutEntity CollectorNumberRange { get; init; }
    public IDictionary<string, object> Properties { get; init; }
}

public sealed class CollectorNumberRangeOutEntity
{
    public string Min { get; init; }
    public string Max { get; init; }
    public IList<string> OrConditions { get; init; }
}