using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs.Cards;

public interface ICollectorNumberRangeItrEntity
{
    string Min { get; }
    string Max { get; }
    IList<string> OrConditions { get; }
}
