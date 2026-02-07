using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.Artists.Queries;

/// <summary>
/// Marker interface for retrieving cards by artist ID aggregation operation.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface ICardsByArtistAggregatorService
{
    Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(
        IArtistIdItrEntity input,
        CancellationToken cancellationToken);
}
