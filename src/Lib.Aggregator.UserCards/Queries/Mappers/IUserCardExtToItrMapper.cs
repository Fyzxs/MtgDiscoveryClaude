using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Queries.Mappers;

/// <summary>
/// Maps UserCardExtEntity to IUserCardOufEntity for point read operations.
/// </summary>
internal interface IUserCardExtToItrMapper : ICreateMapper<UserCardExtEntity, IUserCardOufEntity>;
