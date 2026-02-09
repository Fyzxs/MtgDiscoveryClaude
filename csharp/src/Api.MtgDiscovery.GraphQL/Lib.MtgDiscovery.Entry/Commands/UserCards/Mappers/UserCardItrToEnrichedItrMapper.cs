using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;

namespace Lib.MtgDiscovery.Entry.Commands.UserCards.Mappers;

internal sealed class UserCardItrToEnrichedItrMapper : IUserCardItrToEnrichedItrMapper
{
    public Task<IUserCardItrEntity> Map(IUserCardItrEntity itrEntity, CardItemOutEntity cardItem, string cardNameGuid)
    {
        IEnumerable<string> artistIds = cardItem.ArtistIds ?? [];

        IUserCardItrEntity result = new UserCardCollectionItrEntity
        {
            UserId = itrEntity.UserId,
            CardId = itrEntity.CardId,
            SetId = itrEntity.SetId,
            CardName = cardItem.Name,
            SetName = cardItem.SetName,
            SetCode = cardItem.SetCode,
            ReleasedAt = cardItem.ReleasedAt,
            Artist = cardItem.Artist,
            ArtistIds = artistIds,
            CardNameGuid = cardNameGuid,
            Details = itrEntity.Details,
            ReplaceMode = itrEntity.ReplaceMode
        };

        return Task.FromResult(result);
    }
}
