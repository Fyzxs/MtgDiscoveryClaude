using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

internal interface IUserCardDetailsExtToItrMapper : ICreateMapper<UserCardDetailsExtEntity, IUserCardDetailsItrEntity>;