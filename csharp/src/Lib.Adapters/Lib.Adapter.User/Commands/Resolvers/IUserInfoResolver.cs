using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserInfo;
using Lib.Cosmos.Resolvers;
using Lib.Shared.DataModels.Entities.Itrs.User;

namespace Lib.Adapter.User.Commands.Resolvers;

internal interface IUserInfoResolver : ICosmosResolver<UserInfoExtEntity, IUserInfoItrEntity>;
