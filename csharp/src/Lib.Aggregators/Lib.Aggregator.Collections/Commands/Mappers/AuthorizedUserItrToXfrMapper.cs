using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Aggregator.Collections.Commands.Entities;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.Aggregator.Collections.Commands.Mappers;

internal sealed class AuthorizedUserItrToXfrMapper : IAuthorizedUserItrToXfrMapper
{
    public Task<IAuthorizedUserXfrEntity> Map(IAuthorizedUserItrEntity source)
    {
        IAuthorizedUserXfrEntity result = new AuthorizedUserXfrEntity
        {
            UserId = source.UserId,
            Role = source.Role,
            GrantedAt = source.GrantedAt,
            GrantedBy = source.GrantedBy
        };

        return Task.FromResult(result);
    }
}
