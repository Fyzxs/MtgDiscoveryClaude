using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class AddCardToWishlistResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("AddCardToWishlistResponse")
            .Description("Union type for different response types from AddCardToWishlist mutation")
            .Type<CardsSuccessDataResponseModelType>()
            .Type<FailureResponseModelType>();
    }
}
