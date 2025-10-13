using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Abstractions.Mappers;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal interface IUserSetCardFinishGroupExtToItrMapper : ICreateMapper<UserSetCardFinishGroupExtEntity, IUserSetCardFinishGroupOufEntity>;
