using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Outs.Sets;

public sealed class CollectorNumberRangeOutEntity
{
    public string Min { get; init; }
    public string Max { get; init; }
    public IList<string> OrConditions { get; init; }
}
