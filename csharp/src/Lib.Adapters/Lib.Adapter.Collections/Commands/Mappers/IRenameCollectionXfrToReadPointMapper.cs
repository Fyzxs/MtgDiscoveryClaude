using Lib.Adapter.Collections.Apis.Entities;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace Lib.Adapter.Collections.Commands.Mappers;

internal interface IRenameCollectionXfrToReadPointMapper
    : ICreateMapper<IRenameCollectionXfrEntity, ReadPointItem>;
