using Lib.Shared.DataModels.Entities.Outs.Cards;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Cards;

internal class ScryfallAllPartsOutEntityType : ObjectType<AllPartsOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<AllPartsOutEntity> descriptor)
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
