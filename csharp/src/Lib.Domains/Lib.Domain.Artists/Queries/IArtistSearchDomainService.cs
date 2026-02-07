using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Artists;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.Artists.Queries;

/// <summary>
/// Marker interface for artist search operation.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface IArtistSearchDomainService
{
    Task<IOperationResponse<IArtistSearchResultCollectionOufEntity>> Execute(
        IArtistSearchTermItrEntity input,
        CancellationToken cancellationToken);
}
