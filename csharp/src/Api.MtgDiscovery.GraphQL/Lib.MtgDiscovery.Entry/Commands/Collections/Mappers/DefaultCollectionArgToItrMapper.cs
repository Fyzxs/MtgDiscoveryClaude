using System;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.Collections.Entities;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Mappers;

internal sealed class DefaultCollectionArgToItrMapper : IDefaultCollectionArgToItrMapper
{
    public Task<ICollectionItrEntity> Map(IUserIdArgEntity source)
    {
        string now = DateTime.UtcNow.ToString("o");

        ICollectionItrEntity result = new CollectionItrEntity
        {
            CollectionId = Guid.NewGuid().ToString(),
            OwnerId = source.UserId,
            Name = "My Collection",
            Type = "default",
            Visibility = "public",
            IsDefault = true,
            AuthorizedUsers = [],
            CreatedAt = now,
            UpdatedAt = now
        };

        return Task.FromResult(result);
    }
}
