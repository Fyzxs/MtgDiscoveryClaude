using System.Collections.Generic;
using Lib.Scryfall.Shared.Apis.Models;
using Lib.Universal.Primitives;

namespace Lib.Scryfall.Ingestion.Models;

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

}
