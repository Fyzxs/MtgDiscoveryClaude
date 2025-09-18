using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using App.MtgDiscovery.GraphQL.Entities.Args.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Outs.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Mappers;
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
public sealed class UserCardsMutationMethods
{
    private readonly IEntryService _entryService;
    private readonly IUserCardItrToOutMapper _mapper;

    public UserCardsMutationMethods(ILogger logger) : this(
        new EntryService(logger),
        new UserCardItrToOutMapper())
    {
    }

    private UserCardsMutationMethods(
        IEntryService entryService,
        IUserCardItrToOutMapper mapper)
    {
        _entryService = entryService;
        _mapper = mapper;
    }

    [Authorize]
    [GraphQLType(typeof(UserCardCollectionResponseModelUnionType))]
    public async Task<ResponseModel> AddCardToCollectionAsync(ClaimsPrincipal claimsPrincipal, UserCardArgEntity args)
    {
        AuthUserArgEntity authUserArg = new(claimsPrincipal);

        IOperationResponse<IUserCardItrEntity> response = await _entryService.AddCardToCollectionAsync(authUserArg, args).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        UserCardOutEntity result = await _mapper.Map(response.ResponseData).ConfigureAwait(false);

        return new SuccessDataResponseModel<UserCardOutEntity>() { Data = result };
    }
}
