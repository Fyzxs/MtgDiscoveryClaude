using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Aggregator.Collections.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Collections;

namespace Lib.Aggregator.Collections.Mappers;

internal sealed class CollectionExtToOufMapper
    : ChildCollectionMapper<AuthorizedUserExtEntity, IAuthorizedUserOufEntity>,
      ICollectionExtToOufMapper
{
    public CollectionExtToOufMapper() : this(new AuthorizedUserExtToOufMapper()) { }

    private CollectionExtToOufMapper(IAuthorizedUserExtToOufMapper mapper) : base(mapper) { }

    public async Task<ICollectionOufEntity> Map(CollectionExtEntity source)
    {
        IAuthorizedUserOufEntity[] authorizedUsers = await MapChildren(source.AuthorizedUsers).ConfigureAwait(false);

        ICollectionOufEntity result = new CollectionOufEntity
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
