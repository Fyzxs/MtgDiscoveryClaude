using App.MtgDiscovery.GraphQL.Entities.Outs.Cards;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Cards;

internal class ScryfallCardOutEntityType : ObjectType<ScryfallCardOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<ScryfallCardOutEntity> descriptor)
    {
        descriptor.Name("Card");
        descriptor.Description("Represents a Magic: The Gathering card from Scryfall");

        descriptor.Field(f => f.Id)
            .Type<NonNullType<StringType>>()
            .Description("The unique Scryfall identifier of the card");

        descriptor.Field(f => f.OracleId)
            .Description("The Oracle ID for this card");

        descriptor.Field(f => f.ArenaId)
            .Description("The MTG Arena ID");

        descriptor.Field(f => f.MtgoId)
            .Description("The Magic Online ID");

        descriptor.Field(f => f.MtgoFoilId)
            .Description("The Magic Online foil ID");

        descriptor.Field(f => f.MultiverseIds)
            .Description("The multiverse IDs on Gatherer");

        descriptor.Field(f => f.TcgPlayerId)
            .Description("The TCGPlayer ID");

        descriptor.Field(f => f.TcgPlayerEtchedId)
            .Description("The TCGPlayer etched foil ID");

        descriptor.Field(f => f.CardMarketId)
            .Description("The Cardmarket ID");

        descriptor.Field(f => f.Name)
            .Description("The name of the card");

        descriptor.Field(f => f.FlavorName)
            .Description("The flavor name of the card");

        descriptor.Field(f => f.Lang)
            .Description("The language code for this printing");

        descriptor.Field(f => f.ReleasedAt)
            .Description("The date this card was released");

        descriptor.Field(f => f.Uri)
            .Description("The Scryfall API URI for this card");

        descriptor.Field(f => f.ScryfallUri)
            .Description("The Scryfall web page for this card");

        descriptor.Field(f => f.Layout)
            .Description("The card layout");

        descriptor.Field(f => f.HighResImage)
            .Description("Whether high-resolution images are available");

        descriptor.Field(f => f.ImageStatus)
            .Description("The status of image availability");

        descriptor.Field(f => f.ImageUris)
            .Type<ScryfallImageUrisOutEntityType>()
            .Description("URIs to images of the card");

        descriptor.Field(f => f.ManaCost)
            .Description("The mana cost");

        descriptor.Field(f => f.Cmc)
            .Description("The converted mana cost");

        descriptor.Field(f => f.TypeLine)
            .Description("The type line");

        descriptor.Field(f => f.OracleText)
            .Description("The Oracle text");

        descriptor.Field(f => f.Power)
            .Description("The power (creatures only)");

        descriptor.Field(f => f.Toughness)
            .Description("The toughness (creatures only)");

        descriptor.Field(f => f.Loyalty)
            .Description("The loyalty (planeswalkers only)");

        descriptor.Field(f => f.Defense)
            .Description("The defense (battles only)");

        descriptor.Field(f => f.LifeModifier)
            .Description("The life modifier (Vanguard only)");

        descriptor.Field(f => f.HandModifier)
            .Description("The hand modifier (Vanguard only)");

        descriptor.Field(f => f.Colors)
            .Description("The card colors");

        descriptor.Field(f => f.ColorIdentity)
            .Description("The color identity");

        descriptor.Field(f => f.ColorIndicator)
            .Description("The color indicator");

        descriptor.Field(f => f.Keywords)
            .Description("Keywords on this card");

        descriptor.Field(f => f.Legalities)
            .Type<ScryfallLegalitiesOutEntityType>()
            .Description("Format legalities");

        descriptor.Field(f => f.Games)
            .Description("Games this card appears in");

        descriptor.Field(f => f.Reserved)
            .Description("Whether this card is on the reserved list");

        descriptor.Field(f => f.GameChanger)
            .Description("Whether this card is a game changer");

        descriptor.Field(f => f.Foil)
            .Description("Whether this card is available in foil");

        descriptor.Field(f => f.NonFoil)
            .Description("Whether this card is available in non-foil");

        descriptor.Field(f => f.Finishes)
            .Description("Available finishes");

        descriptor.Field(f => f.Oversized)
            .Description("Whether this card is oversized");

        descriptor.Field(f => f.Promo)
            .Description("Whether this card is a promotional printing");

        descriptor.Field(f => f.Reprint)
            .Description("Whether this card is a reprint");

        descriptor.Field(f => f.Variation)
            .Description("Whether this card is a variation");

        descriptor.Field(f => f.SetId)
            .Description("The set ID");

        descriptor.Field(f => f.SetCode)
            .Description("The set code");

        descriptor.Field(f => f.SetName)
            .Description("The set name");

        descriptor.Field(f => f.SetType)
            .Description("The set type");

        descriptor.Field(f => f.SetUri)
            .Description("The Scryfall API URI for the set");

        descriptor.Field(f => f.SetSearchUri)
            .Description("The Scryfall search URI for the set");

        descriptor.Field(f => f.ScryfallSetUri)
            .Description("The Scryfall web page for the set");

        descriptor.Field(f => f.RulingsUri)
            .Description("The URI for rulings");

        descriptor.Field(f => f.PrintsSearchUri)
            .Description("The URI to search for prints");

        descriptor.Field(f => f.CollectorNumber)
            .Description("The collector number");

        descriptor.Field(f => f.Digital)
            .Description("Whether this is a digital card");

        descriptor.Field(f => f.Rarity)
            .Description("The rarity");

        descriptor.Field(f => f.FlavorText)
            .Description("The flavor text");

        descriptor.Field(f => f.CardBackId)
            .Description("The card back ID");

        descriptor.Field(f => f.Artist)
            .Description("The artist name");

        descriptor.Field(f => f.ArtistIds)
            .Description("The artist IDs");

        descriptor.Field(f => f.IllustrationId)
            .Description("The illustration ID");

        descriptor.Field(f => f.BorderColor)
            .Description("The border color");

        descriptor.Field(f => f.Frame)
            .Description("The frame version");

        descriptor.Field(f => f.FrameEffects)
            .Description("Frame effects");

        descriptor.Field(f => f.SecurityStamp)
            .Description("The security stamp");

        descriptor.Field(f => f.FullArt)
            .Description("Whether this card is full art");

        descriptor.Field(f => f.Textless)
            .Description("Whether this card is textless");

        descriptor.Field(f => f.Booster)
            .Description("Whether this card appears in boosters");

        descriptor.Field(f => f.StorySpotlight)
            .Description("Whether this card has a story spotlight");

        descriptor.Field(f => f.PromoTypes)
            .Description("The types of promotional printing");

        descriptor.Field(f => f.EdhRecRank)
            .Description("The EDHREC rank");

        descriptor.Field(f => f.PennyRank)
            .Description("The Penny Dreadful rank");

        descriptor.Field(f => f.Prices)
            .Type<ScryfallPricesOutEntityType>()
            .Description("Price information");

        descriptor.Field(f => f.RelatedUris)
            .Type<ScryfallRelatedUrisOutEntityType>()
            .Description("Related URIs");

        descriptor.Field(f => f.PurchaseUris)
            .Type<ScryfallPurchaseUrisOutEntityType>()
            .Description("Purchase URIs");

        descriptor.Field(f => f.CardFaces)
            .Type<ListType<ScryfallCardFaceOutEntityType>>()
            .Description("Card faces for multi-faced cards");

        descriptor.Field(f => f.AllParts)
            .Type<ListType<ScryfallAllPartsOutEntityType>>()
            .Description("Related card parts");

        descriptor.Field(f => f.PrintedName)
            .Description("The printed name");

        descriptor.Field(f => f.PrintedTypeLine)
            .Description("The printed type line");

        descriptor.Field(f => f.PrintedText)
            .Description("The printed text");

        descriptor.Field(f => f.Watermark)
            .Description("The watermark");

        descriptor.Field(f => f.ContentWarning)
            .Description("Content warning flag");

        descriptor.Field(f => f.Preview)
            .Type<ScryfallPreviewOutEntityType>()
            .Description("Preview information");

        descriptor.Field(f => f.ProducedMana)
            .Description("Mana produced by this card");

        descriptor.Field(f => f.AttractionLights)
            .Name("attractions")
            .Description("Attraction lights");
    }
}
