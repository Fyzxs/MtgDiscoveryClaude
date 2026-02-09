using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Sets;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.Sets.Queries;

internal interface ISetsDomain
    : IOperationResponseService<ISetIdsItrEntity, ISetItemCollectionOufEntity>;
