using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.UserCards;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.UserCards;

public sealed class AddCardToCollectionArgEntityInputType : InputObjectType<AddCardToCollectionArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<AddCardToCollectionArgEntity> descriptor)
    {
        descriptor.Name("AddCardToCollectionInput");
        descriptor.Description("Input for adding cards to a user's collection");

        descriptor.Field(x => x.CardId)
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the card");

        descriptor.Field(x => x.SetId)
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the set");

        descriptor.Field(x => x.CollectedItem)
            .Type<NonNullType<CollectedItemArgEntityInputType>>()
            .Description("The collected item with its finish and count");
    }
}
