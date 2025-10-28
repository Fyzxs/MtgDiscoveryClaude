using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.Cards.Queries.CardsBySetCode;

internal interface ICardsBySetCodeAggregatorService
    : IOperationResponseService<ISetCodeItrEntity, ICardItemCollectionOufEntity>;
