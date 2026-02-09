using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserInfo;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.User;

namespace Lib.Aggregator.User.Commands.Mappers;

/// <summary>
/// Maps UserInfoExtEntity to IUserInfoItrEntity.
/// </summary>
internal interface IUserInfoExtToOufEntityMapper : ICreateMapper<UserInfoExtEntity, IUserInfoOufEntity>;
