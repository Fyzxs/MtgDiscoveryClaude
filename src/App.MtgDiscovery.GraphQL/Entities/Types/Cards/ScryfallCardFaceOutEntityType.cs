using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.Cards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Cards;

internal class ScryfallCardFaceOutEntityType : ObjectType<CardFaceOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<CardFaceOutEntity> descriptor)
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
        descriptor.Field(f => f.ImageUris).Type<ScryfallImageUrisOutEntityType>().Description("Image URIs");
        descriptor.Field(f => f.FlavorText).Description("The flavor text");
        descriptor.Field(f => f.PrintedName).Description("The printed name");
        descriptor.Field(f => f.PrintedTypeLine).Description("The printed type line");
        descriptor.Field(f => f.PrintedText).Description("The printed text");
        descriptor.Field(f => f.Watermark).Description("The watermark");
        descriptor.Field(f => f.Layout).Description("The layout");
        descriptor.Field(f => f.Cmc).Description("The converted mana cost");
    }
}
