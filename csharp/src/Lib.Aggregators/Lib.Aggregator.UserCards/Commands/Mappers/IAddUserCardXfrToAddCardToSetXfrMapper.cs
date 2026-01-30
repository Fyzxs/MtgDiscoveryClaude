using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

/// <summary>
/// Maps IAddUserCardXfrEntity and the updated collected list to IAddCardToSetXfrEntity.
/// Creates AddCardToSetXfrEntity for updating UserSetCards aggregation,
/// including the remaining finish count to prevent incorrect group removal.
/// </summary>
internal interface IAddUserCardXfrToAddCardToSetXfrMapper : ICreateMapper<IAddUserCardXfrEntity, IEnumerable<UserCardDetailsExtEntity>, IAddCardToSetXfrEntity>
{
}
