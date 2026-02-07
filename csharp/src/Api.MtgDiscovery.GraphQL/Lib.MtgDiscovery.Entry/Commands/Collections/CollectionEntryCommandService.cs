using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.Collections.Apis;
using Lib.MtgDiscovery.Entry.Commands.Collections.Apis;
using Lib.MtgDiscovery.Entry.Commands.Collections.Mappers;
using Lib.MtgDiscovery.Entry.Commands.Collections.Validators;
using Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Delete;
using Lib.MtgDiscovery.Entry.Commands.Collections.Validators.GrantAccess;
using Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Rename;
using Lib.MtgDiscovery.Entry.Commands.Collections.Validators.RevokeAccess;
using Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Transfer;
using Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Uniqueness;
using Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Visibility;
using Lib.MtgDiscovery.Entry.Entities.Collections;
using Lib.MtgDiscovery.Entry.Entities.Outs.Collections;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Commands.Collections;

internal sealed class CollectionEntryCommandService : ICollectionEntryCommandService
{
    private readonly ICollectionsDomainService _domainService;
    private readonly ICreateCollectionArgEntityValidator _createValidator;
    private readonly IRenameCollectionArgEntityValidator _renameValidator;
    private readonly IUpdateCollectionVisibilityArgEntityValidator _visibilityValidator;
    private readonly IGrantCollectionAccessArgEntityValidator _grantAccessValidator;
    private readonly IRevokeCollectionAccessArgEntityValidator _revokeAccessValidator;
    private readonly IDeleteCollectionArgEntityValidator _deleteValidator;
    private readonly ITransferCollectionOwnershipArgEntityValidator _transferValidator;
    private readonly ICreateCollectionNameUniquenessValidator _createUniquenessValidator;
    private readonly IRenameCollectionNameUniquenessValidator _renameUniquenessValidator;
    private readonly ICreateCollectionArgToItrMapper _createArgToItrMapper;
    private readonly IRenameCollectionArgToItrMapper _renameArgToItrMapper;
    private readonly IUpdateCollectionVisibilityArgToItrMapper _visibilityArgToItrMapper;
    private readonly IGrantCollectionAccessArgToItrMapper _grantAccessArgToItrMapper;
    private readonly IRevokeCollectionAccessArgToItrMapper _revokeAccessArgToItrMapper;
    private readonly IDeleteCollectionArgToItrMapper _deleteArgToItrMapper;
    private readonly ITransferCollectionOwnershipArgToItrMapper _transferArgToItrMapper;
    private readonly ICollectionOufToOutMapper _oufToOutMapper;

    public CollectionEntryCommandService(ILogger logger) : this(
        new CollectionsDomainService(logger),
        new CreateCollectionArgEntityValidatorContainer(),
        new RenameCollectionArgEntityValidatorContainer(),
        new UpdateCollectionVisibilityArgEntityValidatorContainer(),
        new GrantCollectionAccessArgEntityValidatorContainer(),
        new RevokeCollectionAccessArgEntityValidatorContainer(),
        new DeleteCollectionArgEntityValidatorContainer(),
        new TransferCollectionOwnershipArgEntityValidatorContainer(),
        new CreateCollectionNameUniquenessValidator(new CollectionsDomainService(logger)),
        new RenameCollectionNameUniquenessValidator(new CollectionsDomainService(logger)),
        new CreateCollectionArgToItrMapper(),
        new RenameCollectionArgToItrMapper(),
        new UpdateCollectionVisibilityArgToItrMapper(),
        new GrantCollectionAccessArgToItrMapper(),
        new RevokeCollectionAccessArgToItrMapper(),
        new DeleteCollectionArgToItrMapper(),
        new TransferCollectionOwnershipArgToItrMapper(),
        new CollectionOufToOutMapper())
    { }

