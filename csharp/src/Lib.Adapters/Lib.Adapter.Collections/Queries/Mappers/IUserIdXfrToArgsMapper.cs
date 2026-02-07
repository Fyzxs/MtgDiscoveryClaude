using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace Lib.Adapter.Collections.Queries.Mappers;

internal interface IUserIdXfrToArgsMapper
    : ICreateMapper<IUserIdXfrEntity, UserIdExtEntity>;
