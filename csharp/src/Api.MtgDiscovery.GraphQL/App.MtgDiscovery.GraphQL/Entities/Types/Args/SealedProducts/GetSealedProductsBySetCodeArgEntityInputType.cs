using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.SealedProducts;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.SealedProducts;

internal sealed class GetSealedProductsBySetCodeArgEntityInputType : InputObjectType<GetSealedProductsBySetCodeArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<GetSealedProductsBySetCodeArgEntity> descriptor)
    {
        _ = descriptor.Name("GetSealedProductsBySetCodeInput")
            .Description("Input for querying sealed products by set code");

        _ = descriptor.Field(x => x.SetCode)
            .Name("setCode")
            .Type<NonNullType<StringType>>()
            .Description("The set code to query sealed products for");
        _ = descriptor.Field(x => x.UserId)
            .Name("userId")
            .Type<StringType>()
            .Description("Optional user identifier to enrich products with collection data");
        _ = descriptor.Field(x => x.CollectionId)
            .Name("collectionId")
            .Type<StringType>()
            .Description("Optional collection identifier to enrich products with collection data");
    }
}
