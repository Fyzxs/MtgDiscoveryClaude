using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Sets.Entities;

internal sealed class SetIdsItrEntity : ISetIdsItrEntity
{
    public IReadOnlyCollection<string> SetIds { get; init; }
}
