using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.Cards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Cards;

internal sealed class ScryfallPreviewOutEntityType : ObjectType<PreviewOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<PreviewOutEntity> descriptor)
    {
        descriptor.Name("Preview")
            .Description("Preview information");

        descriptor.Field(f => f.Source)
            .Name("source")
            .Type<StringType>()
            .Description("The preview source");
        descriptor.Field(f => f.SourceUri)
            .Name("sourceUri")
            .Type<StringType>()
            .Description("The preview source URI");
        descriptor.Field(f => f.PreviewedAt)
            .Name("previewedAt")
            .Type<StringType>()
            .Description("The preview date");
    }
}
