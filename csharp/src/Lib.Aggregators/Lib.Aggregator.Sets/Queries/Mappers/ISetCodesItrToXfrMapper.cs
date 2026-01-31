using Lib.Adapter.Sets.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Sets;

namespace Lib.Aggregator.Sets.Queries.Mappers;

internal interface ISetCodesItrToXfrMapper : ICreateMapper<ISetCodesItrEntity, ISetCodesXfrEntity>;
