using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.UserSetCards.Queries.GetUserSetCard;

internal interface IGetUserSetCardAggregatorService
    : IOperationResponseService<IUserSetCardItrEntity, IUserSetCardOufEntity>;
