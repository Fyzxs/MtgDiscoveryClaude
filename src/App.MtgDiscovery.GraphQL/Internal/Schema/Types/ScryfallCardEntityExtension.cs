using App.MtgDiscovery.GraphQL.Apis.Types;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Internal.Schema.Types;

internal class ScryfallCardEntityExtension : ObjectType<ScryfallCardEntity>
{
    protected override void Configure(IObjectTypeDescriptor<ScryfallCardEntity> descriptor)
    {
        descriptor.Name("Card");
        descriptor.Description("Represents a Magic: The Gathering card from Scryfall");

        descriptor.Field(f => f.Id)
            .Type<NonNullType<StringType>>()
            .Description("The unique Scryfall identifier of the card");

        descriptor.Field(f => f.OracleId)
            .Description("The Oracle ID for this card");

        descriptor.Field(f => f.MultiverseIds)
            .Description("The multiverse IDs on Gatherer");

        descriptor.Field(f => f.MtgoId)
            .Description("The Magic Online ID");

        descriptor.Field(f => f.MtgoFoilId)
            .Description("The foil Magic Online ID");

        descriptor.Field(f => f.TcgplayerId)
            .Description("The TCGPlayer ID");

        descriptor.Field(f => f.CardmarketId)
            .Description("The Cardmarket ID");

        descriptor.Field(f => f.Name)
            .Description("The name of the card");

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

        descriptor.Field(f => f.HighresImage)
            .Description("Whether high-resolution images are available");

        descriptor.Field(f => f.ImageStatus)
            .Description("The status of image availability");

        descriptor.Field(f => f.ImageUris)
            .Type<ScryfallImageUrisTypeExtension>()
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
            .Type<ScryfallLegalitiesTypeExtension>()
            .Description("Format legalities");

        descriptor.Field(f => f.Games)
            .Description("Games this card appears in");

        descriptor.Field(f => f.Reserved)
            .Description("Whether this card is on the reserved list");

        descriptor.Field(f => f.Foil)
            .Description("Whether this card is available in foil");

        descriptor.Field(f => f.Nonfoil)
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

        descriptor.Field(f => f.EdhrecRank)
            .Description("The EDHREC rank");

        descriptor.Field(f => f.PennyRank)
            .Description("The Penny Dreadful rank");

        descriptor.Field(f => f.Prices)
            .Type<ScryfallPricesTypeExtension>()
            .Description("Price information");

        descriptor.Field(f => f.RelatedUris)
            .Type<ScryfallRelatedUrisTypeExtension>()
            .Description("Related URIs");

        descriptor.Field(f => f.PurchaseUris)
            .Type<ScryfallPurchaseUrisTypeExtension>()
            .Description("Purchase URIs");

        descriptor.Field(f => f.CardFaces)
            .Type<ListType<ScryfallCardFaceTypeExtension>>()
            .Description("Card faces for multi-faced cards");

        descriptor.Field(f => f.AllParts)
            .Type<ListType<ScryfallAllPartsTypeExtension>>()
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
            .Type<ScryfallPreviewTypeExtension>()
            .Description("Preview information");

        descriptor.Field(f => f.ProducedMana)
            .Description("Mana produced by this card");

        descriptor.Field(f => f.Attractions)
            .Description("Attraction lights");
    }
}

internal class ScryfallImageUrisTypeExtension : ObjectType<ImageUrisType>
{
    protected override void Configure(IObjectTypeDescriptor<ImageUrisType> descriptor)
    {
        descriptor.Name("ImageUris");
        descriptor.Description("URIs to various image formats");

        descriptor.Field(f => f.Small).Description("Small JPG image");
        descriptor.Field(f => f.Normal).Description("Normal JPG image");
        descriptor.Field(f => f.Large).Description("Large JPG image");
        descriptor.Field(f => f.Png).Description("PNG image");
        descriptor.Field(f => f.ArtCrop).Description("Art crop image");
        descriptor.Field(f => f.BorderCrop).Description("Border crop image");
    }
}

internal class ScryfallLegalitiesTypeExtension : ObjectType<LegalitiesType>
{
    protected override void Configure(IObjectTypeDescriptor<LegalitiesType> descriptor)
    {
        descriptor.Name("Legalities");
        descriptor.Description("Format legalities for the card");

        descriptor.Field(f => f.Standard).Description("Standard legality");
        descriptor.Field(f => f.Future).Description("Future Standard legality");
        descriptor.Field(f => f.Historic).Description("Historic legality");
        descriptor.Field(f => f.Timeless).Description("Timeless legality");
        descriptor.Field(f => f.Gladiator).Description("Gladiator legality");
        descriptor.Field(f => f.Pioneer).Description("Pioneer legality");
        descriptor.Field(f => f.Explorer).Description("Explorer legality");
        descriptor.Field(f => f.Modern).Description("Modern legality");
        descriptor.Field(f => f.Legacy).Description("Legacy legality");
        descriptor.Field(f => f.Pauper).Description("Pauper legality");
        descriptor.Field(f => f.Vintage).Description("Vintage legality");
        descriptor.Field(f => f.Penny).Description("Penny Dreadful legality");
        descriptor.Field(f => f.Commander).Description("Commander legality");
        descriptor.Field(f => f.Oathbreaker).Description("Oathbreaker legality");
        descriptor.Field(f => f.StandardBrawl).Description("Standard Brawl legality");
        descriptor.Field(f => f.Brawl).Description("Brawl legality");
        descriptor.Field(f => f.Alchemy).Description("Alchemy legality");
        descriptor.Field(f => f.PauperCommander).Description("Pauper Commander legality");
        descriptor.Field(f => f.Duel).Description("Duel Commander legality");
        descriptor.Field(f => f.Oldschool).Description("Old School legality");
        descriptor.Field(f => f.Premodern).Description("Premodern legality");
        descriptor.Field(f => f.Predh).Description("PreDH legality");
    }
}

