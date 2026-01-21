using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.User.Commands.RegisterUser;

internal interface IRegisterUserAggregatorService
    : IOperationResponseService<IUserInfoItrEntity, IUserInfoOufEntity>;
