using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class AddUserSealedProductResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("AddUserSealedProductResponse")
            .Description("Union type for different response types from AddUserSealedProduct mutation")
            .Type<AddUserSealedProductSuccessDataResponseModelType>()
            .Type<FailureResponseModelType>();
    }
}
