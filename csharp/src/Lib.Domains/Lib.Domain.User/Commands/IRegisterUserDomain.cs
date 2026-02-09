using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.User;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.User.Commands;

internal interface IRegisterUserDomain
    : IOperationResponseService<IUserInfoItrEntity, IUserSyncOufEntity>;
