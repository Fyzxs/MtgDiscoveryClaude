using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
    { }

    private UserCardsMutationMethods(IEntryService entryService) =>
        _entryService = entryService;

    [Authorize]
    [GraphQLType(typeof(UserCardCollectionResponseModelUnionType))]
    public async Task<ResponseModel> AddCardToCollectionAsync(
        ClaimsPrincipal claimsPrincipal,
        string cardId,
        string setId,
        ICollection<CollectedItemArgEntity> collectedList)
    {
        if (claimsPrincipal is null)
        {
            return new FailureResponseModel
            {
                Status = new StatusDataModel
                {
                    Message = "Authentication required",
                    StatusCode = System.Net.HttpStatusCode.Unauthorized
                }
            };
        }

        if (collectedList is null)
        {
            return new FailureResponseModel
            {
                Status = new StatusDataModel
                {
                    Message = "Collected list is required",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                }
            };
        }

        string userId = ExtractUserId(claimsPrincipal);

        AddCardToCollectionArgEntity args = new()
        {
            UserId = userId,
            CardId = cardId,
            SetId = setId,
            CollectedList = [.. collectedList.Cast<ICollectedItemArgEntity>()]
        };

        IOperationResponse<IUserCardCollectionItrEntity> response = await _entryService
            .AddCardToCollectionAsync(args)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureResponseModel
            {
                Status = new StatusDataModel
                {
                    Message = response.OuterException?.StatusMessage ?? "Failed to add card to collection",
                    StatusCode = response.OuterException?.StatusCode ?? System.Net.HttpStatusCode.InternalServerError
                }
            };
        }

        IUserCardCollectionItrEntity collection = response.ResponseData;

        UserCardCollectionOutEntity result = new()
        {
            UserId = collection.UserId,
            CardId = collection.CardId,
            SetId = collection.SetId,
            CollectedList = [.. collection.CollectedList
                .Select(item => new CollectedItemOutEntity
                {
                    Finish = item.Finish,
                    Special = item.Special,
                    Count = item.Count
                })]
        };

        return new SuccessDataResponseModel<UserCardCollectionOutEntity> { Data = result };
    }

    private static string ExtractUserId(ClaimsPrincipal claimsPrincipal)
    {
        System.Guid userSubjectNamespace = new("4d746755-7365-7253-7562-6a6563744775");
        string subject = claimsPrincipal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        System.Guid guid = Lib.Universal.Utilities.GuidUtility.Create(userSubjectNamespace, subject);
        return guid.ToString();
    }
}
