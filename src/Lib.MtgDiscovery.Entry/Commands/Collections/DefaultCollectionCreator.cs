using System;
using System.Threading.Tasks;
using Lib.Domain.Collections.Apis;
using Lib.MtgDiscovery.Entry.Commands.Collections.Apis;
using Lib.MtgDiscovery.Entry.Commands.Collections.Entities;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Commands.Collections;

internal sealed class DefaultCollectionCreator : IDefaultCollectionCreator
{
    private readonly ICollectionsDomainService _domainService;

    public DefaultCollectionCreator(ILogger logger) : this(new CollectionsDomainService(logger)) { }

    private DefaultCollectionCreator(ICollectionsDomainService domainService) => _domainService = domainService;

    public async Task CreateDefaultCollectionAsync(string userId)
    {
        // Idempotency check: don't create if a default collection already exists
        // This prevents duplicate collections when registration is called concurrently
        IOperationResponse<ICollectionOufEntity> existingDefault =
            await _domainService.GetDefaultCollectionAsync(userId).ConfigureAwait(false);

        if (existingDefault.IsSuccess)
        {
            return;
        }

        string now = DateTime.UtcNow.ToString("o");

        ICollectionItrEntity defaultCollection = new CollectionItrEntity
        {
            CollectionId = Guid.NewGuid().ToString(),
            OwnerId = userId,
            Name = "My Collection",
            Type = "default",
            Visibility = "public",
            IsDefault = true,
            AuthorizedUsers = [],
            CreatedAt = now,
            UpdatedAt = now
        };

        await _domainService.CreateCollectionAsync(defaultCollection).ConfigureAwait(false);
    }
}
