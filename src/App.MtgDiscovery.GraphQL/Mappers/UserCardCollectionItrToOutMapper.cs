using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Outs.UserCards;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal sealed class UserCardCollectionItrToOutMapper : IUserCardCollectionItrToOutMapper
{
    private readonly IUserCardItrToOutMapper _mapper;

    public UserCardCollectionItrToOutMapper() : this(new UserCardItrToOutMapper())
    { }

    private UserCardCollectionItrToOutMapper(IUserCardItrToOutMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<List<UserCardOutEntity>> Map(IEnumerable<IUserCardItrEntity> userCards)
    {
        UserCardOutEntity[] results = await Task.WhenAll(
            userCards.Select(userCard => _mapper.Map(userCard))
        ).ConfigureAwait(false);

        return results.ToList();
    }
}
