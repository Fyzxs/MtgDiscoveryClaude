using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.Artists.Queries;

/// <summary>
/// Marker interface for retrieving cards by artist name aggregation operation.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface ICardsByArtistNameAggregatorService
    : IOperationResponseService<IArtistNameItrEntity, ICardItemCollectionOufEntity>;
