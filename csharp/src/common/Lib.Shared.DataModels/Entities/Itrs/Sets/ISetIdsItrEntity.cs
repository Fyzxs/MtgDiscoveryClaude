using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs.Sets;

public interface ISetIdsItrEntity
{
    ICollection<string> SetIds { get; }
}
