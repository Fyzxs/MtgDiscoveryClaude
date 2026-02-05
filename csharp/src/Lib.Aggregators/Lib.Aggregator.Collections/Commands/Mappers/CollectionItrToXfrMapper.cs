using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Aggregator.Collections.Commands.Entities;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.Aggregator.Collections.Commands.Mappers;

internal sealed class CollectionItrToXfrMapper : ICollectionItrToXfrMapper
{
    private readonly IAuthorizedUserItrToXfrMapper _authorizedUserMapper;

    public CollectionItrToXfrMapper() : this(new AuthorizedUserItrToXfrMapper()) { }

    private CollectionItrToXfrMapper(IAuthorizedUserItrToXfrMapper authorizedUserMapper)
    {
        _authorizedUserMapper = authorizedUserMapper;
    }

    public async Task<ICollectionXfrEntity> Map(ICollectionItrEntity source)
    {
        IAuthorizedUserXfrEntity[] authorizedUsers = await Task.WhenAll(
            source.AuthorizedUsers.Select(u => _authorizedUserMapper.Map(u))).ConfigureAwait(false);

        ICollectionXfrEntity result = new CollectionXfrEntity
        {
            CollectionId = source.CollectionId,
            OwnerId = source.OwnerId,
            Name = source.Name,
            Type = source.Type,
            Visibility = source.Visibility,
            IsDefault = source.IsDefault,
            AuthorizedUsers = authorizedUsers,
            CreatedAt = source.CreatedAt,
            UpdatedAt = source.UpdatedAt
        };

        return result;
    }
}
