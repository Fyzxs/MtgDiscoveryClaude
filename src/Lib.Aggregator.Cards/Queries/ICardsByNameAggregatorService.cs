using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.Cards.Queries;

internal interface ICardsByNameAggregatorService
    : IOperationResponseService<ICardNameItrEntity, ICardItemCollectionOufEntity>;
