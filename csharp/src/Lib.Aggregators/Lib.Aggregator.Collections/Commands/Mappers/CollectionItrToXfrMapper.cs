using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Aggregator.Collections.Commands.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.Aggregator.Collections.Commands.Mappers;

internal sealed class CollectionItrToXfrMapper
    : ChildCollectionMapper<IAuthorizedUserItrEntity, IAuthorizedUserXfrEntity>,
      ICollectionItrToXfrMapper
{
    public CollectionItrToXfrMapper() : this(new AuthorizedUserItrToXfrMapper()) { }

    private CollectionItrToXfrMapper(IAuthorizedUserItrToXfrMapper mapper) : base(mapper) { }

    public async Task<ICollectionXfrEntity> Map(ICollectionItrEntity source)
    {
        IAuthorizedUserXfrEntity[] authorizedUsers = await MapChildren(source.AuthorizedUsers).ConfigureAwait(false);

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
