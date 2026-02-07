using Lib.Adapter.Collections.Apis.Entities;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace Lib.Adapter.Collections.Queries.Mappers;

internal interface ICollectionIdXfrToReadPointMapper
    : ICreateMapper<ICollectionIdXfrEntity, ReadPointItem>;
