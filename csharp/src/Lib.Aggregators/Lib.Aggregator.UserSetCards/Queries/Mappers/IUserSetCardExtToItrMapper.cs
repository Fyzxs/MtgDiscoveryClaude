using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal interface IUserSetCardExtToItrMapper : ICreateMapper<UserSetCardExtEntity, IUserSetCardOufEntity>;
