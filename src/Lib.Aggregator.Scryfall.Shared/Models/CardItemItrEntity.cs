using System.Collections.Generic;
using System.Linq;
using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Shared.DataModels.Entities;
using Newtonsoft.Json.Linq;

namespace Lib.Aggregator.Scryfall.Shared.Models;

public sealed class CardItemItrEntity : ICardItemItrEntity
{
    private readonly dynamic _data;

    public CardItemItrEntity(ScryfallCardItem scryfallCard)
    {
        _data = scryfallCard?.Data;
    }

    public string Id => _data?.id;
    public string OracleId => _data?.oracle_id;
    public int? ArenaId => _data?.arena_id;
    public int? MtgoId => _data?.mtgo_id;
    public int? MtgoFoilId => _data?.mtgo_foil_id;
    public ICollection<int> MultiverseIds => ConvertToIntArray(_data?.multiverse_ids);
    public int? TcgPlayerId => _data?.tcgplayer_id;
    public int? TcgPlayerEtchedId => _data?.tcgplayer_etched_id;
    public int? CardMarketId => _data?.cardmarket_id;
    public string Name => _data?.name;
    public string FlavorName => _data?.flavor_name;
    public string Lang => _data?.lang;
    public string ReleasedAt => _data?.released_at;
    public string Uri => _data?.uri;
    public string ScryfallUri => _data?.scryfall_uri;
    public string Layout => _data?.layout;
    public bool HighResImage => _data?.highres_image ?? false;
    public string ImageStatus => _data?.image_status;
    public IImageUrisItrEntity ImageUris => _data?.image_uris != null ? new ImageUrisItrEntity(_data.image_uris) : null;
    public string ManaCost => _data?.mana_cost;
    public decimal? Cmc => _data?.cmc;
    public string TypeLine => _data?.type_line;
    public string OracleText => _data?.oracle_text;
    public ICollection<string> Colors => ConvertToStringArray(_data?.colors);
    public ICollection<string> ColorIdentity => ConvertToStringArray(_data?.color_identity);
    public ICollection<string> Keywords => ConvertToStringArray(_data?.keywords);
    public ILegalitiesItrEntity Legalities => _data?.legalities != null ? new LegalitiesItrEntity(_data.legalities) : null;
    public ICollection<string> Games => ConvertToStringArray(_data?.games);
    public bool Reserved => _data?.reserved ?? false;
    public bool GameChanger => _data?.game_changer ?? false;
    public bool Foil => _data?.foil ?? false;
    public bool NonFoil => _data?.nonfoil ?? false;
    public ICollection<string> Finishes => ConvertToStringArray(_data?.finishes);
    public bool Oversized => _data?.oversized ?? false;
    public bool Promo => _data?.promo ?? false;
    public bool Reprint => _data?.reprint ?? false;
    public bool Variation => _data?.variation ?? false;
    public string SetId => _data?.set_id;
    public string SetCode => _data?.set;
    public string SetName => _data?.set_name;
    public string SetType => _data?.set_type;
    public string SetUri => _data?.set_uri;
    public string SetSearchUri => _data?.set_search_uri;
    public string ScryfallSetUri => _data?.scryfall_set_uri;
    public string RulingsUri => _data?.rulings_uri;
    public string PrintsSearchUri => _data?.prints_search_uri;
    public string CollectorNumber => _data?.collector_number;
    public bool Digital => _data?.digital ?? false;
    public string Rarity => _data?.rarity;
    public string FlavorText => _data?.flavor_text;
    public string CardBackId => _data?.card_back_id;
    public string Artist => _data?.artist;
    public ICollection<string> ArtistIds => ConvertToStringArray(_data?.artist_ids);
    public string IllustrationId => _data?.illustration_id;
    public string BorderColor => _data?.border_color;
    public string Frame => _data?.frame;
    public ICollection<string> FrameEffects => ConvertToStringArray(_data?.frame_effects);
    public string SecurityStamp => _data?.security_stamp;
    public bool FullArt => _data?.full_art ?? false;
    public bool Textless => _data?.textless ?? false;
    public bool Booster => _data?.booster ?? false;
    public bool StorySpotlight => _data?.story_spotlight ?? false;
    public ICollection<string> PromoTypes => ConvertToStringArray(_data?.promo_types);
    public int? EdhRecRank => _data?.edhrec_rank;
    public int? PennyRank => _data?.penny_rank;
    public IPricesItrEntity Prices => _data?.prices != null ? new PricesItrEntity(_data.prices) : null;
    public IRelatedUrisItrEntity RelatedUris => _data?.related_uris != null ? new RelatedUrisItrEntity(_data.related_uris) : null;
    public IPurchaseUrisItrEntity PurchaseUris => _data?.purchase_uris != null ? new PurchaseUrisItrEntity(_data.purchase_uris) : null;
    public string Power => _data?.power;
    public string Toughness => _data?.toughness;
    public string Loyalty => _data?.loyalty;
    public string Defense => _data?.defense;
    public string LifeModifier => _data?.life_modifier;
    public string HandModifier => _data?.hand_modifier;
    public ICollection<string> ColorIndicator => ConvertToStringArray(_data?.color_indicator);
    public ICollection<ICardFaceItrEntity> CardFaces => MapCardFaces(_data?.card_faces);
    public ICollection<IAllPartsItrEntity> AllParts => MapAllParts(_data?.all_parts);
    public string PrintedName => _data?.printed_name;
    public string PrintedTypeLine => _data?.printed_type_line;
    public string PrintedText => _data?.printed_text;
    public string Watermark => _data?.watermark;
    public int? ContentWarning => _data?.content_warning;
    public IPreviewItrEntity Preview => _data?.preview != null ? new PreviewItrEntity(_data.preview) : null;
    public ICollection<string> ProducedMana => ConvertToStringArray(_data?.produced_mana);
    public ICollection<int> AttractionLights => ConvertToIntArray(_data?.attraction_lights);

    private static ICollection<int> ConvertToIntArray(dynamic value)
    {
        if (value == null) return null;
        if (value is JArray jArray)
        {
            return jArray.Select(x => (int)x).ToArray();
        }
        return null;
    }

    private static ICollection<string> ConvertToStringArray(dynamic value)
    {
        if (value == null) return null;
        if (value is JArray jArray)
        {
            return jArray.Select(x => (string)x).ToArray();
        }
        return null;
    }

    private static ICollection<ICardFaceItrEntity> MapCardFaces(dynamic cardFaces)
    {
        if (cardFaces == null) return null;
        if (cardFaces is JArray jArray)
        {
            ICardFaceItrEntity[] result = new ICardFaceItrEntity[jArray.Count];
            for (int i = 0; i < jArray.Count; i++)
            {
                result[i] = new CardFaceItrEntity(jArray[i]);
            }
            return result;
        }
        return null;
    }

    private static ICollection<IAllPartsItrEntity> MapAllParts(dynamic allParts)
    {
        if (allParts == null) return null;
        if (allParts is JArray jArray)
        {
            IAllPartsItrEntity[] result = new IAllPartsItrEntity[jArray.Count];
            for (int i = 0; i < jArray.Count; i++)
            {
                result[i] = new AllPartsItrEntity(jArray[i]);
            }
            return result;
        }
        return null;
    }
}