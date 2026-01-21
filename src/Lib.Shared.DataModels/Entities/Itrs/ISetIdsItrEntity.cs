using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs;

public interface ISetIdsItrEntity
{
    ICollection<string> SetIds { get; }
}
