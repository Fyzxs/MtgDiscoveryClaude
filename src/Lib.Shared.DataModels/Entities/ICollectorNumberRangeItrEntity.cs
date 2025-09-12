using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities;

public interface ICollectorNumberRangeItrEntity
{
    string Min { get; }
    string Max { get; }
    IList<string> OrConditions { get; }
}
