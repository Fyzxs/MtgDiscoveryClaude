using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs;

public interface IGroupingFiltersItrEntity
{
    ICollectorNumberRangeItrEntity CollectorNumberRange { get; }
    IDictionary<string, object> Properties { get; }
}
