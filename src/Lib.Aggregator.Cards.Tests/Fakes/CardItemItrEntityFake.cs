using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal sealed class CardItemItrEntityFake : ICardItemItrEntity
{
    public string Id { get; init; } = string.Empty;
    public string OracleId { get; init; } = string.Empty;
    public int? ArenaId { get; }
    public int? MtgoId { get; }
    public int? MtgoFoilId { get; }
    public ICollection<int> MultiverseIds { get; init; } = [];
    public int? TcgPlayerId { get; init; }
    public int? TcgPlayerEtchedId { get; }
    public int? CardMarketId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string FlavorName { get; }
    public string Lang { get; init; } = string.Empty;
    public string ReleasedAt { get; init; } = string.Empty;
    public string Uri { get; init; } = string.Empty;
    public string ScryfallUri { get; init; } = string.Empty;
    public string Layout { get; init; } = string.Empty;
    public bool HighResImage { get; init; }
    public string ImageStatus { get; init; } = string.Empty;
    public IImageUrisItrEntity ImageUris { get; init; } = default!;
    public string ManaCost { get; init; } = string.Empty;
    public decimal? Cmc { get; init; }
    public string TypeLine { get; init; } = string.Empty;
    public string OracleText { get; init; } = string.Empty;
    public ICollection<string> Colors { get; init; } = [];
    public ICollection<string> ColorIdentity { get; init; } = [];
    public ICollection<string> Keywords { get; init; } = [];
    public ILegalitiesItrEntity Legalities { get; init; } = default!;
    public ICollection<string> Games { get; init; } = [];
    public bool Reserved { get; init; }
    public bool GameChanger { get; }
    public bool Foil { get; init; }
    public bool NonFoil { get; init; }
    public ICollection<string> Finishes { get; init; } = [];
    public bool Oversized { get; init; }
    public bool Promo { get; init; }
    public bool Reprint { get; init; }
    public bool Variation { get; init; }
    public string SetId { get; init; } = string.Empty;
    public string SetCode { get; init; } = string.Empty;
    public string SetName { get; init; } = string.Empty;
    public string SetType { get; init; } = string.Empty;
    public string SetUri { get; init; } = string.Empty;
    public string SetSearchUri { get; init; } = string.Empty;
    public string ScryfallSetUri { get; init; } = string.Empty;
    public string RulingsUri { get; init; } = string.Empty;
    public string PrintsSearchUri { get; init; } = string.Empty;
    public string CollectorNumber { get; init; } = string.Empty;
    public bool Digital { get; init; }
    public string Rarity { get; init; } = string.Empty;
    public string FlavorText { get; init; } = string.Empty;
    public string CardBackId { get; init; } = string.Empty;
    public string Artist { get; init; } = string.Empty;
    public ICollection<string> ArtistIds { get; init; } = [];
    public string IllustrationId { get; init; } = string.Empty;
    public string BorderColor { get; init; } = string.Empty;
    public string Frame { get; init; } = string.Empty;
    public ICollection<string> FrameEffects { get; init; } = [];
    public string SecurityStamp { get; init; } = string.Empty;
    public bool FullArt { get; init; }
    public bool Textless { get; init; }
    public bool Booster { get; init; }
    public bool StorySpotlight { get; init; }
    public ICollection<string> PromoTypes { get; }
    public int? EdhRecRank { get; init; }
    public int? PennyRank { get; init; }
    public IPricesItrEntity Prices { get; init; } = default!;
    public IRelatedUrisItrEntity RelatedUris { get; init; } = default!;
    public IPurchaseUrisItrEntity PurchaseUris { get; init; } = default!;
    public string Power { get; init; } = string.Empty;
    public string Toughness { get; init; } = string.Empty;
    public string Loyalty { get; init; } = string.Empty;
    public string Defense { get; init; } = string.Empty;
    public string LifeModifier { get; init; } = string.Empty;
    public string HandModifier { get; init; } = string.Empty;
    public ICollection<string> ColorIndicator { get; init; } = [];
    public ICollection<ICardFaceItrEntity> CardFaces { get; init; } = [];
    public ICollection<IAllPartsItrEntity> AllParts { get; init; } = [];
    public string PrintedName { get; init; } = string.Empty;
    public string PrintedTypeLine { get; init; } = string.Empty;
    public string PrintedText { get; init; } = string.Empty;
    public string Watermark { get; init; } = string.Empty;
    public int? ContentWarning { get; init; }
    public IPreviewItrEntity Preview { get; init; } = default!;
    public ICollection<string> ProducedMana { get; init; } = [];
    public ICollection<int> AttractionLights { get; }
    public ICollection<string> Attractions { get; init; } = [];
}
