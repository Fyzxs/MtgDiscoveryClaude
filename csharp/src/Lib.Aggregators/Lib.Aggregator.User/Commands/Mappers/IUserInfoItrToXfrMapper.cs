using Lib.Adapter.User.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.User;

namespace Lib.Aggregator.User.Commands.Mappers;

/// <summary>
/// Maps IUserInfoItrEntity to IUserInfoXfrEntity for adapter consumption.
/// </summary>
internal interface IUserInfoItrToXfrMapper : ICreateMapper<IUserInfoItrEntity, IUserInfoXfrEntity>;
