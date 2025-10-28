using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.Sets.Queries.SetsById;

internal interface ISetsByIdAggregatorService
    : IOperationResponseService<ISetIdsItrEntity, ISetItemCollectionOufEntity>;
