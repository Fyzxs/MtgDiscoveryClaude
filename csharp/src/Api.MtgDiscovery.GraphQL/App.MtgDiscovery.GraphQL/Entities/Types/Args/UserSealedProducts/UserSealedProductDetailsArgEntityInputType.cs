using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.UserSealedProducts;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.UserSealedProducts;

internal sealed class UserSealedProductDetailsArgEntityInputType : InputObjectType<UserSealedProductDetailsArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<UserSealedProductDetailsArgEntity> descriptor)
    {
        _ = descriptor.Name("UserSealedProductDetailsInput")
            .Description("Details for sealed product quantity changes");

        _ = descriptor.Field(x => x.Count)
            .Name("count")
            .Type<NonNullType<IntType>>()
            .Description("The quantity change (positive to add, negative to remove)");
    }
}
