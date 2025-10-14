using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal interface IUserSetCardCollectingExtToOufMapper : ICreateMapper<UserSetCardCollectingExtEntity, IUserSetCardCollectingOufEntity>;
