using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.UserCards;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.UserCards;

public sealed class CollectedItemArgEntityInputType : InputObjectType<UserCardDetailsArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<UserCardDetailsArgEntity> descriptor)
    {
        _ = descriptor.Name("CollectedItemInput")
            .Description("Represents a collected item with its finish and count");

        _ = descriptor.Field(x => x.Finish)
            .Name("finish")
            .Type<NonNullType<StringType>>()
            .Description("The finish type of the card (e.g., 'foil', 'nonfoil')");
        _ = descriptor.Field(x => x.Special)
            .Name("special")
            .Type<StringType>()
            .Description("Special treatment or version of the card");
        _ = descriptor.Field(x => x.Count)
            .Name("count")
            .Type<NonNullType<IntType>>()
            .Description("The number of copies of this variant");
        _ = descriptor.Field(x => x.SetGroupId)
            .Name("setGroupId")
            .Type<StringType>()
            .Description("The set grouping ID (e.g., 'borderless', 'showcase'). Null if card has no grouping.");
    }
}
