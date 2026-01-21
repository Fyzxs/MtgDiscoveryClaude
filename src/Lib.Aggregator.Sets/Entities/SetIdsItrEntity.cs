using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Sets.Entities;

internal sealed class SetIdsItrEntity : ISetIdsItrEntity
{
    public ICollection<string> SetIds { get; init; }
}
