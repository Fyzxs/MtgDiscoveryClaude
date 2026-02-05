using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSetCards;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace Lib.Adapter.UserSetCards.Commands.Mappers;

internal interface IUserSetCardsUpsertXfrToExtMapper : ICreateMapper<IUserSetCardUpsertXfrEntity, UserSetCardExtEntity>;
