using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.UserCards.Commands.AddUserCard;

internal interface IAddUserCardAggregatorService
    : IOperationResponseService<IUserCardItrEntity, IUserCardOufEntity>;
