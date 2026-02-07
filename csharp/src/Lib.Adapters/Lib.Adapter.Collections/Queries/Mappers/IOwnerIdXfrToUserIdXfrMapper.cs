using Lib.Adapter.Collections.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace Lib.Adapter.Collections.Queries.Mappers;

internal interface IOwnerIdXfrToUserIdXfrMapper
    : ICreateMapper<IOwnerIdXfrEntity, IUserIdXfrEntity>;