    private CollectionEntryCommandService(
        ICollectionsDomainService domainService,
        ICreateCollectionArgEntityValidator createValidator,
        IRenameCollectionArgEntityValidator renameValidator,
        IUpdateCollectionVisibilityArgEntityValidator visibilityValidator,
        IGrantCollectionAccessArgEntityValidator grantAccessValidator,
        IRevokeCollectionAccessArgEntityValidator revokeAccessValidator,
        IDeleteCollectionArgEntityValidator deleteValidator,
        ITransferCollectionOwnershipArgEntityValidator transferValidator,
        ICreateCollectionNameUniquenessValidator createUniquenessValidator,
        IRenameCollectionNameUniquenessValidator renameUniquenessValidator,
        ICreateCollectionArgToItrMapper createArgToItrMapper,
        IRenameCollectionArgToItrMapper renameArgToItrMapper,
        IUpdateCollectionVisibilityArgToItrMapper visibilityArgToItrMapper,
        IGrantCollectionAccessArgToItrMapper grantAccessArgToItrMapper,
        IRevokeCollectionAccessArgToItrMapper revokeAccessArgToItrMapper,
        IDeleteCollectionArgToItrMapper deleteArgToItrMapper,
        ITransferCollectionOwnershipArgToItrMapper transferArgToItrMapper,
        ICollectionOufToOutMapper oufToOutMapper)
    {
        _domainService = domainService;
        _createValidator = createValidator;
        _renameValidator = renameValidator;
        _visibilityValidator = visibilityValidator;
        _grantAccessValidator = grantAccessValidator;
        _revokeAccessValidator = revokeAccessValidator;
        _deleteValidator = deleteValidator;
        _transferValidator = transferValidator;
        _createUniquenessValidator = createUniquenessValidator;
        _renameUniquenessValidator = renameUniquenessValidator;
        _createArgToItrMapper = createArgToItrMapper;
        _renameArgToItrMapper = renameArgToItrMapper;
        _visibilityArgToItrMapper = visibilityArgToItrMapper;
        _grantAccessArgToItrMapper = grantAccessArgToItrMapper;
        _revokeAccessArgToItrMapper = revokeAccessArgToItrMapper;
        _deleteArgToItrMapper = deleteArgToItrMapper;
        _transferArgToItrMapper = transferArgToItrMapper;
        _oufToOutMapper = oufToOutMapper;
    }

    public async Task<IOperationResponse<CollectionOutEntity>> CreateCollectionAsync(ICreateCollectionArgsEntity argsEntity, CancellationToken cancellationToken)
    {
        ICreateCollectionArgEntity argEntity = argsEntity.CreateCollection;
        string userId = argsEntity.AuthUser.UserId;

        IValidatorActionResult<IOperationResponse<ICollectionItrEntity>> validatorResult = await _createValidator.Validate(argEntity).ConfigureAwait(false);
        if (validatorResult.IsNotValid())
        {
            return new FailureOperationResponse<CollectionOutEntity>(validatorResult.FailureStatus().OuterException);
        }

        IValidatorActionResult<IOperationResponse<ICollectionOufEntity>> uniquenessResult = await _createUniquenessValidator.Validate(argsEntity, cancellationToken).ConfigureAwait(false);
        if (uniquenessResult.IsNotValid())
        {
            return new FailureOperationResponse<CollectionOutEntity>(uniquenessResult.FailureStatus().OuterException);
        }

        ICollectionItrEntity itrEntity = await _createArgToItrMapper.Map(argEntity, userId).ConfigureAwait(false);
        IOperationResponse<ICollectionOufEntity> opResponse = await _domainService.CreateCollectionAsync(itrEntity, cancellationToken).ConfigureAwait(false);
        if (opResponse.IsFailure)
        {
            return new FailureOperationResponse<CollectionOutEntity>(opResponse.OuterException);
        }

        CollectionOutEntity outEntity = await _oufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<CollectionOutEntity>(outEntity);
    }

