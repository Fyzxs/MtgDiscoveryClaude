using System.Collections.Generic;
using Lib.Adapter.Sets.Apis.Entities;

namespace Lib.Adapter.Sets.Queries.Entities;

internal sealed class SetIdsXfrEntity : ISetIdsXfrEntity
{
    public IEnumerable<string> SetIds { get; init; }
}