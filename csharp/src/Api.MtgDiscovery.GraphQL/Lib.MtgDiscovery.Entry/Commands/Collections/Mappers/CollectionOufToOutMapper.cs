using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Collections;
using Lib.Shared.DataModels.Entities.Oufs.Collections;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Mappers;

internal sealed class CollectionOufToOutMapper : ICollectionOufToOutMapper
{
    public Task<CollectionOutEntity> Map(ICollectionOufEntity source)
    {
        CollectionOutEntity result = new()
        {
            CollectionId = source.CollectionId,
            OwnerId = source.OwnerId,
            Name = source.Name,
            Type = source.Type,
            Visibility = source.Visibility,
            IsDefault = source.IsDefault,
            AuthorizedUsers = [.. source.AuthorizedUsers.Select(u => new AuthorizedUserOutEntity
            {
                UserId = u.UserId,
                Role = u.Role,
                GrantedAt = u.GrantedAt,
                GrantedBy = u.GrantedBy
            })],
            CreatedAt = source.CreatedAt,
            UpdatedAt = source.UpdatedAt
        };

        return Task.FromResult(result);
    }
}