    public async Task<IOperationResponse<CollectionOutEntity>> RenameCollectionAsync(IRenameCollectionArgsEntity argsEntity, CancellationToken cancellationToken)
    {
        IRenameCollectionArgEntity argEntity = argsEntity.RenameCollection;
        string userId = argsEntity.AuthUser.UserId;

        IValidatorActionResult<IOperationResponse<IRenameCollectionItrEntity>> validatorResult = await _renameValidator.Validate(argEntity).ConfigureAwait(false);
        if (validatorResult.IsNotValid())
        {
            return new FailureOperationResponse<CollectionOutEntity>(validatorResult.FailureStatus().OuterException);
        }

        IValidatorActionResult<IOperationResponse<ICollectionOufEntity>> uniquenessResult = await _renameUniquenessValidator.Validate(argsEntity, cancellationToken).ConfigureAwait(false);
        if (uniquenessResult.IsNotValid())
        {
            return new FailureOperationResponse<CollectionOutEntity>(uniquenessResult.FailureStatus().OuterException);
        }

        IRenameCollectionItrEntity itrEntity = await _renameArgToItrMapper.Map(argEntity, userId).ConfigureAwait(false);
        IOperationResponse<ICollectionOufEntity> opResponse = await _domainService.RenameCollectionAsync(itrEntity, cancellationToken).ConfigureAwait(false);
        if (opResponse.IsFailure)
        {
            return new FailureOperationResponse<CollectionOutEntity>(opResponse.OuterException);
        }

        CollectionOutEntity outEntity = await _oufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<CollectionOutEntity>(outEntity);
    }

