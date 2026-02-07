using Lib.Adapter.Collections.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.Aggregator.Collections.Queries.Mappers;

internal interface ICollectionIdItrToXfrMapper : ICreateMapper<ICollectionIdItrEntity, ICollectionIdXfrEntity>;
