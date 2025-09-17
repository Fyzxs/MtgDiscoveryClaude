using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal interface IUserCardItrToExtapper : ICreateMapper<IUserCardItrEntity, UserCardExtEntity>;
