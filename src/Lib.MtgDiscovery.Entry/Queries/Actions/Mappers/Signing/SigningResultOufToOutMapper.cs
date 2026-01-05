using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Signing;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers.Signing;

internal sealed class SigningResultOufToOutMapper : ISigningResultOufToOutMapper
{
    private const int VintageCutoffYear = 1996;

    public Task<SigningResultOutEntity> Map(ISigningResultOufEntity oufEntity)
    {
        List<SigningSetGroupOutEntity> allSets = oufEntity.Sets
            .Select(MapSetGroup)
            .ToList();

        // Separate vintage sets (1996 and earlier) from modern sets
        List<SigningSetGroupOutEntity> vintageSets = [];
        List<SigningSetGroupOutEntity> modernSets = [];

        foreach (SigningSetGroupOutEntity set in allSets)
        {
            if (IsVintageSet(set.ReleasedAt))
            {
                vintageSets.Add(set);
            }
            else
            {
                modernSets.Add(set);
            }
        }

        // Sort vintage sets by release date (ascending - oldest first)
        vintageSets = vintageSets
            .OrderBy(s => s.ReleasedAt)
            .ToList();

        // Sort modern sets by artist count, then unsigned count
        modernSets = modernSets
            .OrderByDescending(s => s.ArtistCount)
            .ThenByDescending(s => s.UnsignedCardCount)
            .ToList();

        // Combine: vintage sets first, then modern sets
        List<SigningSetGroupOutEntity> sortedSets = [.. vintageSets, .. modernSets];

        return Task.FromResult(new SigningResultOutEntity
        {
            Sets = sortedSets
        });
    }

    private static bool IsVintageSet(string releasedAt)
    {
        if (string.IsNullOrEmpty(releasedAt))
        {
            return false;
        }

        if (DateTime.TryParse(releasedAt, out DateTime releaseDate))
        {
            return releaseDate.Year < VintageCutoffYear + 1;
        }

        return false;
    }

    private static SigningSetGroupOutEntity MapSetGroup(ISigningSetGroupOufEntity setGroup)
    {
        List<SigningArtistGroupOutEntity> artists = setGroup.Artists
            .Select(MapArtistGroup)
            .ToList();

        return new SigningSetGroupOutEntity
        {
            SetId = setGroup.SetId,
            SetCode = setGroup.SetCode,
            SetName = setGroup.SetName,
            ArtistCount = setGroup.ArtistCount,
            UnsignedCardCount = setGroup.UnsignedCardCount,
            ReleasedAt = setGroup.ReleasedAt,
            Artists = artists
        };
    }

    private static SigningArtistGroupOutEntity MapArtistGroup(ISigningArtistGroupOufEntity artistGroup)
    {
        List<SigningCardOutEntity> cards = artistGroup.Cards
            .Select(MapCard)
            .ToList();

        return new SigningArtistGroupOutEntity
        {
            ArtistId = artistGroup.ArtistId,
            ArtistName = artistGroup.ArtistName,
            UnsignedCount = artistGroup.UnsignedCount,
            PartiallySignedCount = artistGroup.PartiallySignedCount,
            Cards = cards
        };
    }

    private static SigningCardOutEntity MapCard(ISigningCardOufEntity card)
    {
        List<CollectedItemOutEntity> details = card.CollectedDetails
            .Select(d => new CollectedItemOutEntity
            {
                Finish = d.Finish,
                Special = d.Special,
                Count = d.Count
            })
            .ToList();

        return new SigningCardOutEntity
        {
            CardId = card.CardId,
            CardName = card.CardName,
            ImageUri = card.ImageUri,
            UnsignedCopies = card.UnsignedCopies,
            IsPartiallySigned = card.IsPartiallySigned,
            IsFullySigned = card.IsFullySigned,
            CollectedDetails = details
        };
    }
}
