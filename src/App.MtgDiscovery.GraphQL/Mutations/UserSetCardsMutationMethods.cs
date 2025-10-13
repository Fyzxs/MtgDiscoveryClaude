using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using App.MtgDiscovery.GraphQL.Entities.Args.UserSetCards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Mutations;

[ExtendObjectType(typeof(ApiMutation))]
public sealed class UserSetCardsMutationMethods
{
    private readonly IEntryService _entryService;

    public UserSetCardsMutationMethods(ILogger logger) : this(new EntryService(logger))
    {
    }

    private UserSetCardsMutationMethods(IEntryService entryService) => _entryService = entryService;

    [Authorize]
    [GraphQLType(typeof(UserSetCardResponseModelUnionType))]
    public async Task<ResponseModel> AddSetGroupToUserSetCardAsync(
        ClaimsPrincipal claimsPrincipal,
        AddSetGroupToUserSetCardArgEntity input)
    {
        AuthUserArgEntity authUserArg = new(claimsPrincipal);

        IOperationResponse<UserSetCardOutEntity> response = await _entryService
            .AddSetGroupToUserSetCardAsync(authUserArg, input)
            .ConfigureAwait(false);

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

        return new SuccessDataResponseModel<UserSetCardOutEntity>() { Data = response.ResponseData };
    }
}
