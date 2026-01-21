using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Adapter.Cards.Queries.Entities;

internal sealed class CardItemItrEntity : ICardItemItrEntity
{
    public string Id { get; init; }
    public string OracleId { get; init; }
    public int? ArenaId { get; init; }
    public int? MtgoId { get; init; }
    public int? MtgoFoilId { get; init; }
    public ICollection<int> MultiverseIds { get; init; }
    public int? TcgPlayerId { get; init; }
    public int? TcgPlayerEtchedId { get; init; }
    public int? CardMarketId { get; init; }
    public string Name { get; init; }
    public string FlavorName { get; init; }
    public string Lang { get; init; }
    public string ReleasedAt { get; init; }
    public string Uri { get; init; }
    public string ScryfallUri { get; init; }
    public string Layout { get; init; }
    public bool HighResImage { get; init; }
    public string ImageStatus { get; init; }
    public IImageUrisItrEntity ImageUris { get; init; }
    public string ManaCost { get; init; }
    public decimal? Cmc { get; init; }
    public string TypeLine { get; init; }
    public string OracleText { get; init; }
    public ICollection<string> Colors { get; init; }
    public ICollection<string> ColorIdentity { get; init; }
    public ICollection<string> Keywords { get; init; }
    public ILegalitiesItrEntity Legalities { get; init; }
    public ICollection<string> Games { get; init; }
    public bool Reserved { get; init; }
    public bool GameChanger { get; init; }
    public bool Foil { get; init; }
    public bool NonFoil { get; init; }
    public ICollection<string> Finishes { get; init; }
    public bool Oversized { get; init; }
    public bool Promo { get; init; }
    public bool Reprint { get; init; }
    public bool Variation { get; init; }
    public string SetId { get; init; }
    public string SetCode { get; init; }
    public string SetName { get; init; }
    public string SetType { get; init; }
    public string SetUri { get; init; }
    public string SetSearchUri { get; init; }
    public string ScryfallSetUri { get; init; }
    public string RulingsUri { get; init; }
    public string PrintsSearchUri { get; init; }
    public string CollectorNumber { get; init; }
    public bool Digital { get; init; }
    public string Rarity { get; init; }
    public string FlavorText { get; init; }
    public string CardBackId { get; init; }
    public string Artist { get; init; }
    public ICollection<string> ArtistIds { get; init; }
    public string IllustrationId { get; init; }
    public string BorderColor { get; init; }
    public string Frame { get; init; }
    public ICollection<string> FrameEffects { get; init; }
    public string SecurityStamp { get; init; }
    public bool FullArt { get; init; }
    public bool Textless { get; init; }
    public bool Booster { get; init; }
    public bool StorySpotlight { get; init; }
    public ICollection<string> PromoTypes { get; init; }
    public int? EdhRecRank { get; init; }
    public int? PennyRank { get; init; }
    public IPricesItrEntity Prices { get; init; }
    public IRelatedUrisItrEntity RelatedUris { get; init; }
    public IPurchaseUrisItrEntity PurchaseUris { get; init; }
    public string Power { get; init; }
    public string Toughness { get; init; }
    public string Loyalty { get; init; }
    public string Defense { get; init; }
    public string LifeModifier { get; init; }
    public string HandModifier { get; init; }
    public ICollection<string> ColorIndicator { get; init; }
    public ICollection<ICardFaceItrEntity> CardFaces { get; init; }
    public ICollection<IAllPartsItrEntity> AllParts { get; init; }
    public string PrintedName { get; init; }
    public string PrintedTypeLine { get; init; }
    public string PrintedText { get; init; }
    public string Watermark { get; init; }
    public int? ContentWarning { get; init; }
    public IPreviewItrEntity Preview { get; init; }
    public ICollection<string> ProducedMana { get; init; }
    public ICollection<int> AttractionLights { get; init; }
}
