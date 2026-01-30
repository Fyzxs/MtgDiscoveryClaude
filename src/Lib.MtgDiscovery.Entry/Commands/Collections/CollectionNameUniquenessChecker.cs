using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Domain.Collections.Apis;
using Lib.MtgDiscovery.Entry.Commands.Collections.Apis;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Commands.Collections;

internal sealed class CollectionNameUniquenessChecker : ICollectionNameUniquenessChecker
{
    private readonly ICollectionsDomainService _domainService;

    public CollectionNameUniquenessChecker(ILogger logger) : this(new CollectionsDomainService(logger)) { }

    private CollectionNameUniquenessChecker(ICollectionsDomainService domainService) =>
        _domainService = domainService;

    public async Task<bool> IsNameUniqueForOwnerAsync(string name, string ownerId, string excludeCollectionId = null)
    {
        IOperationResponse<IEnumerable<ICollectionOufEntity>> response = await _domainService
            .GetCollectionsByOwnerAsync(ownerId)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return true;
        }

        IEnumerable<ICollectionOufEntity> collections = response.ResponseData ?? [];

        bool nameExists = collections.Any(c =>
            string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase) &&
            (excludeCollectionId is null || string.Equals(c.CollectionId, excludeCollectionId, StringComparison.Ordinal) is false));

        return nameExists is false;
    }
}
