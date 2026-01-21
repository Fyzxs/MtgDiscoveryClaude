using System.Linq;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class UserCardOufToOutMapper : IUserCardOufToOutMapper
{
    private readonly IUserCardDetailsOufToOutMapper _mapper;

    public UserCardOufToOutMapper() : this(new UserCardDetailsOufToOutMapper()) { }

    internal UserCardOufToOutMapper(IUserCardDetailsOufToOutMapper mapper) => _mapper = mapper;

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
            CollectedList = [.. mappedDetails]
        };

        return result;
    }
}
