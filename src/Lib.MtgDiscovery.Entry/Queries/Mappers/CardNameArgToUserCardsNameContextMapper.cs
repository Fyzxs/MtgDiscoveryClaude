using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class CardNameArgToUserCardsNameContextMapper : ICardNameArgToUserCardsNameContextMapper
{
    public Task<IUserCardsNameItrEntity> Map(ICardNameArgEntity cardName)
    {
        IUserCardsNameItrEntity context = new UserCardsNameItrEntity
        {
            UserId = cardName.UserId,
            CardName = cardName.CardName
        };

        return Task.FromResult(context);
    }
}
