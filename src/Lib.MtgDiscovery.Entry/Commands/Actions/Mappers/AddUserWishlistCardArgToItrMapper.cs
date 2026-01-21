using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.Entities;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Mappers;

internal sealed class AddUserWishlistCardArgToItrMapper : IAddUserWishlistCardArgToItrMapper
{
    private readonly IUserWishlistCardDetailsArgToItrMapper _mapper;

    public AddUserWishlistCardArgToItrMapper() : this(new UserWishlistCardDetailsArgToItrMapper())
    { }

    private AddUserWishlistCardArgToItrMapper(IUserWishlistCardDetailsArgToItrMapper mapper) => _mapper = mapper;

    public async Task<IUserWishlistCardItrEntity> Map(IAddCardToWishlistArgsEntity source)
    {
        IUserWishlistCardDetailsItrEntity mappedDetails = await _mapper.Map(source.AddUserWishlistCard.UserWishlistCardDetails).ConfigureAwait(false);

        return new UserWishlistCardItrEntity
        {
            UserId = source.AuthUser.UserId,
            CardId = source.AddUserWishlistCard.CardId,
            SetId = source.AddUserWishlistCard.SetId,
            Details = mappedDetails
        };
    }
}
