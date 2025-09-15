using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using App.MtgDiscovery.GraphQL.Entities.Args.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Outs.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Entities.Types.UserCards;
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

    public UserCardsMutationMethods(ILogger logger) : this(new EntryService(logger))
    {
    }

    private UserCardsMutationMethods(IEntryService entryService)
    {
        _entryService = entryService;
    }

    [Authorize]
    [GraphQLType(typeof(UserCardCollectionResponseModelUnionType))]
    public async Task<ResponseModel> AddCardToCollectionAsync(ClaimsPrincipal claimsPrincipal, string cardId, string setId, ICollection<CollectedItemArgEntity> collectedList)
    {
        // Create AuthUserArgEntity directly from ClaimsPrincipal
        AuthUserArgEntity authUserArg = new(claimsPrincipal);

        AddCardToCollectionArgEntity args = new()
        {
            UserId = authUserArg.UserId,
            CardId = cardId,
            SetId = setId,
            CollectedList = [.. collectedList.Cast<ICollectedItemArgEntity>()]
        };

        IOperationResponse<IUserCardCollectionItrEntity> response = await _entryService.AddCardToCollectionAsync(args).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        IUserCardCollectionItrEntity collection = response.ResponseData;

        UserCardCollectionOutEntity result = new()
        {
            UserId = collection.UserId,
            CardId = collection.CardId,
            SetId = collection.SetId,
            CollectedList = [.. collection.CollectedList.Select(item => new CollectedItemOutEntity { Finish = item.Finish, Special = item.Special, Count = item.Count })]
        };

        return new SuccessDataResponseModel<UserCardCollectionOutEntity>() { Data = result };
    }

}
