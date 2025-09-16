using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using App.MtgDiscovery.GraphQL.Entities.Args.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Outs.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Entities.Types.UserCards;
using App.MtgDiscovery.GraphQL.Mappers;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Mutations;

[ExtendObjectType(typeof(ApiMutation))]
public sealed class UserCardsMutationMethods
{
    private readonly IEntryService _entryService;
    private readonly IUserCardCollectionItrToOutMapper _mapper;

    public UserCardsMutationMethods(ILogger logger) : this(
        new EntryService(logger),
        new UserCardCollectionItrToOutMapper())
    {
    }

    private UserCardsMutationMethods(
        IEntryService entryService,
        IUserCardCollectionItrToOutMapper mapper)
    {
        _entryService = entryService;
        _mapper = mapper;
    }

    [Authorize]
    [GraphQLType(typeof(UserCardCollectionResponseModelUnionType))]
    public async Task<ResponseModel> AddCardToCollectionAsync(ClaimsPrincipal claimsPrincipal, AddCardToCollectionArgEntity args)
    {
        // Extract user information from JWT claims
        AuthUserArgEntity authUserArg = new(claimsPrincipal);

        // Pass both auth and args to entry service - it will combine them during mapping
        IOperationResponse<IUserCardCollectionItrEntity> response = await _entryService.AddCardToCollectionAsync(authUserArg, args).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        // Use mapper to transform ITR entity to OUT entity
        UserCardCollectionOutEntity result = await _mapper.Map(response.ResponseData).ConfigureAwait(false);

        return new SuccessDataResponseModel<UserCardCollectionOutEntity>() { Data = result };
    }

}
