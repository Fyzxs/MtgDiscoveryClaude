namespace Lib.Shared.DataModels.Entities;

public interface ISetGroupingItrEntity
{
    string Id { get; }
    string DisplayName { get; }
    int Order { get; }
    int CardCount { get; }
    string RawQuery { get; }
    IGroupingFiltersItrEntity Filters { get; }
}
