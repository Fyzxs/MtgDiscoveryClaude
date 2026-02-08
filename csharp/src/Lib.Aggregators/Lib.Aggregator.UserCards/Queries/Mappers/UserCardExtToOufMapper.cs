using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Aggregator.UserCards.Commands.Mappers;
using Lib.Aggregator.UserCards.Entities;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;

namespace Lib.Aggregator.UserCards.Queries.Mappers;

/// <summary>
/// Maps UserCardExtEntity to IUserCardOufEntity for point read operations.
/// </summary>
internal sealed class UserCardExtToOufMapper : IUserCardExtToOufMapper
{
    private readonly IUserCardDetailsExtToOufMapper _mapper;

    public UserCardExtToOufMapper() : this(new UserCardDetailsExtToOufMapper())
    { }

    internal UserCardExtToOufMapper(IUserCardDetailsExtToOufMapper mapper) => _mapper = mapper;

    public async Task<IUserCardOufEntity> Map([NotNull] UserCardExtEntity source)
    {
        IUserCardDetailsOufEntity[] mappedDetails = await Task.WhenAll(
            source.CollectedList.Select(detail => _mapper.Map(detail))
        ).ConfigureAwait(false);

        return new UserCardOufEntity
        {
            UserId = source.UserId,
            CardId = source.CardId,
            SetId = source.SetId,
            CollectedList = mappedDetails
        };
    }
}
