using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Collections;
using Lib.Shared.DataModels.Entities.Oufs.Collections;

namespace Lib.MtgDiscovery.Entry.Queries.Collections.Mappers;

internal sealed class CollectionOufListToOutMapper : ICollectionOufListToOutMapper
{
    public Task<List<CollectionOutEntity>> Map(IEnumerable<ICollectionOufEntity> source)
    {
        List<CollectionOutEntity> result = [.. source.Select(ouf => new CollectionOutEntity
        {
            CollectionId = ouf.CollectionId,
            OwnerId = ouf.OwnerId,
            Name = ouf.Name,
            Type = ouf.Type,
            Visibility = ouf.Visibility,
            IsDefault = ouf.IsDefault,
            AuthorizedUsers = [.. ouf.AuthorizedUsers.Select(u => new AuthorizedUserOutEntity
            {
                UserId = u.UserId,
                Role = u.Role,
                GrantedAt = u.GrantedAt,
                GrantedBy = u.GrantedBy
            })],
            CreatedAt = ouf.CreatedAt,
            UpdatedAt = ouf.UpdatedAt
        })];

        return Task.FromResult(result);
    }
}
