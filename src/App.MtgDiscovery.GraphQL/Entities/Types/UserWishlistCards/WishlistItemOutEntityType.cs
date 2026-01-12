using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserWishlistCards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.UserWishlistCards;

public sealed class WishlistItemOutEntityType : ObjectType<WishlistItemOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<WishlistItemOutEntity> descriptor)
    {
        descriptor.Name("WishlistItem")
            .Description("A wishlist item variant with finish and special treatment");

        descriptor.Field(f => f.Finish)
            .Name("finish")
            .Type<NonNullType<StringType>>()
            .Description("The finish type (nonfoil, foil, etched)");
        descriptor.Field(f => f.Special)
            .Name("special")
            .Type<NonNullType<StringType>>()
            .Description("The special treatment (none, showcase, extended, retro, promo, altered, serialized)");
        descriptor.Field(f => f.Count)
            .Name("count")
            .Type<NonNullType<IntType>>()
            .Description("The number of this variant wanted");
    }
}
