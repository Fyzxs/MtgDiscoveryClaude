using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.Entities;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Commands.Mappers;

internal sealed class AddUserCardArgToItrMapper : IAddUserCardArgToItrMapper
{
    private readonly IUserCardDetailsArgToItrMapper _mapper;

    public AddUserCardArgToItrMapper() : this(new UserCardDetailsArgToItrMapper())
    { }

    private AddUserCardArgToItrMapper(IUserCardDetailsArgToItrMapper mapper) => _mapper = mapper;

    public async Task<IUserCardItrEntity> Map(IAddCardToCollectionArgsEntity source)
    {
        IUserCardDetailsItrEntity mappedDetails = await _mapper.Map(source.AddUserCard.UserCardDetails).ConfigureAwait(false);

        return new UserCardCollectionItrEntity
        {
            UserId = source.AuthUser.UserId,
            CardId = source.AddUserCard.CardId,
            SetId = source.AddUserCard.SetId,
            CollectedList = [mappedDetails]
        };
    }
}
