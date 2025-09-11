using System.Collections.Generic;
using System.Linq;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Scryfall.Shared.Internals;

internal sealed class CardFaceItrEntity : ICardFaceItrEntity
{
    private readonly dynamic _data;

    public CardFaceItrEntity(dynamic data) => _data = data;
    public string Name => _data?.name;
    public string ManaCost => _data?.mana_cost;
    public string TypeLine => _data?.type_line;
    public string OracleText => _data?.oracle_text;
    public ICollection<string> Colors => ConvertToStringArray(_data?.colors);
    public ICollection<string> ColorIndicator => ConvertToStringArray(_data?.color_indicator);
    public string Power => _data?.power;
    public string Toughness => _data?.toughness;
    public string Loyalty => _data?.loyalty;
    public string Defense => _data?.defense;
    public string Artist => _data?.artist;
    public string ArtistId => _data?.artist_id;
    public string IllustrationId => _data?.illustration_id;
    public IImageUrisItrEntity ImageUris => _data?.image_uris != null ? new ImageUrisItrEntity(_data.image_uris) : null;
    public string FlavorText => _data?.flavor_text;
    public string PrintedName => _data?.printed_name;
    public string PrintedTypeLine => _data?.printed_type_line;
    public string PrintedText => _data?.printed_text;
    public string Watermark => _data?.watermark;
    public string Layout => _data?.layout;
    public decimal? Cmc => _data?.cmc;

    private static ICollection<string> ConvertToStringArray(dynamic value)
    {
        if (value == null) return null;
        if (value is Newtonsoft.Json.Linq.JArray jArray)
        {
            return jArray.Select(x => (string)x).ToArray();
        }
        return null;
    }
}
