using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Actions.Mappers;
using App.MtgDiscovery.GraphQL.Authentication;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.User;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Mutations;

[ExtendObjectType(typeof(ApiMutation))]
internal sealed class UserMutationMethods
{
    private readonly IEntryService _entryService;
    private readonly IOperationResponseToResponseModelMapper<UserSyncOutEntity> _userSyncResponseMapper;

    public UserMutationMethods(ILogger logger) : this(
        new EntryService(logger),
        new OperationResponseToResponseModelMapper<UserSyncOutEntity>())
    {
    }

    private UserMutationMethods(
        IEntryService entryService,
        IOperationResponseToResponseModelMapper<UserSyncOutEntity> userSyncResponseMapper)
    {
        _entryService = entryService;
        _userSyncResponseMapper = userSyncResponseMapper;
    }

    [Authorize]
    [GraphQLType(typeof(UserRegistrationResponseModelUnionType))]
    public async Task<ResponseModel> RegisterUserInfoAsync(ClaimsPrincipal claimsPrincipal)
    {
        AuthUserArgEntity authUserArg = new(claimsPrincipal);
        IOperationResponse<UserSyncOutEntity> response = await _entryService.RegisterUserAsync(authUserArg).ConfigureAwait(false);
        return await _userSyncResponseMapper.Map(response).ConfigureAwait(false);
    }
}
