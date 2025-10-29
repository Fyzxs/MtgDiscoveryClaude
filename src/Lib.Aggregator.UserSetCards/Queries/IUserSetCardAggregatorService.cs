using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.UserSetCards.Queries;

internal interface IUserSetCardAggregatorService
    : IOperationResponseService<IUserSetCardItrEntity, IUserSetCardOufEntity>;
