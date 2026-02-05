using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.Artists.Queries;

/// <summary>
/// Marker interface for retrieving cards by artist ID.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface ICardsByArtistDomainService
{
    Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(
        IArtistIdItrEntity input,
        CancellationToken cancellationToken);
}
