using System.Threading.Tasks;
using Lib.Adapter.Collections.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.DataModels.Entities.Oufs.Collections;

namespace Lib.Adapter.Collections.Queries.Mappers;

internal sealed class AuthorizedUserExtToOufMapper : IAuthorizedUserExtToOufMapper
{
    public Task<IAuthorizedUserOufEntity> Map(AuthorizedUserExtEntity source)
    {
        IAuthorizedUserOufEntity result = new AuthorizedUserOufEntity
        {
            UserId = source.UserId,
            Role = source.Role,
            GrantedAt = source.GrantedAt,
            GrantedBy = source.GrantedBy
        };

        return Task.FromResult(result);
    }
}
