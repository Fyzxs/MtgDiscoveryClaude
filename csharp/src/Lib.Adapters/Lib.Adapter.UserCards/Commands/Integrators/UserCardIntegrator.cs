using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Adapter.UserCards.Commands.Mappers;

namespace Lib.Adapter.UserCards.Commands.Integrators;

internal sealed class UserCardIntegrator : IUserCardIntegrator
{
    private readonly ICollectedItemsMergeMapper _mergeMapper;
    private readonly ICollectedItemsReplaceMapper _replaceMapper;
    private readonly IUserCardMetadataMapper _metadataMapper;

    public UserCardIntegrator() : this(new CollectedItemsMergeMapper(), new CollectedItemsReplaceMapper(), new UserCardMetadataMapper())
    { }

    private UserCardIntegrator(ICollectedItemsMergeMapper mergeMapper, ICollectedItemsReplaceMapper replaceMapper, IUserCardMetadataMapper metadataMapper)
    {
        _mergeMapper = mergeMapper;
        _replaceMapper = replaceMapper;
        _metadataMapper = metadataMapper;
    }

    public Task<UserCardExtEntity> Integrate(UserCardExtEntity current, IAddUserCardXfrEntity change)
    {
        ICollection<UserCardDetailsExtEntity> updatedCollectedList = change.ReplaceMode
            ? _replaceMapper.Map([.. current.CollectedList], change.Details)
            : _mergeMapper.Map([.. current.CollectedList], change.Details);

        UserCardExtEntity result = _metadataMapper.Map(current, change, updatedCollectedList);

        return Task.FromResult(result);
    }
}
