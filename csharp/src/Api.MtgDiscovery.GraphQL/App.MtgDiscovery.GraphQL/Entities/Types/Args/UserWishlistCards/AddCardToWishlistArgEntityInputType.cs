using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.UserWishlistCards;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.UserWishlistCards;

public sealed class AddCardToWishlistArgEntityInputType : InputObjectType<AddUserWishlistCardArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<AddUserWishlistCardArgEntity> descriptor)
    {
        _ = descriptor.Name("AddCardToWishlistInput")
            .Description("Input for adding cards to a user's wishlist");

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
        _ = descriptor.Field(x => x.UserWishlistCardDetails)
            .Name("userWishlistCardDetails")
            .Type<NonNullType<WishlistItemArgEntityInputType>>()
            .Description("The wishlist item with its finish and count");
    }
}
