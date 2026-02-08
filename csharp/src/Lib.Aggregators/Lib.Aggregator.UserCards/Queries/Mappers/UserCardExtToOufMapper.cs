using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Aggregator.UserCards.Commands.Mappers;
using Lib.Aggregator.UserCards.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;

namespace Lib.Aggregator.UserCards.Queries.Mappers;

internal sealed class UserCardExtToOufMapper
    : ChildCollectionMapper<UserCardDetailsExtEntity, IUserCardDetailsOufEntity>,
      IUserCardExtToOufMapper
{
    public UserCardExtToOufMapper() : this(new UserCardDetailsExtToOufMapper()) { }

    internal UserCardExtToOufMapper(IUserCardDetailsExtToOufMapper mapper) : base(mapper) { }

    public async Task<IUserCardOufEntity> Map([NotNull] UserCardExtEntity source)
    {
        IUserCardDetailsOufEntity[] mappedDetails = await MapChildren(source.CollectedList).ConfigureAwait(false);

        return new UserCardOufEntity
        {
            UserId = source.UserId,
            CardId = source.CardId,
            SetId = source.SetId,
            CollectedList = mappedDetails
        };
    }
}
