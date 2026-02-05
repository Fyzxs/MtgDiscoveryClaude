using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Aggregator.UserCards.Entities;
using Lib.Aggregator.UserCards.Queries.UserCardsForSigning.Entities;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;

namespace Lib.Aggregator.UserCards.Queries.UserCardsForSigning.Mappers;

internal sealed class UserCardsToSigningResultMapper : IUserCardsToSigningResultMapper
{
    private const string SignedSpecial = "signed";

    public Task<ISigningResultOufEntity> Map(IEnumerable<UserCardExtEntity> userCards, IEnumerable<string> selectedArtistIds)
    {
        HashSet<string> selectedArtists = [.. selectedArtistIds];
        List<UserCardExtEntity> cardsList = [.. userCards];

        // Group cards by SetId
        IEnumerable<IGrouping<string, UserCardExtEntity>> cardsBySet = cardsList.GroupBy(c => c.SetId);

        List<ISigningSetGroupOufEntity> setGroups = [];

        foreach (IGrouping<string, UserCardExtEntity> setGroup in cardsBySet)
        {
            string setId = setGroup.Key;
            List<UserCardExtEntity> cardsInSet = [.. setGroup];

            // For each card, find which selected artists are on it
            // A card can have multiple artists, so we need to create artist groups
            Dictionary<string, List<UserCardExtEntity>> cardsByArtist = [];

            foreach (UserCardExtEntity card in cardsInSet)
            {
                // Find which selected artists are on this card
                IEnumerable<string> matchingArtists = card.ArtistIds.Where(a => selectedArtists.Contains(a));
                foreach (string artistId in matchingArtists)
                {
                    if (cardsByArtist.ContainsKey(artistId) is false)
                    {
                        cardsByArtist[artistId] = [];
                    }

                    cardsByArtist[artistId].Add(card);
                }
            }

            if (cardsByArtist.Count == 0)
            {
                continue;
            }

            List<ISigningArtistGroupOufEntity> artistGroups = [];
            int setUnsignedCardCount = 0;

            foreach (KeyValuePair<string, List<UserCardExtEntity>> artistGroup in cardsByArtist)
            {
                string artistId = artistGroup.Key;
                List<UserCardExtEntity> artistCards = artistGroup.Value;

                List<ISigningCardOufEntity> signingCards = [];
                int artistUnsignedCount = 0;
                int artistPartiallySignedCount = 0;

                foreach (UserCardExtEntity card in artistCards)
                {
                    (int unsignedCopies, bool isPartiallySigned, bool isFullySigned, List<IUserCardDetailsOufEntity> details) = CalculateSigningStats(card);

                    if (0 < unsignedCopies)
                    {
                        artistUnsignedCount++;
                    }

                    if (isPartiallySigned)
                    {
                        artistPartiallySignedCount++;
                    }

                    signingCards.Add(new SigningCardOufEntity
                    {
                        CardId = card.CardId,
                        CardName = card.CardName ?? string.Empty,
                        ImageUri = string.Empty, // Loaded on demand when expanded
                        UnsignedCopies = unsignedCopies,
                        IsPartiallySigned = isPartiallySigned,
                        IsFullySigned = isFullySigned,
                        CollectedDetails = details
                    });
                }

                if (signingCards.Count == 0)
                {
                    continue;
                }

                // Order cards: unsigned first, then partial, then fully signed
                List<ISigningCardOufEntity> orderedCards = [.. signingCards
                    .OrderByDescending(c => c.UnsignedCopies > 0 && c.IsPartiallySigned is false) // Unsigned only first
                    .ThenByDescending(c => c.IsPartiallySigned) // Then partial
                    .ThenBy(c => c.IsFullySigned) // Fully signed last
                    .ThenBy(c => c.CardName)];

                setUnsignedCardCount += artistUnsignedCount;

                // Get artist name from first card (all cards in this group have the same artist)
                string artistName = artistCards.FirstOrDefault()?.Artist ?? string.Empty;

                artistGroups.Add(new SigningArtistGroupOufEntity
                {
                    ArtistId = artistId,
                    ArtistName = artistName,
                    UnsignedCount = artistUnsignedCount,
                    PartiallySignedCount = artistPartiallySignedCount,
                    Cards = orderedCards
                });
            }

            if (artistGroups.Count == 0)
            {
                continue;
            }

            // Get set info from first card in the set (all cards have the same set info)
            UserCardExtEntity firstCard = cardsInSet.FirstOrDefault();
            string setCode = firstCard?.SetCode ?? string.Empty;
            string setName = firstCard?.SetName ?? string.Empty;
            string releasedAt = firstCard?.ReleasedAt ?? string.Empty;

            setGroups.Add(new SigningSetGroupOufEntity
            {
                SetId = setId,
                SetCode = setCode,
                SetName = setName,
                ArtistCount = artistGroups.Count,
                UnsignedCardCount = setUnsignedCardCount,
                ReleasedAt = releasedAt,
                Artists = artistGroups
            });
        }

        ISigningResultOufEntity result = new SigningResultOufEntity
        {
            Sets = setGroups
        };

        return Task.FromResult(result);
    }

    private static (int UnsignedCopies, bool IsPartiallySigned, bool IsFullySigned, List<IUserCardDetailsOufEntity> Details) CalculateSigningStats(UserCardExtEntity card)
    {
        List<UserCardDetailsExtEntity> collected = [.. card.CollectedList];

        bool hasSigned = collected.Any(c => c.Special == SignedSpecial);
        int unsignedCopies = collected
            .Where(c => c.Special != SignedSpecial)
            .Sum(c => c.Count);

        bool isPartiallySigned = hasSigned && 0 < unsignedCopies;
        bool isFullySigned = hasSigned && unsignedCopies == 0;

        List<IUserCardDetailsOufEntity> details = [.. collected
            .Select(c => new UserCardDetailsOufEntity
            {
                Finish = c.Finish,
                Special = c.Special,
                Count = c.Count
            })
            .Cast<IUserCardDetailsOufEntity>()];

        return (unsignedCopies, isPartiallySigned, isFullySigned, details);
    }
}
