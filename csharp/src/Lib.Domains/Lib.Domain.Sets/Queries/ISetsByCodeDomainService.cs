using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Sets;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.Sets.Queries;

/// <summary>
/// Marker interface for retrieving sets by code collection.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface ISetsByCodeDomainService
{
    Task<IOperationResponse<ISetItemCollectionOufEntity>> Execute(
        ISetCodesItrEntity input,
        CancellationToken cancellationToken);
}
