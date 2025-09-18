using System.Linq;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Outs.UserCards;
using Lib.Shared.DataModels.Entities.Itrs;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal sealed class UserCardOufToOutMapper : IUserCardOufToOutMapper
{
    private readonly IUserCardDetailsOufToOutMapper _mapper;

    public UserCardOufToOutMapper() : this(new UserCardDetailsOufToOutMapper()) { }

    internal UserCardOufToOutMapper(IUserCardDetailsOufToOutMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<UserCardOutEntity> Map(IUserCardOufEntity source)
    {
        CollectedItemOutEntity[] mappedDetails = await Task.WhenAll(
            source.CollectedList.Select(detail => _mapper.Map(detail))
        ).ConfigureAwait(false);

        UserCardOutEntity result = new()
        {
            UserId = source.UserId,
            CardId = source.CardId,
            SetId = source.SetId,
            CollectedList = mappedDetails.ToList()
        };

        return result;
    }
}
