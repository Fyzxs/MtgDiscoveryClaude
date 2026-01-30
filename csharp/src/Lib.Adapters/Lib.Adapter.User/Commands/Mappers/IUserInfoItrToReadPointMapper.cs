using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.User;

namespace Lib.Adapter.User.Commands.Mappers;

internal interface IUserInfoItrToReadPointMapper : ICreateMapper<IUserInfoItrEntity, ReadPointItem>;
