using System.Collections.Generic;
using Lib.Adapter.Sets.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace Lib.Adapter.Sets.Queries.Mappers;

internal interface ICollectionStringToSetIdsXfrMapper : ICreateMapper<IEnumerable<string>, ISetIdsXfrEntity>
{
}