internal class ScryfallPricesTypeExtension : ObjectType<PricesType>
{
    protected override void Configure(IObjectTypeDescriptor<PricesType> descriptor)
    {
        descriptor.Name("Prices");
        descriptor.Description("Price information in various currencies");

        descriptor.Field(f => f.Usd).Description("USD price");
        descriptor.Field(f => f.UsdFoil).Description("USD foil price");
        descriptor.Field(f => f.UsdEtched).Description("USD etched price");
        descriptor.Field(f => f.Eur).Description("EUR price");
        descriptor.Field(f => f.EurFoil).Description("EUR foil price");
        descriptor.Field(f => f.Tix).Description("MTGO Tix price");
    }
}

internal class ScryfallRelatedUrisTypeExtension : ObjectType<RelatedUrisType>
{
    protected override void Configure(IObjectTypeDescriptor<RelatedUrisType> descriptor)
    {
        descriptor.Name("RelatedUris");
        descriptor.Description("Related URIs for additional information");

        descriptor.Field(f => f.Gatherer).Description("Gatherer page");
        descriptor.Field(f => f.TcgplayerInfiniteArticles).Description("TCGPlayer articles");
        descriptor.Field(f => f.TcgplayerInfiniteDecks).Description("TCGPlayer decks");
        descriptor.Field(f => f.Edhrec).Description("EDHREC page");
    }
}

internal class ScryfallPurchaseUrisTypeExtension : ObjectType<PurchaseUrisType>
{
    protected override void Configure(IObjectTypeDescriptor<PurchaseUrisType> descriptor)
    {
        descriptor.Name("PurchaseUris");
        descriptor.Description("URIs for purchasing the card");

        descriptor.Field(f => f.Tcgplayer).Description("TCGPlayer purchase link");
        descriptor.Field(f => f.Cardmarket).Description("Cardmarket purchase link");
        descriptor.Field(f => f.Cardhoarder).Description("Cardhoarder purchase link");
    }
}

internal class ScryfallCardFaceTypeExtension : ObjectType<CardFaceType>
{
    protected override void Configure(IObjectTypeDescriptor<CardFaceType> descriptor)
    {
        descriptor.Name("CardFace");
        descriptor.Description("A face of a multi-faced card");

        descriptor.Field(f => f.ObjectString).Description("The object type");
        descriptor.Field(f => f.Name).Description("The face name");
        descriptor.Field(f => f.ManaCost).Description("The mana cost");
        descriptor.Field(f => f.TypeLine).Description("The type line");
        descriptor.Field(f => f.OracleText).Description("The Oracle text");
        descriptor.Field(f => f.Colors).Description("The colors");
        descriptor.Field(f => f.ColorIndicator).Description("The color indicator");
        descriptor.Field(f => f.Power).Description("The power");
        descriptor.Field(f => f.Toughness).Description("The toughness");
        descriptor.Field(f => f.Loyalty).Description("The loyalty");
        descriptor.Field(f => f.Defense).Description("The defense");
        descriptor.Field(f => f.Artist).Description("The artist");
        descriptor.Field(f => f.ArtistId).Description("The artist ID");
        descriptor.Field(f => f.IllustrationId).Description("The illustration ID");
        descriptor.Field(f => f.ImageUris).Type<ScryfallImageUrisTypeExtension>().Description("Image URIs");
        descriptor.Field(f => f.FlavorText).Description("The flavor text");
        descriptor.Field(f => f.PrintedName).Description("The printed name");
        descriptor.Field(f => f.PrintedTypeLine).Description("The printed type line");
        descriptor.Field(f => f.PrintedText).Description("The printed text");
        descriptor.Field(f => f.Watermark).Description("The watermark");
        descriptor.Field(f => f.Layout).Description("The layout");
        descriptor.Field(f => f.Cmc).Description("The converted mana cost");
    }
}

internal class ScryfallAllPartsTypeExtension : ObjectType<AllPartsType>
{
    protected override void Configure(IObjectTypeDescriptor<AllPartsType> descriptor)
    {
        descriptor.Name("AllParts");
        descriptor.Description("Related card parts");

        descriptor.Field(f => f.ObjectString).Description("The object type");
        descriptor.Field(f => f.Id).Description("The card ID");
        descriptor.Field(f => f.Component).Description("The component type");
        descriptor.Field(f => f.Name).Description("The card name");
        descriptor.Field(f => f.TypeLine).Description("The type line");
        descriptor.Field(f => f.Uri).Description("The Scryfall API URI");
    }
}

internal class ScryfallPreviewTypeExtension : ObjectType<PreviewType>
{
    protected override void Configure(IObjectTypeDescriptor<PreviewType> descriptor)
    {
        descriptor.Name("Preview");
        descriptor.Description("Preview information");

        descriptor.Field(f => f.Source).Description("The preview source");
        descriptor.Field(f => f.SourceUri).Description("The preview source URI");
        descriptor.Field(f => f.PreviewedAt).Description("The preview date");
    }
}