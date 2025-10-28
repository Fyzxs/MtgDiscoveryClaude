using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.Sets.Queries.AllSets;

internal interface IAllSetsAggregatorService
    : IOperationResponseService<INoArgsItrEntity, ISetItemCollectionOufEntity>;
