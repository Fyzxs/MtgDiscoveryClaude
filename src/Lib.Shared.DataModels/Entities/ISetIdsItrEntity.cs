using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities;

public interface ISetIdsItrEntity
{
    IReadOnlyCollection<string> SetIds { get; }
}
