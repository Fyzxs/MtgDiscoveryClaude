using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.UserCards.Commands.Mappers;
using Lib.Aggregator.UserCards.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Queries.Mappers;

/// <summary>
/// Maps UserCardExtEntity to IUserCardOufEntity for point read operations.
/// </summary>
internal sealed class UserCardExtToItrMapper : IUserCardExtToItrMapper
{
    private readonly IUserCardDetailsExtToOufMapper _mapper;

    public UserCardExtToItrMapper() : this(new UserCardDetailsExtToOufMapper())
    { }

    internal UserCardExtToItrMapper(IUserCardDetailsExtToOufMapper mapper) => _mapper = mapper;

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
