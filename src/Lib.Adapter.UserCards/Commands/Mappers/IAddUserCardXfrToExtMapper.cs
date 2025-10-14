using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Abstractions.Mappers;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal interface IAddUserCardXfrToExtMapper : ICreateMapper<IAddUserCardXfrEntity, UserCardExtEntity>;
