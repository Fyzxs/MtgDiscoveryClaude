using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.Collections.Apis;
using Lib.MtgDiscovery.Entry.Entities.Collections;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Exceptions;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Uniqueness;

internal sealed class RenameCollectionNameUniquenessValidator : IRenameCollectionNameUniquenessValidator
{
    private readonly ICollectionsDomainService _domainService;

    public RenameCollectionNameUniquenessValidator(ICollectionsDomainService domainService) => _domainService = domainService;

    public Task<IValidatorActionResult<IOperationResponse<ICollectionOufEntity>>> Validate(
        IRenameCollectionArgsEntity args) => Validate(args, CancellationToken.None);

    public async Task<IValidatorActionResult<IOperationResponse<ICollectionOufEntity>>> Validate(
        IRenameCollectionArgsEntity args, CancellationToken cancellationToken)
    {
        OwnerIdItrEntity ownerIdItr = new() { OwnerId = args.AuthUser.UserId };
        IOperationResponse<IEnumerable<ICollectionOufEntity>> response = await _domainService
            .GetCollectionsByOwnerAsync(ownerIdItr, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new ValidStatus();
        }

        IEnumerable<ICollectionOufEntity> collections = response.ResponseData ?? [];

        bool nameExists = collections.Any(c =>
            string.Equals(c.Name, args.RenameCollection.Name, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(c.CollectionId, args.RenameCollection.CollectionId, StringComparison.Ordinal) is false);

        if (nameExists)
        {
            return new InvalidStatus();
        }

        return new ValidStatus();
    }

    private sealed class ValidStatus : IValidatorActionResult<IOperationResponse<ICollectionOufEntity>>
    {
        public bool IsValid() => true;

        public IOperationResponse<ICollectionOufEntity> FailureStatus() => null;
    }

    private sealed class InvalidStatus : IValidatorActionResult<IOperationResponse<ICollectionOufEntity>>
    {
        public bool IsValid() => false;

        public IOperationResponse<ICollectionOufEntity> FailureStatus() => new FailureOperationResponse<ICollectionOufEntity>(new BadRequestOperationException("A collection with this name already exists"));
    }
}
