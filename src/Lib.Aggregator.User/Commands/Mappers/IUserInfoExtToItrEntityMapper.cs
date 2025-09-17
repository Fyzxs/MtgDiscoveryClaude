using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.User.Commands.Mappers;

/// <summary>
/// Maps UserInfoExtEntity to IUserInfoItrEntity.
/// </summary>
internal interface IUserInfoExtToItrEntityMapper : ICreateMapper<UserInfoExtEntity, IUserInfoItrEntity>;
