using Lib.Adapter.Sets.Apis.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Sets.Queries.Mappers;

internal interface ISetIdsItrToXfrMapper : ICreateMapper<ISetIdsItrEntity, ISetIdsXfrEntity>;