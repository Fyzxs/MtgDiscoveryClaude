using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.Sets.Queries;

internal interface ISetsByCodeAggregatorService
    : IOperationResponseService<ISetCodesItrEntity, ISetItemCollectionOufEntity>;
