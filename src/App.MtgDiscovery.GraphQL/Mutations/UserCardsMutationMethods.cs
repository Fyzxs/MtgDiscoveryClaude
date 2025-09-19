using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using App.MtgDiscovery.GraphQL.Entities.Args.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.DataModels.Entities.Outs.UserCards;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Mutations;

[ExtendObjectType(typeof(ApiMutation))]
public sealed class UserCardsMutationMethods
{
    private readonly IEntryService _entryService;

    public UserCardsMutationMethods(ILogger logger) : this(new EntryService(logger))
    {
    }

    private UserCardsMutationMethods(IEntryService entryService)
    {
        _entryService = entryService;
    }

    [Authorize]
    [GraphQLType(typeof(UserCardCollectionResponseModelUnionType))]
    public async Task<ResponseModel> AddCardToCollectionAsync(ClaimsPrincipal claimsPrincipal, AddUserCardArgEntity args)
    {
        AuthUserArgEntity authUserArg = new(claimsPrincipal);

        IOperationResponse<UserCardOutEntity> response = await _entryService.AddCardToCollectionAsync(authUserArg, args).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        return new SuccessDataResponseModel<UserCardOutEntity>() { Data = response.ResponseData };
    }
}
