using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.UserCards;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.UserCards;

public sealed class AddCardToCollectionArgEntityInputType : InputObjectType<AddUserCardArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<AddUserCardArgEntity> descriptor)
    {
        _ = descriptor.Name("AddCardToCollectionInput")
            .Description("Input for adding cards to a user's collection");

        _ = descriptor.Field(x => x.CardId)
            .Name("cardId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the card");
        _ = descriptor.Field(x => x.SetId)
            .Name("setId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the set");
        _ = descriptor.Field(x => x.UserId)
            .Name("userId")
            .Type<NonNullType<StringType>>()
            .Description("The user Id of the user adding the card");
        _ = descriptor.Field(x => x.UserCardDetails)
            .Name("userCardDetails")
            .Type<NonNullType<CollectedItemArgEntityInputType>>()
            .Description("The collected item with its finish and count");
    }
}
