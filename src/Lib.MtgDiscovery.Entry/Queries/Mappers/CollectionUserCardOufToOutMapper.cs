using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.UserCards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class CollectionUserCardOufToOutMapper : ICollectionUserCardOufToOutMapper
{
    private readonly IUserCardOufToOutMapper _mapper;

    public CollectionUserCardOufToOutMapper() : this(new UserCardOufToOutMapper())
    { }

    private CollectionUserCardOufToOutMapper(IUserCardOufToOutMapper mapper) => _mapper = mapper;

    public async Task<List<UserCardOutEntity>> Map(IEnumerable<IUserCardOufEntity> userCards)
    {
        UserCardOutEntity[] results = await Task.WhenAll(
            userCards.Select(userCard => _mapper.Map(userCard))
        ).ConfigureAwait(false);

        return [.. results];
    }
}
