using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Sets;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.Sets.Queries;

/// <summary>
/// Marker interface for retrieving sets by ID collection.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface ISetsDomainService
{
    Task<IOperationResponse<ISetItemCollectionOufEntity>> Execute(
        ISetIdsItrEntity input,
        CancellationToken cancellationToken);
}
