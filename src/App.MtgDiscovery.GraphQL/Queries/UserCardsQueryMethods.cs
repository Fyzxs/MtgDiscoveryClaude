using System.Collections.Generic;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Args.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Outs.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Mappers;
using HotChocolate;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
public sealed class UserCardsQueryMethods
{
    private readonly IEntryService _entryService;
    private readonly ICollectionUserCardOufToOutMapper _collectionMapper;
    private readonly IUserCardOufToOutMapper _singleMapper;

    public UserCardsQueryMethods(ILogger logger) : this(
        new EntryService(logger),
        new CollectionUserCardOufToOutMapper(),
        new UserCardOufToOutMapper())
    {
    }

    private UserCardsQueryMethods(
        IEntryService entryService,
        ICollectionUserCardOufToOutMapper collectionMapper,
        IUserCardOufToOutMapper singleMapper)
    {
        _entryService = entryService;
        _collectionMapper = collectionMapper;
        _singleMapper = singleMapper;
    }

    [GraphQLType(typeof(UserCardsCollectionResponseModelUnionType))]
    public async Task<ResponseModel> UserCardsBySet(UserCardsBySetArgEntity setArgs)
    {
        IOperationResponse<IEnumerable<IUserCardOufEntity>> response = await _entryService
            .UserCardsBySetAsync(setArgs)
            .ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        List<UserCardOutEntity> results = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);

        return new SuccessDataResponseModel<List<UserCardOutEntity>>() { Data = results };
    }

    [GraphQLType(typeof(UserCardsCollectionResponseModelUnionType))]
    public async Task<ResponseModel> UserCard(UserCardArgEntity cardArgs)
    {
        IOperationResponse<IEnumerable<IUserCardOufEntity>> response = await _entryService
            .UserCardAsync(cardArgs)
            .ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        List<UserCardOutEntity> results = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);

        return new SuccessDataResponseModel<List<UserCardOutEntity>>() { Data = results };
    }

    [GraphQLType(typeof(UserCardsCollectionResponseModelUnionType))]
    public async Task<ResponseModel> UserCardsByIds(UserCardsByIdsArgEntity cardsArgs)
    {
        IOperationResponse<IEnumerable<IUserCardOufEntity>> response = await _entryService
            .UserCardsByIdsAsync(cardsArgs)
            .ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        List<UserCardOutEntity> results = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);

        return new SuccessDataResponseModel<List<UserCardOutEntity>>() { Data = results };
    }
}
