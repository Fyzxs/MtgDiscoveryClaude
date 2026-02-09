using Lib.Shared.DataModels.Entities.Itrs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Artists;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.Artists.Queries;

internal interface IArtistSearchAggregatorService
    : IOperationResponseService<IArtistSearchTermItrEntity, IArtistSearchResultCollectionOufEntity>;
