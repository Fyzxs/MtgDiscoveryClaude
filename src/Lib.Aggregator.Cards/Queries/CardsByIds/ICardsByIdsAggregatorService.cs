using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.Cards.Queries.CardsByIds;

internal interface ICardsByIdsAggregatorService
    : IOperationResponseService<ICardIdsItrEntity, ICardItemCollectionOufEntity>;
