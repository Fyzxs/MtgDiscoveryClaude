using App.MtgDiscovery.GraphQL.Entities.Outs.Cards;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Cards;

internal class ScryfallImageUrisOutEntityType : ObjectType<ImageUrisOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<ImageUrisOutEntity> descriptor)
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
