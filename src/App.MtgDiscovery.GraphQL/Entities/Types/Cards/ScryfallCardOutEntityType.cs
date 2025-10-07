using App.MtgDiscovery.GraphQL.Entities.Types.UserCards;
using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.Cards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Cards;

internal sealed class ScryfallCardOutEntityType : ObjectType<CardItemOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<CardItemOutEntity> descriptor)
    {
        descriptor.Name("Card")
             .Description("Represents a Magic: The Gathering card from Scryfall");
        descriptor.Field(f => f.Id)
            .Name("id")
            .Type<NonNullType<StringType>>()
             .Description("The unique Scryfall identifier of the card");
        descriptor.Field(f => f.OracleId)
            .Name("oracleId")
            .Type<StringType>()
            .Description("The Oracle ID for this card");
        descriptor.Field(f => f.ArenaId)
            .Name("arenaId")
            .Type<IntType>()
            .Description("The MTG Arena ID");
        descriptor.Field(f => f.MtgoId)
            .Name("mtgoId")
            .Type<IntType>()
            .Description("The Magic Online ID");
        descriptor.Field(f => f.MtgoFoilId)
            .Name("mtgoFoilId")
            .Type<IntType>()
            .Description("The Magic Online foil ID");
        descriptor.Field(f => f.MultiverseIds)
            .Name("multiverseIds")
            .Type<ListType<IntType>>()
            .Description("The multiverse IDs on Gatherer");
        descriptor.Field(f => f.TcgPlayerId)
            .Name("tcgPlayerId")
            .Type<IntType>()
            .Description("The TCGPlayer ID");
        descriptor.Field(f => f.TcgPlayerEtchedId)
            .Name("tcgPlayerEtchedId")
            .Type<IntType>()
            .Description("The TCGPlayer etched foil ID");
        descriptor.Field(f => f.CardMarketId)
            .Name("cardMarketId")
            .Type<IntType>()
            .Description("The Cardmarket ID");
        descriptor.Field(f => f.Name)
            .Name("name")
            .Type<StringType>()
            .Description("The name of the card");
        descriptor.Field(f => f.FlavorName)
            .Name("flavorName")
            .Type<StringType>()
            .Description("The flavor name of the card");
        descriptor.Field(f => f.Lang)
            .Name("lang")
            .Type<StringType>()
            .Description("The language code for this printing");
        descriptor.Field(f => f.ReleasedAt)
            .Name("releasedAt")
            .Type<StringType>()
            .Description("The date this card was released");
        descriptor.Field(f => f.Uri)
            .Name("uri")
            .Type<StringType>()
            .Description("The Scryfall API URI for this card");
        descriptor.Field(f => f.ScryfallUri)
            .Name("scryfallUri")
            .Type<StringType>()
            .Description("The Scryfall web page for this card");
        descriptor.Field(f => f.Layout)
            .Name("layout")
            .Type<StringType>()
            .Description("The card layout");
        descriptor.Field(f => f.HighResImage)
            .Name("highResImage")
            .Type<BooleanType>()
            .Description("Whether high-resolution images are available");
        descriptor.Field(f => f.ImageStatus)
            .Name("imageStatus")
            .Type<StringType>()
            .Description("The status of image availability");
        descriptor.Field(f => f.ImageUris)
            .Name("imageUris")
            .Type<ScryfallImageUrisOutEntityType>()
             .Description("URIs to images of the card");
        descriptor.Field(f => f.ManaCost)
            .Name("manaCost")
            .Type<StringType>()
            .Description("The mana cost");
        descriptor.Field(f => f.Cmc)
            .Name("cmc")
            .Type<FloatType>()
            .Description("The converted mana cost");
        descriptor.Field(f => f.TypeLine)
            .Name("typeLine")
            .Type<StringType>()
            .Description("The type line");
        descriptor.Field(f => f.OracleText)
            .Name("oracleText")
            .Type<StringType>()
            .Description("The Oracle text");
        descriptor.Field(f => f.Power)
            .Name("power")
            .Type<StringType>()
            .Description("The power (creatures only)");
        descriptor.Field(f => f.Toughness)
            .Name("toughness")
            .Type<StringType>()
            .Description("The toughness (creatures only)");
        descriptor.Field(f => f.Loyalty)
            .Name("loyalty")
            .Type<StringType>()
            .Description("The loyalty (planeswalkers only)");
        descriptor.Field(f => f.Defense)
            .Name("defense")
            .Type<StringType>()
            .Description("The defense (battles only)");
        descriptor.Field(f => f.LifeModifier)
            .Name("lifeModifier")
            .Type<StringType>()
            .Description("The life modifier (Vanguard only)");
        descriptor.Field(f => f.HandModifier)
            .Name("handModifier")
            .Type<StringType>()
            .Description("The hand modifier (Vanguard only)");
        descriptor.Field(f => f.Colors)
            .Name("colors")
            .Type<ListType<StringType>>()
            .Description("The card colors");
        descriptor.Field(f => f.ColorIdentity)
            .Name("colorIdentity")
            .Type<ListType<StringType>>()
            .Description("The color identity");
        descriptor.Field(f => f.ColorIndicator)
            .Name("colorIndicator")
            .Type<ListType<StringType>>()
            .Description("The color indicator");
        descriptor.Field(f => f.Keywords)
            .Name("keywords")
            .Type<ListType<StringType>>()
            .Description("Keywords on this card");
        descriptor.Field(f => f.Legalities)
            .Name("legalities")
            .Type<ScryfallLegalitiesOutEntityType>()
             .Description("Format legalities");
        descriptor.Field(f => f.Games)
            .Name("games")
            .Type<ListType<StringType>>()
            .Description("Games this card appears in");
        descriptor.Field(f => f.Reserved)
            .Name("reserved")
            .Type<BooleanType>()
            .Description("Whether this card is on the reserved list");
        descriptor.Field(f => f.GameChanger)
            .Name("gameChanger")
            .Type<BooleanType>()
            .Description("Whether this card is a game changer");
        descriptor.Field(f => f.Foil)
            .Name("foil")
            .Type<BooleanType>()
            .Description("Whether this card is available in foil");
        descriptor.Field(f => f.NonFoil)
            .Name("nonFoil")
            .Type<BooleanType>()
            .Description("Whether this card is available in non-foil");
        descriptor.Field(f => f.Finishes)
            .Name("finishes")
            .Type<ListType<StringType>>()
            .Description("Available finishes");
        descriptor.Field(f => f.Oversized)
            .Name("oversized")
            .Type<BooleanType>()
            .Description("Whether this card is oversized");
        descriptor.Field(f => f.Promo)
            .Name("promo")
            .Type<BooleanType>()
            .Description("Whether this card is a promotional printing");
        descriptor.Field(f => f.Reprint)
            .Name("reprint")
            .Type<BooleanType>()
            .Description("Whether this card is a reprint");
        descriptor.Field(f => f.Variation)
            .Name("variation")
            .Type<BooleanType>()
            .Description("Whether this card is a variation");
        descriptor.Field(f => f.SetId)
            .Name("setId")
            .Type<StringType>()
            .Description("The set ID");
        descriptor.Field(f => f.SetCode)
            .Name("setCode")
            .Type<StringType>()
            .Description("The set code");
        descriptor.Field(f => f.SetGroupId)
            .Name("setGroupId")
            .Type<StringType>()
            .Description("The set grouping ID for collector-focused categorization (e.g., borderless, showcase, extended art)");
        descriptor.Field(f => f.SetName)
            .Name("setName")
            .Type<StringType>()
            .Description("The set name");
        descriptor.Field(f => f.SetType)
            .Name("setType")
            .Type<StringType>()
            .Description("The set type");
        descriptor.Field(f => f.SetUri)
            .Name("setUri")
            .Type<StringType>()
            .Description("The Scryfall API URI for the set");
        descriptor.Field(f => f.SetSearchUri)
            .Name("setSearchUri")
            .Type<StringType>()
            .Description("The Scryfall search URI for the set");
        descriptor.Field(f => f.ScryfallSetUri)
            .Name("scryfallSetUri")
            .Type<StringType>()
            .Description("The Scryfall web page for the set");
        descriptor.Field(f => f.RulingsUri)
            .Name("rulingsUri")
            .Type<StringType>()
            .Description("The URI for rulings");
        descriptor.Field(f => f.PrintsSearchUri)
            .Name("printsSearchUri")
            .Type<StringType>()
            .Description("The URI to search for prints");
        descriptor.Field(f => f.CollectorNumber)
            .Name("collectorNumber")
            .Type<StringType>()
            .Description("The collector number");
        descriptor.Field(f => f.Digital)
            .Name("digital")
            .Type<BooleanType>()
            .Description("Whether this is a digital card");
        descriptor.Field(f => f.Rarity)
            .Name("rarity")
            .Type<StringType>()
            .Description("The rarity");
        descriptor.Field(f => f.FlavorText)
            .Name("flavorText")
            .Type<StringType>()
            .Description("The flavor text");
        descriptor.Field(f => f.CardBackId)
            .Name("cardBackId")
            .Type<StringType>()
            .Description("The card back ID");
        descriptor.Field(f => f.Artist)
            .Name("artist")
            .Type<StringType>()
            .Description("The artist name");
        descriptor.Field(f => f.ArtistIds)
            .Name("artistIds")
            .Type<ListType<StringType>>()
            .Description("The artist IDs");
        descriptor.Field(f => f.IllustrationId)
            .Name("illustrationId")
            .Type<StringType>()
            .Description("The illustration ID");
        descriptor.Field(f => f.BorderColor)
            .Name("borderColor")
            .Type<StringType>()
            .Description("The border color");
        descriptor.Field(f => f.Frame)
            .Name("frame")
            .Type<StringType>()
            .Description("The frame version");
        descriptor.Field(f => f.FrameEffects)
            .Name("frameEffects")
            .Type<ListType<StringType>>()
            .Description("Frame effects");
        descriptor.Field(f => f.SecurityStamp)
            .Name("securityStamp")
            .Type<StringType>()
            .Description("The security stamp");
        descriptor.Field(f => f.FullArt)
            .Name("fullArt")
            .Type<BooleanType>()
            .Description("Whether this card is full art");
        descriptor.Field(f => f.Textless)
            .Name("textless")
            .Type<BooleanType>()
            .Description("Whether this card is textless");
        descriptor.Field(f => f.Booster)
            .Name("booster")
            .Type<BooleanType>()
            .Description("Whether this card appears in boosters");
        descriptor.Field(f => f.StorySpotlight)
            .Name("storySpotlight")
            .Type<BooleanType>()
            .Description("Whether this card has a story spotlight");
        descriptor.Field(f => f.PromoTypes)
            .Name("promoTypes")
            .Type<ListType<StringType>>()
            .Description("The types of promotional printing");
        descriptor.Field(f => f.EdhRecRank)
            .Name("edhRecRank")
            .Type<IntType>()
            .Description("The EDHREC rank");
        descriptor.Field(f => f.PennyRank)
            .Name("pennyRank")
            .Type<IntType>()
            .Description("The Penny Dreadful rank");
        descriptor.Field(f => f.Prices)
            .Name("prices")
            .Type<ScryfallPricesOutEntityType>()
             .Description("Price information");
        descriptor.Field(f => f.RelatedUris)
            .Name("relatedUris")
            .Type<ScryfallRelatedUrisOutEntityType>()
             .Description("Related URIs");
        descriptor.Field(f => f.PurchaseUris)
            .Name("purchaseUris")
            .Type<ScryfallPurchaseUrisOutEntityType>()
             .Description("Purchase URIs");
        descriptor.Field(f => f.CardFaces)
            .Name("cardFaces")
            .Type<ListType<ScryfallCardFaceOutEntityType>>()
             .Description("Card faces for multi-faced cards");
        descriptor.Field(f => f.AllParts)
            .Name("allParts")
            .Type<ListType<ScryfallAllPartsOutEntityType>>()
             .Description("Related card parts");
        descriptor.Field(f => f.PrintedName)
            .Name("printedName")
            .Type<StringType>()
            .Description("The printed name");
        descriptor.Field(f => f.PrintedTypeLine)
            .Name("printedTypeLine")
            .Type<StringType>()
            .Description("The printed type line");
        descriptor.Field(f => f.PrintedText)
            .Name("printedText")
            .Type<StringType>()
            .Description("The printed text");
        descriptor.Field(f => f.Watermark)
            .Name("watermark")
            .Type<StringType>()
            .Description("The watermark");
        descriptor.Field(f => f.ContentWarning)
            .Name("contentWarning")
            .Type<BooleanType>()
            .Description("Content warning flag");
        descriptor.Field(f => f.Preview)
            .Name("preview")
            .Type<ScryfallPreviewOutEntityType>()
             .Description("Preview information");
        descriptor.Field(f => f.ProducedMana)
            .Name("producedMana")
            .Type<ListType<StringType>>()
            .Description("Mana produced by this card");
        descriptor.Field(f => f.AttractionLights)
            .Name("attractions")
             .Description("Attraction lights");
        descriptor.Field(f => f.UserCollection)
            .Name("userCollection")
            .Type<ListType<CollectedItemOutEntityType>>()
            .Description("User's collection information for this card (populated when userId provided)");
    }
}
