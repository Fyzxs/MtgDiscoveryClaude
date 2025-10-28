using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.Cards.Queries.CardsByName;

internal interface ICardsByNameAggregatorService
    : IOperationResponseService<ICardNameItrEntity, ICardItemCollectionOufEntity>;
