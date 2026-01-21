using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSealedProducts;

namespace App.MtgDiscovery.GraphQL.Entities.Types.UserSealedProducts;

internal sealed class AddUserSealedProductResultOutEntityType : ObjectType<AddUserSealedProductResultOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<AddUserSealedProductResultOutEntity> descriptor)
    {
        descriptor.Name("AddUserSealedProductResult")
            .Description("Result of adding a sealed product to user collection");

        descriptor.Field(f => f.ProductUuid)
            .Name("productUuid")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the sealed product");
        descriptor.Field(f => f.Count)
            .Name("count")
            .Type<NonNullType<IntType>>()
            .Description("The updated count of this sealed product in the collection");
    }
}
