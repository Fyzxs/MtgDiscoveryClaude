using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs;

public interface ICollectorNumberRangeItrEntity
{
    string Min { get; }
    string Max { get; }
    IList<string> OrConditions { get; }
}
