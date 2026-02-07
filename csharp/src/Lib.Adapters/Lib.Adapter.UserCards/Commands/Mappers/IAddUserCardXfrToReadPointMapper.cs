using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace Lib.Adapter.UserCards.Commands.Mappers;

/// <summary>
/// Maps AddUserCardXfrEntity to a Cosmos read point for querying user card data.
/// </summary>
internal interface IAddUserCardXfrToReadPointMapper : ICreateMapper<IAddUserCardXfrEntity, ReadPointItem>;
