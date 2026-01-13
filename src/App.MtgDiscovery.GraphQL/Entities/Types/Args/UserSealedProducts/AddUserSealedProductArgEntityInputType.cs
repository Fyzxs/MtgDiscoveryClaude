using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.UserSealedProducts;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.UserSealedProducts;

public sealed class AddUserSealedProductArgEntityInputType : InputObjectType<AddUserSealedProductArgEntity>
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

        _ = descriptor.Field(x => x.CountDelta)
            .Name("countDelta")
            .Type<NonNullType<IntType>>()
            .Description("The quantity change (positive to add, negative to remove)");
    }
}
