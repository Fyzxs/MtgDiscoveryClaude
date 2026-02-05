using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserInfo;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.User;

namespace Lib.Aggregator.User.Commands.Mappers;

/// <summary>
/// Maps UserInfoExtEntity to IUserSyncOufEntity.
/// Computes IsFirstLogin from timestamps (CreatedAt == LastLoginAt indicates first login).
/// </summary>
internal interface IUserInfoExtToSyncOufMapper
    : ICreateMapper<UserInfoExtEntity, IUserSyncOufEntity>;
