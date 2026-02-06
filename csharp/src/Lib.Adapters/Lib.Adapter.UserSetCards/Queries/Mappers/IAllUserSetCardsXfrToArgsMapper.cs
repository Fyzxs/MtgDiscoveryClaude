using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace Lib.Adapter.UserSetCards.Queries.Mappers;

internal interface IAllUserSetCardsXfrToArgsMapper : ICreateMapper<IAllUserSetCardsXfrEntity, AllUserSetCardsExtEntitys>;
