using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Actions.Mappers;
using App.MtgDiscovery.GraphQL.Actions.Mappers.Collections;
using App.MtgDiscovery.GraphQL.Entities.Args.Collections;
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

namespace App.MtgDiscovery.GraphQL.Mutations;

[ExtendObjectType(typeof(ApiMutation))]
public sealed class CollectionMutationMethods
{
    private readonly IEntryService _entryService;
    private readonly IOperationResponseToResponseModelMapper<List<CollectionOutEntity>> _responseMapper;
    private readonly ICreateCollectionArgsMapper _createCollectionArgsMapper;
    private readonly IRenameCollectionArgsMapper _renameCollectionArgsMapper;
    private readonly IUpdateCollectionVisibilityArgsMapper _updateCollectionVisibilityArgsMapper;
    private readonly IGrantCollectionAccessArgsMapper _grantCollectionAccessArgsMapper;
    private readonly IRevokeCollectionAccessArgsMapper _revokeCollectionAccessArgsMapper;
    private readonly IDeleteCollectionArgsMapper _deleteCollectionArgsMapper;
    private readonly ITransferCollectionOwnershipArgsMapper _transferCollectionOwnershipArgsMapper;

    public CollectionMutationMethods(ILogger logger) : this(
        new EntryService(logger),
        new OperationResponseToResponseModelMapper<List<CollectionOutEntity>>(),
        new CreateCollectionArgsMapper(),
        new RenameCollectionArgsMapper(),
        new UpdateCollectionVisibilityArgsMapper(),
        new GrantCollectionAccessArgsMapper(),
        new RevokeCollectionAccessArgsMapper(),
        new DeleteCollectionArgsMapper(),
        new TransferCollectionOwnershipArgsMapper())
    { }

    private CollectionMutationMethods(
        IEntryService entryService,
        IOperationResponseToResponseModelMapper<List<CollectionOutEntity>> responseMapper,
        ICreateCollectionArgsMapper createCollectionArgsMapper,
        IRenameCollectionArgsMapper renameCollectionArgsMapper,
        IUpdateCollectionVisibilityArgsMapper updateCollectionVisibilityArgsMapper,
        IGrantCollectionAccessArgsMapper grantCollectionAccessArgsMapper,
        IRevokeCollectionAccessArgsMapper revokeCollectionAccessArgsMapper,
        IDeleteCollectionArgsMapper deleteCollectionArgsMapper,
        ITransferCollectionOwnershipArgsMapper transferCollectionOwnershipArgsMapper)
    {
        _entryService = entryService;
        _responseMapper = responseMapper;
        _createCollectionArgsMapper = createCollectionArgsMapper;
        _renameCollectionArgsMapper = renameCollectionArgsMapper;
        _updateCollectionVisibilityArgsMapper = updateCollectionVisibilityArgsMapper;
        _grantCollectionAccessArgsMapper = grantCollectionAccessArgsMapper;
        _revokeCollectionAccessArgsMapper = revokeCollectionAccessArgsMapper;
        _deleteCollectionArgsMapper = deleteCollectionArgsMapper;
        _transferCollectionOwnershipArgsMapper = transferCollectionOwnershipArgsMapper;
    }

    [Authorize]
    [GraphQLType(typeof(CollectionResponseModelUnionType))]
    public async Task<ResponseModel> CreateCollectionAsync(ClaimsPrincipal claimsPrincipal, CreateCollectionArgEntity args)
    {
        ICreateCollectionArgsEntity argsEntity = await _createCollectionArgsMapper
            .Map(claimsPrincipal, args)
            .ConfigureAwait(false);
        IOperationResponse<CollectionOutEntity> response = await _entryService
            .CreateCollectionAsync(argsEntity)
            .ConfigureAwait(false);
        return await MapSingleToList(response).ConfigureAwait(false);
    }

    [Authorize]
    [GraphQLType(typeof(CollectionResponseModelUnionType))]
    public async Task<ResponseModel> RenameCollectionAsync(ClaimsPrincipal claimsPrincipal, RenameCollectionArgEntity args)
    {
        IRenameCollectionArgsEntity argsEntity = await _renameCollectionArgsMapper
            .Map(claimsPrincipal, args)
            .ConfigureAwait(false);
        IOperationResponse<CollectionOutEntity> response = await _entryService
            .RenameCollectionAsync(argsEntity)
            .ConfigureAwait(false);
        return await MapSingleToList(response).ConfigureAwait(false);
    }

