using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.Abstractions.Mappers;

namespace Lib.Adapter.UserSetCards.Commands.Mappers;

internal interface IUserSetCardsUpsertXfrToExtMapper : ICreateMapper<IUserSetCardUpsertXfrEntity, UserSetCardExtEntity>;
