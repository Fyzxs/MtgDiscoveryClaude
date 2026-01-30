using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.Shared.DataModels.Entities.Itrs.Sets;

public interface ISetGroupingItrEntity
{
    string Id { get; }
    string DisplayName { get; }
    int Order { get; }
    IFinishCountsItrEntity CardCounts { get; }
    string RawQuery { get; }
    IGroupingFiltersItrEntity Filters { get; }
}
