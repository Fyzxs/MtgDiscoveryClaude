using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface ICollectionUserSetCardOufToOutMapper
{
    Task<List<UserSetCardOutEntity>> Map(IEnumerable<IUserSetCardOufEntity> userSetCards);
}

internal sealed class CollectionUserSetCardOufToOutMapper : ICollectionUserSetCardOufToOutMapper
{
    private readonly IUserSetCardOufToOutMapper _mapper;

    public CollectionUserSetCardOufToOutMapper() : this(new UserSetCardOufToOutMapper())
    {
    }

    private CollectionUserSetCardOufToOutMapper(IUserSetCardOufToOutMapper mapper) => _mapper = mapper;

    public async Task<List<UserSetCardOutEntity>> Map(IEnumerable<IUserSetCardOufEntity> userSetCards)
    {
        UserSetCardOutEntity[] results = await Task.WhenAll(
            userSetCards.Select(userSetCard => _mapper.Map(userSetCard))
        ).ConfigureAwait(false);

        return [.. results];
    }
}
