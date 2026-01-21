using System.Collections.Generic;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Args.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.DataModels.Entities.Outs.UserCards;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
public sealed class UserCardsQueryMethods
{
    private readonly IEntryService _entryService;

    public UserCardsQueryMethods(ILogger logger) : this(new EntryService(logger))
    {
    }

    private UserCardsQueryMethods(IEntryService entryService) => _entryService = entryService;

    [GraphQLType(typeof(UserCardsCollectionResponseModelUnionType))]
    public async Task<ResponseModel> UserCardsBySet(UserCardsBySetArgEntity setArgs)
    {
        IOperationResponse<List<UserCardOutEntity>> response = await _entryService
            .UserCardsBySetAsync(setArgs)
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

        return new SuccessDataResponseModel<List<UserCardOutEntity>>() { Data = response.ResponseData };
    }

    [GraphQLType(typeof(UserCardsCollectionResponseModelUnionType))]
    public async Task<ResponseModel> UserCard(UserCardArgEntity cardArgs)
    {
        IOperationResponse<List<UserCardOutEntity>> response = await _entryService
            .UserCardAsync(cardArgs)
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

        return new SuccessDataResponseModel<List<UserCardOutEntity>>() { Data = response.ResponseData };
    }

    [GraphQLType(typeof(UserCardsCollectionResponseModelUnionType))]
    public async Task<ResponseModel> UserCardsByIds(UserCardsByIdsArgEntity cardsArgs)
    {
        IOperationResponse<List<UserCardOutEntity>> response = await _entryService
            .UserCardsByIdsAsync(cardsArgs)
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

        return new SuccessDataResponseModel<List<UserCardOutEntity>>() { Data = response.ResponseData };
    }
}
