namespace Lib.Shared.DataModels.Entities.Itrs;

public interface ISetGroupingItrEntity
{
    string Id { get; }
    string DisplayName { get; }
    int Order { get; }
    IFinishCountsOufEntity CardCounts { get; }
    string RawQuery { get; }
    IGroupingFiltersItrEntity Filters { get; }
}
