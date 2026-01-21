using System.Collections.Generic;
using System.Collections.ObjectModel;
using Lib.Scryfall.Ingestion.Dtos;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Models;

internal sealed class ScryfallCard : IScryfallCard
{
    private readonly ExtScryfallCardDto _dto;
    private readonly IScryfallSet _set;

    public ScryfallCard(ExtScryfallCardDto dto, IScryfallSet set)
    {
        _dto = dto;
        _set = set;
    }

    public string Id() => _dto.Data.id;
    public string Name() => _dto.Data.name;

    public string FlavorName()
    {
        try
        {
            return _dto.Data.flavor_name ?? string.Empty;
        }
        catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
        {
            return string.Empty;
        }
    }

    public dynamic Data() => _dto.Data;

    public IScryfallSet Set() => _set;

    public ICardImageInfoCollection ImageUris()
    {
        Collection<ICardImageInfo> imageInfos = [];

        try
        {
            ProcessSingleFace(imageInfos);
            ProcessMultiFace(imageInfos);
        }
        catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
        {

        }

        return new CardImageInfoCollection(imageInfos);
    }

    private void ProcessMultiFace(Collection<ICardImageInfo> imageInfos)
    {
        if (HasMultipleFaces() is false) return;

        dynamic cardFaces = _dto.Data.card_faces;
        for (int i = 0; i < cardFaces.Count; i++)
        {
            dynamic face = cardFaces[i];

            if (face.image_uris == null) continue;

            string side = i == 0 ? "front" : "back";

            AddCardImageInfo(imageInfos, face.image_uris, side);
        }
    }

    private void ProcessSingleFace(Collection<ICardImageInfo> imageInfos)
    {
        if (HasSingleFace() is false) return;
        AddCardImageInfo(imageInfos, _dto.Data.image_uris, "front");
    }

    private void AddCardImageInfo(Collection<ICardImageInfo> imageInfos, dynamic imageUris, string side)
    {
        if (imageUris.small != null) imageInfos.Add(new CardImageInfo(this, side, "small", (string)imageUris.small));
        if (imageUris.normal != null) imageInfos.Add(new CardImageInfo(this, side, "normal", (string)imageUris.normal));
        if (imageUris.large != null) imageInfos.Add(new CardImageInfo(this, side, "large", (string)imageUris.large));
        if (imageUris.art_crop != null) imageInfos.Add(new CardImageInfo(this, side, "art_crop", (string)imageUris.art_crop));
        if (imageUris.border_crop != null) imageInfos.Add(new CardImageInfo(this, side, "border_crop", (string)imageUris.border_crop));
    }

    public bool HasMultipleFaces()
    {
        try
        {
            dynamic cardFaces = _dto.Data.card_faces;
            if (cardFaces == null) return false;
            return cardFaces.Count > 0;
        }
        catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
        {
            return false;
        }
    }

    public bool HasSingleFace() => HasMultipleFaces() is false;

    public IEnumerable<string> ArtistIds()
    {
        List<string> artistIds = [];

        try
        {
            dynamic artistIdsData = _dto.Data.artist_ids;
            if (artistIdsData == null) return artistIds;

            foreach (string artistId in artistIdsData)
            {
                if (string.IsNullOrWhiteSpace(artistId) is false)
                {
                    artistIds.Add(artistId);
                }
            }
        }
        catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
        {
            // Card has no artist_ids field
        }

        return artistIds;
    }

    public IEnumerable<IArtistIdNamePair> ArtistIdNamePairs()
    {
        List<IArtistIdNamePair> pairs = [];
        List<string> artistIds = [.. ArtistIds()];
        List<string> artistNames = ExtractArtistNames();

        for (int i = 0; i < artistIds.Count; i++)
        {
            string artistId = artistIds[i];
            string artistName = i < artistNames.Count ? artistNames[i] : string.Empty;
            pairs.Add(new ArtistIdNamePair
            {
                ArtistId = artistId,
                ArtistName = artistName
            });
        }

        return pairs;
    }

    private List<string> ExtractArtistNames()
    {
        List<string> names = [];

        try
        {
            dynamic artistField = _dto.Data.artist;
            if (artistField == null) return names;

            string artistString = (string)artistField;

            // Handle multiple artists separated by " & " or " and "
            string[] separators = { " & ", " and " };
            string[] artistNames = artistString.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string name in artistNames)
            {
                names.Add(name.Trim());
            }
        }
        catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
        {
            // Card has no artist field
        }

        return names;
    }
}
