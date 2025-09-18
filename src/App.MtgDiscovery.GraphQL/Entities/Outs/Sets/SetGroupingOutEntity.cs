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
