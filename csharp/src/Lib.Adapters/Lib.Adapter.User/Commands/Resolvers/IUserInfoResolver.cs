using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserInfo;
using Lib.Adapter.User.Apis.Entities;
using Lib.Cosmos.Resolvers;

namespace Lib.Adapter.User.Commands.Resolvers;

internal interface IUserInfoResolver : ICosmosResolver<UserInfoExtEntity, IUserInfoXfrEntity>;
