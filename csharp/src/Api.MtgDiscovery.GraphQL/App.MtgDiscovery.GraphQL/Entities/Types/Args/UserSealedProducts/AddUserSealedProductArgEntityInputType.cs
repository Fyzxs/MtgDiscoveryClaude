using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.UserSealedProducts;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.UserSealedProducts;

internal sealed class AddUserSealedProductArgEntityInputType : InputObjectType<AddUserSealedProductArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<AddUserSealedProductArgEntity> descriptor)
    {
        _ = descriptor.Name("AddUserSealedProductInput")
            .Description("Input for adding sealed products to a user's collection");

        _ = descriptor.Field(x => x.ProductUuid)
            .Name("productUuid")
            .Type<NonNullType<StringType>>()
            .Description("The UUID of the sealed product");

        _ = descriptor.Field(x => x.SetId)
            .Name("setId")
            .Type<NonNullType<StringType>>()
            .Description("The set ID containing the sealed product");

        _ = descriptor.Field(x => x.UserSealedProductDetails)
            .Name("userSealedProductDetails")
            .Type<NonNullType<UserSealedProductDetailsArgEntityInputType>>()
            .Description("The sealed product details including quantity change");
    }
}