    public async Task<IOperationResponse<CollectionOutEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityArgsEntity argsEntity, CancellationToken cancellationToken)
    {
        IUpdateCollectionVisibilityArgEntity argEntity = argsEntity.UpdateVisibility;
        string userId = argsEntity.AuthUser.UserId;

        IValidatorActionResult<IOperationResponse<IUpdateCollectionVisibilityItrEntity>> validatorResult = await _visibilityValidator.Validate(argEntity).ConfigureAwait(false);
        if (validatorResult.IsNotValid())
        {
            return new FailureOperationResponse<CollectionOutEntity>(validatorResult.FailureStatus().OuterException);
        }

        IUpdateCollectionVisibilityItrEntity itrEntity = await _visibilityArgToItrMapper.Map(argEntity, userId).ConfigureAwait(false);
        IOperationResponse<ICollectionOufEntity> opResponse = await _domainService.UpdateCollectionVisibilityAsync(itrEntity, cancellationToken).ConfigureAwait(false);
        if (opResponse.IsFailure)
        {
            return new FailureOperationResponse<CollectionOutEntity>(opResponse.OuterException);
        }

        CollectionOutEntity outEntity = await _oufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<CollectionOutEntity>(outEntity);
    }

    public async Task<IOperationResponse<CollectionOutEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessArgsEntity argsEntity, CancellationToken cancellationToken)
    {
        IGrantCollectionAccessArgEntity argEntity = argsEntity.GrantAccess;
        string userId = argsEntity.AuthUser.UserId;

        IValidatorActionResult<IOperationResponse<IGrantCollectionAccessItrEntity>> validatorResult = await _grantAccessValidator.Validate(argEntity).ConfigureAwait(false);
        if (validatorResult.IsNotValid())
        {
            return new FailureOperationResponse<CollectionOutEntity>(validatorResult.FailureStatus().OuterException);
        }

        IGrantCollectionAccessItrEntity itrEntity = await _grantAccessArgToItrMapper.Map(argEntity, userId).ConfigureAwait(false);
        IOperationResponse<ICollectionOufEntity> opResponse = await _domainService.GrantCollectionAccessAsync(itrEntity, cancellationToken).ConfigureAwait(false);
        if (opResponse.IsFailure)
        {
            return new FailureOperationResponse<CollectionOutEntity>(opResponse.OuterException);
        }

        CollectionOutEntity outEntity = await _oufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<CollectionOutEntity>(outEntity);
    }

    public async Task<IOperationResponse<CollectionOutEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessArgsEntity argsEntity, CancellationToken cancellationToken)
    {
        IRevokeCollectionAccessArgEntity argEntity = argsEntity.RevokeAccess;
        string userId = argsEntity.AuthUser.UserId;

        IValidatorActionResult<IOperationResponse<IRevokeCollectionAccessItrEntity>> validatorResult = await _revokeAccessValidator.Validate(argEntity).ConfigureAwait(false);
        if (validatorResult.IsNotValid())
        {
            return new FailureOperationResponse<CollectionOutEntity>(validatorResult.FailureStatus().OuterException);
        }

        IRevokeCollectionAccessItrEntity itrEntity = await _revokeAccessArgToItrMapper.Map(argEntity, userId).ConfigureAwait(false);
        IOperationResponse<ICollectionOufEntity> opResponse = await _domainService.RevokeCollectionAccessAsync(itrEntity, cancellationToken).ConfigureAwait(false);
        if (opResponse.IsFailure)
        {
            return new FailureOperationResponse<CollectionOutEntity>(opResponse.OuterException);
        }

        CollectionOutEntity outEntity = await _oufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<CollectionOutEntity>(outEntity);
    }

    public async Task<IOperationResponse<CollectionOutEntity>> DeleteCollectionAsync(IDeleteCollectionArgsEntity argsEntity, CancellationToken cancellationToken)
    {
        IDeleteCollectionArgEntity argEntity = argsEntity.DeleteCollection;
        string userId = argsEntity.AuthUser.UserId;

        IValidatorActionResult<IOperationResponse<IDeleteCollectionItrEntity>> validatorResult = await _deleteValidator.Validate(argEntity).ConfigureAwait(false);
        if (validatorResult.IsNotValid())
        {
            return new FailureOperationResponse<CollectionOutEntity>(validatorResult.FailureStatus().OuterException);
        }

        IDeleteCollectionItrEntity itrEntity = await _deleteArgToItrMapper.Map(argEntity, userId).ConfigureAwait(false);
        IOperationResponse<ICollectionOufEntity> opResponse = await _domainService.DeleteCollectionAsync(itrEntity, cancellationToken).ConfigureAwait(false);
        if (opResponse.IsFailure)
        {
            return new FailureOperationResponse<CollectionOutEntity>(opResponse.OuterException);
        }

        CollectionOutEntity outEntity = await _oufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<CollectionOutEntity>(outEntity);
    }

    public async Task<IOperationResponse<CollectionOutEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipArgsEntity argsEntity, CancellationToken cancellationToken)
    {
        ITransferCollectionOwnershipArgEntity argEntity = argsEntity.TransferOwnership;
        string userId = argsEntity.AuthUser.UserId;

        IValidatorActionResult<IOperationResponse<ITransferCollectionOwnershipItrEntity>> validatorResult = await _transferValidator.Validate(argEntity).ConfigureAwait(false);
        if (validatorResult.IsNotValid())
        {
            return new FailureOperationResponse<CollectionOutEntity>(validatorResult.FailureStatus().OuterException);
        }

        ITransferCollectionOwnershipItrEntity itrEntity = await _transferArgToItrMapper.Map(argEntity, userId).ConfigureAwait(false);
        IOperationResponse<ICollectionOufEntity> opResponse = await _domainService.TransferCollectionOwnershipAsync(itrEntity, cancellationToken).ConfigureAwait(false);
        if (opResponse.IsFailure)
        {
            return new FailureOperationResponse<CollectionOutEntity>(opResponse.OuterException);
        }

        CollectionOutEntity outEntity = await _oufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<CollectionOutEntity>(outEntity);
    }

    public async Task<IOperationResponse<IEnumerable<AuthorizedUserOutEntity>>> GetCollectionAccessListAsync(IGetCollectionAccessListArgsEntity argsEntity, CancellationToken cancellationToken)
    {
        CollectionIdItrEntity collectionIdItr = new()
        {
            CollectionId = argsEntity.CollectionId,
            OwnerId = argsEntity.AuthUser.UserId
        };

        IOperationResponse<ICollectionOufEntity> opResponse = await _domainService.GetCollectionByIdAsync(collectionIdItr, cancellationToken).ConfigureAwait(false);
        if (opResponse.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<AuthorizedUserOutEntity>>(opResponse.OuterException);
        }

        IEnumerable<AuthorizedUserOutEntity> authorizedUsers = opResponse.ResponseData.AuthorizedUsers.Select(u => new AuthorizedUserOutEntity
        {
            UserId = u.UserId,
            Role = u.Role,
            GrantedAt = u.GrantedAt,
            GrantedBy = u.GrantedBy
        });

        return new SuccessOperationResponse<IEnumerable<AuthorizedUserOutEntity>>(authorizedUsers);
    }
}
