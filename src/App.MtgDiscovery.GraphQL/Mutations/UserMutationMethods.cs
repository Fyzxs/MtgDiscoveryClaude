using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Mappers;
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
public sealed class UserMutationMethods
{
    private readonly IEntryService _entryService;
    private readonly IOperationResponseToResponseModelMapper<UserRegistrationOutEntity> _userRegistrationResponseMapper;

    public UserMutationMethods(ILogger logger) : this(
        new EntryService(logger),
        new OperationResponseToResponseModelMapper<UserRegistrationOutEntity>())
    {
    }

    private UserMutationMethods(
        IEntryService entryService,
        IOperationResponseToResponseModelMapper<UserRegistrationOutEntity> userRegistrationResponseMapper)
    {
        _entryService = entryService;
        _userRegistrationResponseMapper = userRegistrationResponseMapper;
    }

    [Authorize]
    [GraphQLType(typeof(UserRegistrationResponseModelUnionType))]
    public async Task<ResponseModel> RegisterUserInfoAsync(ClaimsPrincipal claimsPrincipal)
    {
        AuthUserArgEntity authUserArg = new(claimsPrincipal);
        IOperationResponse<UserRegistrationOutEntity> response = await _entryService.RegisterUserAsync(authUserArg).ConfigureAwait(false);
        return await _userRegistrationResponseMapper.Map(response).ConfigureAwait(false);
    }
}
