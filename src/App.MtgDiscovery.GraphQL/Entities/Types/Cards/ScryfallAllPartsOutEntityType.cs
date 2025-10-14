using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Cards;

internal sealed class ScryfallAllPartsOutEntityType : ObjectType<AllPartsOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<AllPartsOutEntity> descriptor)
    {
        descriptor.Name("AllParts")
            .Description("Related card parts");

        descriptor.Field(f => f.Id)
            .Name("id")
            .Type<StringType>()
            .Description("The card ID");
        descriptor.Field(f => f.Component)
            .Name("component")
            .Type<StringType>()
            .Description("The component type");
        descriptor.Field(f => f.Name)
            .Name("name")
            .Type<StringType>()
            .Description("The card name");
        descriptor.Field(f => f.TypeLine)
            .Name("typeLine")
            .Type<StringType>()
            .Description("The type line");
        descriptor.Field(f => f.Uri)
            .Name("uri")
            .Type<StringType>()
            .Description("The Scryfall API URI");
    }
}