    [Authorize]
    [GraphQLType(typeof(CollectionResponseModelUnionType))]
    public async Task<ResponseModel> UpdateCollectionVisibilityAsync(ClaimsPrincipal claimsPrincipal, UpdateCollectionVisibilityArgEntity args)
    {
        IUpdateCollectionVisibilityArgsEntity argsEntity = await _updateCollectionVisibilityArgsMapper
            .Map(claimsPrincipal, args)
            .ConfigureAwait(false);
        IOperationResponse<CollectionOutEntity> response = await _entryService
            .UpdateCollectionVisibilityAsync(argsEntity)
            .ConfigureAwait(false);
        return await MapSingleToList(response).ConfigureAwait(false);
    }

    [Authorize]
    [GraphQLType(typeof(CollectionResponseModelUnionType))]
    public async Task<ResponseModel> GrantCollectionAccessAsync(ClaimsPrincipal claimsPrincipal, GrantCollectionAccessArgEntity args)
    {
        IGrantCollectionAccessArgsEntity argsEntity = await _grantCollectionAccessArgsMapper
            .Map(claimsPrincipal, args)
            .ConfigureAwait(false);
        IOperationResponse<CollectionOutEntity> response = await _entryService
            .GrantCollectionAccessAsync(argsEntity)
            .ConfigureAwait(false);
        return await MapSingleToList(response).ConfigureAwait(false);
    }

    [Authorize]
    [GraphQLType(typeof(CollectionResponseModelUnionType))]
    public async Task<ResponseModel> RevokeCollectionAccessAsync(ClaimsPrincipal claimsPrincipal, RevokeCollectionAccessArgEntity args)
    {
        IRevokeCollectionAccessArgsEntity argsEntity = await _revokeCollectionAccessArgsMapper
            .Map(claimsPrincipal, args)
            .ConfigureAwait(false);
        IOperationResponse<CollectionOutEntity> response = await _entryService
            .RevokeCollectionAccessAsync(argsEntity)
            .ConfigureAwait(false);
        return await MapSingleToList(response).ConfigureAwait(false);
    }

    [Authorize]
    [GraphQLType(typeof(CollectionResponseModelUnionType))]
    public async Task<ResponseModel> DeleteCollectionAsync(ClaimsPrincipal claimsPrincipal, DeleteCollectionArgEntity args)
    {
        IDeleteCollectionArgsEntity argsEntity = await _deleteCollectionArgsMapper
            .Map(claimsPrincipal, args)
            .ConfigureAwait(false);
        IOperationResponse<CollectionOutEntity> response = await _entryService
            .DeleteCollectionAsync(argsEntity)
            .ConfigureAwait(false);
        return await MapSingleToList(response).ConfigureAwait(false);
    }

    [Authorize]
    [GraphQLType(typeof(CollectionResponseModelUnionType))]
    public async Task<ResponseModel> TransferCollectionOwnershipAsync(ClaimsPrincipal claimsPrincipal, TransferCollectionOwnershipArgEntity args)
    {
        ITransferCollectionOwnershipArgsEntity argsEntity = await _transferCollectionOwnershipArgsMapper
            .Map(claimsPrincipal, args)
            .ConfigureAwait(false);
        IOperationResponse<CollectionOutEntity> response = await _entryService
            .TransferCollectionOwnershipAsync(argsEntity)
            .ConfigureAwait(false);
        return await MapSingleToList(response).ConfigureAwait(false);
    }

    private Task<ResponseModel> MapSingleToList(IOperationResponse<CollectionOutEntity> response)
    {
        if (response.IsSuccess)
        {
            return _responseMapper.Map(new SuccessOperationResponse<List<CollectionOutEntity>>([response.ResponseData]));
        }

        return _responseMapper.Map(new FailureOperationResponse<List<CollectionOutEntity>>(response.OuterException));
    }
}
