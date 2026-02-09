using Lib.Shared.DataModels.Entities.Itrs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.Artists.Queries;

internal interface ICardsByArtistNameAggregatorService
    : IOperationResponseService<IArtistNameItrEntity, ICardItemCollectionOufEntity>;
