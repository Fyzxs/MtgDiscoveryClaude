using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.User.Commands;

internal interface IRegisterUserAggregatorService
    : IOperationResponseService<IUserInfoItrEntity, IUserInfoOufEntity>;
