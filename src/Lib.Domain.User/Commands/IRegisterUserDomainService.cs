using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.User.Commands;

/// <summary>
/// Marker interface for user registration operation.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface IRegisterUserDomainService
    : IOperationResponseService<IUserInfoItrEntity, IUserInfoOufEntity>;
