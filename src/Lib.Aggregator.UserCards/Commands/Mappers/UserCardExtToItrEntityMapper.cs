using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Aggregator.UserCards.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

/// <summary>
/// Maps UserCardExtEntity to IUserCardItrEntity.
/// </summary>
internal sealed class UserCardExtToItrEntityMapper : IUserCardExtToItrEntityMapper
{
    private readonly IUserCardDetailsExtToItrMapper _mapper;

    public UserCardExtToItrEntityMapper() : this(new UserCardDetailsExtToItrMapper())
    { }

    internal UserCardExtToItrEntityMapper(IUserCardDetailsExtToItrMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IUserCardItrEntity> Map([NotNull] UserCardExtEntity source)
    {
        IUserCardDetailsItrEntity[] mappedDetails = await Task.WhenAll(
            source.CollectedList.Select(detail => _mapper.Map(detail))
        ).ConfigureAwait(false);

        return new UserCardItrEntity
        {
            UserId = source.UserId,
            CardId = source.CardId,
            SetId = source.SetId,
            CollectedList = mappedDetails
        };
    }
}
