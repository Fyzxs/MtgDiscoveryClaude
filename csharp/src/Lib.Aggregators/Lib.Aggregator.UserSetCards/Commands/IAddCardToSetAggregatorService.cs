using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.UserSetCards.Commands;

internal interface IAddCardToSetAggregatorService
    : IOperationResponseService<IAddCardToSetItrEntity, IUserSetCardOufEntity>;
