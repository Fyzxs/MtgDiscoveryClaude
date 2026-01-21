using System.Collections.Generic;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Outs.UserCards;
using Lib.Shared.DataModels.Entities;

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
        List<UserCardOutEntity> results = [];
        foreach (IUserCardItrEntity userCard in userCards)
        {
            UserCardOutEntity mapped = await _mapper.Map(userCard).ConfigureAwait(false);
            results.Add(mapped);
        }

        return results;
    }
}
