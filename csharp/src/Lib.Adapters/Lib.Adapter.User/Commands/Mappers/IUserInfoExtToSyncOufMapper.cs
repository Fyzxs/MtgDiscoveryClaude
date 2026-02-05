using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserInfo;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.User;

namespace Lib.Adapter.User.Commands.Mappers;

internal interface IUserInfoExtToSyncOufMapper : ICreateMapper<UserInfoExtEntity, bool, IUserSyncOufEntity>;
