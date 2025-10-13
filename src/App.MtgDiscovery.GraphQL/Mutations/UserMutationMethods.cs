using System.Security.Claims;
using System.Threading.Tasks;
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
public sealed class UserMutationMethods
{
    private readonly IEntryService _entryService;

    public UserMutationMethods(ILogger logger) : this(new EntryService(logger))
    {
    }

    private UserMutationMethods(IEntryService entryService) => _entryService = entryService;

    [Authorize]
    [GraphQLType(typeof(UserRegistrationResponseModelUnionType))]
    public async Task<ResponseModel> RegisterUserInfoAsync(ClaimsPrincipal claimsPrincipal)
    {
        AuthUserArgEntity authUserArg = new(claimsPrincipal);

        IOperationResponse<UserRegistrationOutEntity> response = await _entryService.RegisterUserAsync(authUserArg).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureResponseModel()
            {
                Status = new StatusDataModel()
                {
                    Message = response.OuterException.StatusMessage,
                    StatusCode = response.OuterException.StatusCode
                }
            };
        }

        return new SuccessDataResponseModel<UserRegistrationOutEntity>() { Data = response.ResponseData };
    }
}
