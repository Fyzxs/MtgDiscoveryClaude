using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.UserCards;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.UserCards;

public sealed class CollectedItemArgEntityInputType : InputObjectType<UserCardDetailsArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<UserCardDetailsArgEntity> descriptor)
    {
        descriptor.Name("CollectedItemInput");
        descriptor.Description("Represents a collected item with its finish and count");

        descriptor.Field(x => x.Finish)
            .Type<NonNullType<StringType>>()
            .Description("The finish type of the card (e.g., 'foil', 'nonfoil')");

        descriptor.Field(x => x.Special)
            .Type<StringType>()
            .Description("Special treatment or version of the card");

        descriptor.Field(x => x.Count)
            .Type<NonNullType<IntType>>()
            .Description("The number of copies of this variant");
    }
}
