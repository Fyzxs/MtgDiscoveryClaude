using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.Cards.Queries;

internal interface ICardsBySetCodeAggregatorService
    : IOperationResponseService<ISetCodeItrEntity, ICardItemCollectionOufEntity>;
