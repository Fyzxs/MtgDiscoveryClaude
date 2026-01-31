using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

internal interface IUserCardDetailsExtToOufMapper : ICreateMapper<UserCardDetailsExtEntity, IUserCardDetailsOufEntity>;
