using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.Cards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Cards;

internal class ScryfallPreviewOutEntityType : ObjectType<PreviewOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<PreviewOutEntity> descriptor)
    {
        descriptor.Name("Preview");
        descriptor.Description("Preview information");

        descriptor.Field(f => f.Source).Description("The preview source");
        descriptor.Field(f => f.SourceUri).Description("The preview source URI");
        descriptor.Field(f => f.PreviewedAt).Description("The preview date");
    }
}
