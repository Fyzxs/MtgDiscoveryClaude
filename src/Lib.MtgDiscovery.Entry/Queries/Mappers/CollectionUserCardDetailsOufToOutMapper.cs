using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class CollectionUserCardDetailsOufToOutMapper : ICollectionUserCardDetailsOufToOutMapper
{
    private readonly IUserCardDetailsOufToOutMapper _mapper;

    public CollectionUserCardDetailsOufToOutMapper() : this(new UserCardDetailsOufToOutMapper())
    { }
    private CollectionUserCardDetailsOufToOutMapper(IUserCardDetailsOufToOutMapper mapper) => _mapper = mapper;

    public async Task<ICollection<CollectedItemOutEntity>> Map(IEnumerable<IUserCardDetailsOufEntity> source)
    {
        CollectedItemOutEntity[] collectedItemOutEntities = await Task.WhenAll(
            source.Select(x => _mapper.Map(x))
        ).ConfigureAwait(false);
        return collectedItemOutEntities;
    }
}
