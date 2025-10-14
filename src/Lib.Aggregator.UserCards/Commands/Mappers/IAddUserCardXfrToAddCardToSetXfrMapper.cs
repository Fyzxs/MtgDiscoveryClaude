using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.Abstractions.Mappers;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

/// <summary>
/// Maps IAddUserCardXfrEntity to IAddCardToSetXfrEntity.
/// Creates AddCardToSetXfrEntity for updating UserSetCards aggregation.
/// </summary>
internal interface IAddUserCardXfrToAddCardToSetXfrMapper : ICreateMapper<IAddUserCardXfrEntity, IAddCardToSetXfrEntity>
{
}
