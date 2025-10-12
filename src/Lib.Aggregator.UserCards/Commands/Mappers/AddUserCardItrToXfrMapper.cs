using System.Threading.Tasks;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Aggregator.UserCards.Commands.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

internal sealed class AddUserCardItrToXfrMapper : IAddUserCardItrToXfrMapper
{
    private readonly IUserCardDetailsItrToXfrMapper _mapper;

    public AddUserCardItrToXfrMapper() : this(new UserCardDetailsItrToXfrMapper())
    { }

    private AddUserCardItrToXfrMapper(IUserCardDetailsItrToXfrMapper mapper) => _mapper = mapper;

    public async Task<IAddUserCardXfrEntity> Map(IUserCardItrEntity source)
    {
        IUserCardDetailsXfrEntity details = await _mapper.Map(source.Details).ConfigureAwait(false);

        return new AddUserCardXfrEntity
        {
            UserId = source.UserId,
            CardId = source.CardId,
            SetId = source.SetId,
            ArtistIds = source.ArtistIds,
            CardNameGuid = source.CardNameGuid,
            Details = details
        };
    }
}
