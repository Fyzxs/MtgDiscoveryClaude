using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.User.Commands;

/// <summary>
/// Adapter for registering new user information in storage.
/// </summary>
internal interface IRegisterUserAdapter
    : IOperationResponseService<IUserInfoItrEntity, UserInfoExtEntity>;
