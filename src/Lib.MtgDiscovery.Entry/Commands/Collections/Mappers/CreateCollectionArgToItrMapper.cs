using System;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.Collections.Entities;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Mappers;

internal sealed class CreateCollectionArgToItrMapper : ICreateCollectionArgToItrMapper
{
    public Task<ICollectionItrEntity> Map(ICreateCollectionArgEntity source1, string userId)
    {
        string now = DateTime.UtcNow.ToString("o");
        string visibility = string.IsNullOrWhiteSpace(source1.Visibility) ? "private" : source1.Visibility;

        ICollectionItrEntity result = new CollectionItrEntity
        {
            CollectionId = Guid.NewGuid().ToString(),
            OwnerId = userId,
            Name = source1.Name,
            Type = source1.Type,
            Visibility = visibility,
            IsDefault = false,
            AuthorizedUsers = [],
            CreatedAt = now,
            UpdatedAt = now
        };

        return Task.FromResult(result);
    }
}
