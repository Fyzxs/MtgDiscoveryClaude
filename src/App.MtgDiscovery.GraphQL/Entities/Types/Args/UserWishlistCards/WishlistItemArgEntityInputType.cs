using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.UserWishlistCards;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.UserWishlistCards;

public sealed class WishlistItemArgEntityInputType : InputObjectType<UserWishlistCardDetailsArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<UserWishlistCardDetailsArgEntity> descriptor)
    {
        _ = descriptor.Name("WishlistItemInput")
            .Description("Represents a wishlist item with its finish and count");

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
    }
}
