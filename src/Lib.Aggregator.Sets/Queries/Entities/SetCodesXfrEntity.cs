using System.Collections.Generic;
using Lib.Adapter.Sets.Apis.Entities;

namespace Lib.Aggregator.Sets.Queries.Entities;

internal sealed class SetCodesXfrEntity : ISetCodesXfrEntity
{
    public IEnumerable<string> SetCodes { get; init; }
}