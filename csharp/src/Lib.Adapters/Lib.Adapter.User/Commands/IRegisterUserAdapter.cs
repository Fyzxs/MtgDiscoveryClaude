using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserInfo;
using Lib.Adapter.User.Apis.Entities;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.User.Commands;

/// <summary>
/// Adapter for registering or syncing user information in storage.
/// Returns UserInfoExtEntity directly; aggregator determines isFirstLogin from timestamps.
/// </summary>
internal interface IRegisterUserAdapter
    : IOperationResponseService<IUserInfoXfrEntity, UserInfoExtEntity>;
