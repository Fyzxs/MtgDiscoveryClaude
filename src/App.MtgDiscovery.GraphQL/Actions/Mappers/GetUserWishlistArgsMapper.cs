using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers;

internal sealed class GetUserWishlistArgsMapper : IGetUserWishlistArgsMapper
{
    public Task<IGetUserWishlistArgsEntity> Map(string userId)
    {
        IGetUserWishlistArgsEntity entity = new GetUserWishlistArgsEntity
        {
            TargetUserId = userId
        };

        return Task.FromResult(entity);
    }
}
