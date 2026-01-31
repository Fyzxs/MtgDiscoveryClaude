using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.User;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.User.Commands;

internal interface IRegisterUserAggregatorService
    : IOperationResponseService<IUserInfoItrEntity, IUserSyncOufEntity>;
