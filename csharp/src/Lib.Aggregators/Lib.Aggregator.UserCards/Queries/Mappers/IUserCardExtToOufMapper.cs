using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;

namespace Lib.Aggregator.UserCards.Queries.Mappers;

/// <summary>
/// Maps UserCardExtEntity to IUserCardOufEntity for point read operations.
/// </summary>
internal interface IUserCardExtToOufMapper : ICreateMapper<UserCardExtEntity, IUserCardOufEntity>;
