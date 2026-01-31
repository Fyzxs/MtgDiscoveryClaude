using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

internal class SealedProductsResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("SealedProductsResponse")
            .Description("Union type for different response types from SealedProductsBySetCode query")
            .Type<SealedProductsSuccessDataResponseModelType>()
            .Type<FailureResponseModelType>();
    }
}
