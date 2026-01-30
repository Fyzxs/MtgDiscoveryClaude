using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Cards;

internal sealed class ScryfallCardFaceOutEntityType : ObjectType<CardFaceOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<CardFaceOutEntity> descriptor)
    {
        descriptor.Name("CardFace")
            .Description("A face of a multi-faced card");

        descriptor.Field(f => f.Name)
            .Name("name")
            .Type<StringType>()
            .Description("The face name");
        descriptor.Field(f => f.ManaCost)
            .Name("manaCost")
            .Type<StringType>()
            .Description("The mana cost");
        descriptor.Field(f => f.TypeLine)
            .Name("typeLine")
            .Type<StringType>()
            .Description("The type line");
        descriptor.Field(f => f.OracleText)
            .Name("oracleText")
            .Type<StringType>()
            .Description("The Oracle text");
        descriptor.Field(f => f.Colors)
            .Name("colors")
            .Type<ListType<StringType>>()
            .Description("The colors");
        descriptor.Field(f => f.ColorIndicator)
            .Name("colorIndicator")
            .Type<ListType<StringType>>()
            .Description("The color indicator");
        descriptor.Field(f => f.Power)
            .Name("power")
            .Type<StringType>()
            .Description("The power");
        descriptor.Field(f => f.Toughness)
            .Name("toughness")
            .Type<StringType>()
            .Description("The toughness");
        descriptor.Field(f => f.Loyalty)
            .Name("loyalty")
            .Type<StringType>()
            .Description("The loyalty");
        descriptor.Field(f => f.Defense)
            .Name("defense")
            .Type<StringType>()
            .Description("The defense");
        descriptor.Field(f => f.Artist)
            .Name("artist")
            .Type<StringType>()
            .Description("The artist");
        descriptor.Field(f => f.ArtistId)
            .Name("artistId")
            .Type<StringType>()
            .Description("The artist ID");
        descriptor.Field(f => f.IllustrationId)
            .Name("illustrationId")
            .Type<StringType>()
            .Description("The illustration ID");
        descriptor.Field(f => f.ImageUris)
            .Name("imageUris")
            .Type<ScryfallImageUrisOutEntityType>()
            .Description("Image URIs");
        descriptor.Field(f => f.FlavorText)
            .Name("flavorText")
            .Type<StringType>()
            .Description("The flavor text");
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
        descriptor.Field(f => f.Layout)
            .Name("layout")
            .Type<StringType>()
            .Description("The layout");
        descriptor.Field(f => f.Cmc)
            .Name("cmc")
            .Type<FloatType>()
            .Description("The converted mana cost");
    }
}
