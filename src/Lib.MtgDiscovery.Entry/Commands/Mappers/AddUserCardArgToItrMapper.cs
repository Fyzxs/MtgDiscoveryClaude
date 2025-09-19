using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Commands.Mappers;

internal sealed class AddUserCardArgToItrMapper : IAddUserCardArgToItrMapper
{
    private readonly IUserCardDetailsArgToItrMapper _mapper;

    public AddUserCardArgToItrMapper() : this(new UserCardDetailsArgToItrMapper())
    { }

    private AddUserCardArgToItrMapper(IUserCardDetailsArgToItrMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IUserCardItrEntity> Map(IAuthUserArgEntity source1, IAddUserCardArgEntity source2)
    {
        IUserCardDetailsItrEntity mappedDetails = await _mapper.Map(source2.UserCardDetails).ConfigureAwait(false);

        return new UserCardCollectionItrEntity
        {
            UserId = source1.UserId,
            CardId = source2.CardId,
            SetId = source2.SetId,
            CollectedList = [mappedDetails]
        };
    }
}
