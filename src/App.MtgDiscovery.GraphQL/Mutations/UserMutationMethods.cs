using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using App.MtgDiscovery.GraphQL.Entities.Outs.User;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Mutations;

[ExtendObjectType(typeof(ApiMutation))]
public class UserMutationMethods
{
    private readonly IEntryService _entryService;

    public UserMutationMethods(ILogger logger) : this(new EntryService(logger))
    {
    }

    private UserMutationMethods(IEntryService entryService)
    {
        _entryService = entryService;
    }

    [Authorize]
    [GraphQLType(typeof(UserRegistrationResponseModelUnionType))]
    public async Task<ResponseModel> RegisterUserInfoAsync(ClaimsPrincipal claimsPrincipal)
    {
        // Create AuthUserArgEntity directly from ClaimsPrincipal
        AuthUserArgEntity authUserArg = new(claimsPrincipal);

        // Call the entry service
        IOperationResponse<IUserInfoItrEntity> response = await _entryService.RegisterUserAsync(authUserArg).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        IUserInfoItrEntity registration = response.ResponseData;

        UserRegistrationOutEntity result = new()
        {
            UserId = registration.UserId,
            DisplayName = authUserArg.DisplayName
        };

        return new SuccessDataResponseModel<UserRegistrationOutEntity>() { Data = result };
    }
}
