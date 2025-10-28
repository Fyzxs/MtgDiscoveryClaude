using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.Sets.Queries.SetsByCode;

internal interface ISetsByCodeAggregatorService
    : IOperationResponseService<ISetCodesItrEntity, ISetItemCollectionOufEntity>;
