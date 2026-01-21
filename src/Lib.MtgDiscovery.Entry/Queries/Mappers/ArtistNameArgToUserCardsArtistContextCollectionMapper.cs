using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class ArtistNameArgToUserCardsArtistContextCollectionMapper : IArtistNameArgToUserCardsArtistContextCollectionMapper
{
    public Task<IEnumerable<IUserCardsArtistItrEntity>> Map(IArtistNameArgEntity artistName, List<CardItemOutEntity> outEntities)
    {
        // Extract all unique artist IDs from the returned cards
        IEnumerable<string> artistIds = outEntities
            .Where(card => card.ArtistIds is not null)
            .SelectMany(card => card.ArtistIds)
            .Distinct();

        List<IUserCardsArtistItrEntity> contexts = [];

        // Create a context for each unique artist ID
        foreach (string artistId in artistIds)
        {
            IUserCardsArtistItrEntity context = new UserCardsArtistItrEntity
            {
                UserId = artistName.UserId,
                ArtistId = artistId
            };
            contexts.Add(context);
        }

        return Task.FromResult<IEnumerable<IUserCardsArtistItrEntity>>(contexts);
    }
}
