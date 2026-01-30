using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Actions.Mappers;
using App.MtgDiscovery.GraphQL.Actions.Mappers.Collections;
using App.MtgDiscovery.GraphQL.Authentication;
using App.MtgDiscovery.GraphQL.Entities.Types.Collections;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Collections;
using Lib.MtgDiscovery.Entry.Entities.Outs.Collections;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
public sealed class CollectionQueryMethods
{
    private readonly IEntryService _entryService;
    private readonly IOperationResponseToResponseModelMapper<List<CollectionOutEntity>> _listResponseMapper;
    private readonly IOperationResponseToResponseModelMapper<CollectionOutEntity> _singleResponseMapper;
    private readonly IOperationResponseToResponseModelMapper<IEnumerable<AuthorizedUserOutEntity>> _authorizedUsersResponseMapper;
    private readonly IGetCollectionAccessListArgsMapper _getCollectionAccessListArgsMapper;

    public CollectionQueryMethods(ILogger logger) : this(
        new EntryService(logger),
        new OperationResponseToResponseModelMapper<List<CollectionOutEntity>>(),
        new OperationResponseToResponseModelMapper<CollectionOutEntity>(),
        new OperationResponseToResponseModelMapper<IEnumerable<AuthorizedUserOutEntity>>(),
        new GetCollectionAccessListArgsMapper())
    { }

    private CollectionQueryMethods(
        IEntryService entryService,
        IOperationResponseToResponseModelMapper<List<CollectionOutEntity>> listResponseMapper,
        IOperationResponseToResponseModelMapper<CollectionOutEntity> singleResponseMapper,
        IOperationResponseToResponseModelMapper<IEnumerable<AuthorizedUserOutEntity>> authorizedUsersResponseMapper,
        IGetCollectionAccessListArgsMapper getCollectionAccessListArgsMapper)
    {
        _entryService = entryService;
        _listResponseMapper = listResponseMapper;
        _singleResponseMapper = singleResponseMapper;
        _authorizedUsersResponseMapper = authorizedUsersResponseMapper;
        _getCollectionAccessListArgsMapper = getCollectionAccessListArgsMapper;
    }

    [Authorize]
    [GraphQLType(typeof(CollectionsResponseModelUnionType))]
    public async Task<ResponseModel> MyCollectionsAsync(ClaimsPrincipal claimsPrincipal)
    {
        AuthUserArgEntity authUser = new(claimsPrincipal);
        IOperationResponse<List<CollectionOutEntity>> response = await _entryService
            .MyCollectionsAsync(authUser.UserId)
            .ConfigureAwait(false);
        return await _listResponseMapper.Map(response).ConfigureAwait(false);
    }

    [Authorize]
    [GraphQLType(typeof(CollectionResponseModelUnionType))]
    public async Task<ResponseModel> GetCollectionAsync(ClaimsPrincipal claimsPrincipal, string collectionId)
    {
        AuthUserArgEntity authUser = new(claimsPrincipal);
        IOperationResponse<CollectionOutEntity> response = await _entryService
            .GetCollectionByIdAsync(collectionId, authUser.UserId)
            .ConfigureAwait(false);
        return await _singleResponseMapper.Map(response).ConfigureAwait(false);
    }

    [Authorize]
    [GraphQLType(typeof(AuthorizedUsersResponseModelUnionType))]
    public async Task<ResponseModel> CollectionAccessListAsync(ClaimsPrincipal claimsPrincipal, string collectionId)
    {
        IGetCollectionAccessListArgsEntity argsEntity = await _getCollectionAccessListArgsMapper
            .Map(claimsPrincipal, collectionId)
            .ConfigureAwait(false);
        IOperationResponse<IEnumerable<AuthorizedUserOutEntity>> response = await _entryService
            .GetCollectionAccessListAsync(argsEntity)
            .ConfigureAwait(false);
        return await _authorizedUsersResponseMapper.Map(response).ConfigureAwait(false);
    }

    [Authorize]
    [GraphQLType(typeof(CollectionsResponseModelUnionType))]
    public async Task<ResponseModel> SharedCollectionsAsync(ClaimsPrincipal claimsPrincipal)
    {
        AuthUserArgEntity authUser = new(claimsPrincipal);
        IOperationResponse<List<CollectionOutEntity>> response = await _entryService
            .SharedCollectionsAsync(authUser.UserId)
            .ConfigureAwait(false);
        return await _listResponseMapper.Map(response).ConfigureAwait(false);
    }

    [Authorize]
    [GraphQLType(typeof(CollectionsResponseModelUnionType))]
    public async Task<ResponseModel> AccessibleCollectionsAsync(ClaimsPrincipal claimsPrincipal)
    {
        AuthUserArgEntity authUser = new(claimsPrincipal);
        IOperationResponse<List<CollectionOutEntity>> response = await _entryService
            .AccessibleCollectionsAsync(authUser.UserId)
            .ConfigureAwait(false);
        return await _listResponseMapper.Map(response).ConfigureAwait(false);
    }
}
