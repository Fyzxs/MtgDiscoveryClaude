using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.Cards.Queries.CardNameSearch;

internal interface ICardNameSearchAggregatorService
    : IOperationResponseService<ICardSearchTermItrEntity, ICardNameSearchCollectionOufEntity>;
