using System.Collections.Generic;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Mappers;

namespace Lib.Adapter.Sets.Queries.Mappers;

internal interface ICollectionSetIdToReadPointItemMapper : ICreateMapper<IEnumerable<string>, ICollection<ReadPointItem>>
{
}
