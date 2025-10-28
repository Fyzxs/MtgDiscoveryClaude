using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.Artists.Queries.ArtistSearch;

/// <summary>
/// Marker interface for artist search aggregation operation.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface IArtistSearchAggregatorService
    : IOperationResponseService<IArtistSearchTermItrEntity, IArtistSearchResultCollectionOufEntity>;
