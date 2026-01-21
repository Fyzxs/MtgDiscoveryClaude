using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Aggregator.UserCards.Commands.Entities;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

internal sealed class UserCardItrToXfrMapper : IUserCardItrToXfrMapper
{
    private readonly IUserCardDetailsItrToXfrMapper _detailsMapper;

    public UserCardItrToXfrMapper() : this(new UserCardDetailsItrToXfrMapper())
    { }

    private UserCardItrToXfrMapper(IUserCardDetailsItrToXfrMapper detailsMapper)
    {
        _detailsMapper = detailsMapper;
    }

    public async Task<IUserCardXfrEntity> Map(IUserCardItrEntity source)
    {
        IEnumerable<Task<IUserCardDetailsXfrEntity>> detailsTasks = source.CollectedList.Select(_detailsMapper.Map);
        IUserCardDetailsXfrEntity[] details = await Task.WhenAll(detailsTasks).ConfigureAwait(false);

        return new UserCardXfrEntity
        {
            UserId = source.UserId,
            CardId = source.CardId,
            SetId = source.SetId,
            CollectedList = details
        };
    }
}
