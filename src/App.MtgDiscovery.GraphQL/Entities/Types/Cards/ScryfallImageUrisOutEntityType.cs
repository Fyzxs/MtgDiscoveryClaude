using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Cards;

internal sealed class ScryfallImageUrisOutEntityType : ObjectType<ImageUrisOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<ImageUrisOutEntity> descriptor)
    {
        descriptor.Name("ImageUris")
            .Description("URIs to various image formats");

        descriptor.Field(f => f.Small)
            .Name("small")
            .Type<StringType>()
            .Description("Small JPG image");
        descriptor.Field(f => f.Normal)
            .Name("normal")
            .Type<StringType>()
            .Description("Normal JPG image");
        descriptor.Field(f => f.Large)
            .Name("large")
            .Type<StringType>()
            .Description("Large JPG image");
        descriptor.Field(f => f.Png)
            .Name("png")
            .Type<StringType>()
            .Description("PNG image");
        descriptor.Field(f => f.ArtCrop)
            .Name("artCrop")
            .Type<StringType>()
            .Description("Art crop image");
        descriptor.Field(f => f.BorderCrop)
            .Name("borderCrop")
            .Type<StringType>()
            .Description("Border crop image");
    }
}
