using System.Collections.Generic;

namespace Lib.MtgDiscovery.Entry.Entities.Outs.Sets;

public sealed class GroupingFiltersOutEntity
{
    public CollectorNumberRangeOutEntity CollectorNumberRange { get; init; }
    public IDictionary<string, object> Properties { get; init; }
}
