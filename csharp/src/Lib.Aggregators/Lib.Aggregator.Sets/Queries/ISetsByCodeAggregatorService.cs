using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Sets;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.Sets.Queries;

internal interface ISetsByCodeAggregatorService
    : IOperationResponseService<ISetCodesItrEntity, ISetItemCollectionOufEntity>;
