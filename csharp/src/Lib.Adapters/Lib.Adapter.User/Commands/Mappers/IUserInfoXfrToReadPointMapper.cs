using Lib.Adapter.User.Apis.Entities;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace Lib.Adapter.User.Commands.Mappers;

internal interface IUserInfoXfrToReadPointMapper : ICreateMapper<IUserInfoXfrEntity, ReadPointItem>;
