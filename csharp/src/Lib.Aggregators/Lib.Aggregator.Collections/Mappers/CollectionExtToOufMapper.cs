using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Aggregator.Collections.Entities;
using Lib.Shared.DataModels.Entities.Oufs.Collections;

namespace Lib.Aggregator.Collections.Mappers;

internal sealed class CollectionExtToOufMapper : ICollectionExtToOufMapper
{
    private readonly IAuthorizedUserExtToOufMapper _authorizedUserMapper;

    public CollectionExtToOufMapper() : this(new AuthorizedUserExtToOufMapper()) { }

    private CollectionExtToOufMapper(IAuthorizedUserExtToOufMapper authorizedUserMapper)
    {
        _authorizedUserMapper = authorizedUserMapper;
    }

    public async Task<ICollectionOufEntity> Map(CollectionExtEntity source)
    {
        IAuthorizedUserOufEntity[] authorizedUsers = await Task.WhenAll(
            source.AuthorizedUsers?.Select(u => _authorizedUserMapper.Map(u)) ?? []).ConfigureAwait(false);

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
