using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Lib.BlobStorage.Apis.Ids;
using Lib.Scryfall.Ingestion.Dtos;
using Lib.Scryfall.Shared.Apis.Models;
using Lib.Universal.Primitives;

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
        List<string> artistIds = new();

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
}

internal sealed class CardImageInfoCollection : ICardImageInfoCollection
{
    private readonly Collection<ICardImageInfo> _imageInfos;

    public CardImageInfoCollection(Collection<ICardImageInfo> imageInfos) => _imageInfos = imageInfos;

    public IEnumerator<ICardImageInfo> GetEnumerator() => _imageInfos.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

internal sealed class CardImageInfo : ICardImageInfo
{
    private readonly IScryfallCard _card;
    private readonly string _side;
    private readonly string _imageType;
    private readonly string _uri;

    public CardImageInfo(IScryfallCard card, string side, string imageType, string uri)
    {
        _card = card;
        _side = side;
        _imageType = imageType;
        _uri = uri;
    }

    public Url ImageUrl() => new ProvidedUrl(_uri);

    public string StoragePath()
    {
        string cardId = _card.Id();
        string first = cardId[..1];
        string second = cardId.Substring(1, 1);
        return new ProvidedBlobPathEntity($"{_imageType}/{_side}/{first}/{second}/{cardId}.jpg");
    }

    public IDictionary<string, string> Metadata()
    {
        return new Dictionary<string, string>
        {
            { "card_id", _card.Id() },
            { "card_name", _card.Name() },
            { "set_id", _card.Set().Id() },
            { "set_code", _card.Set().Code() },
            { "side", _side},
            { "image_type", _imageType },
            { "content_type", "image/jpeg" }
        };
    }

    public string LogValue() => $"[{_card.Name()}/{_card.Set().Code()} | {_side}/{_imageType}]";

    string ICardImageInfo.StoragePath()
    {
        return StoragePath();
    }
}
