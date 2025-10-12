using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Aggregator.UserSetCards.Entities;
using Lib.Shared.Abstractions.Mappers;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal interface IUserSetCardGroupExtToItrMapper : ICreateMapper<UserSetCardGroupExtEntity, IUserSetCardGroupOufEntity>;
