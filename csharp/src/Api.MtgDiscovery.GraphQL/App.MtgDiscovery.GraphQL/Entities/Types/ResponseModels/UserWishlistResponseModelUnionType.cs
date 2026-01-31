using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

internal sealed class UserWishlistResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("UserWishlistResponseModel")
            .Description("Union type for different response types from UserWishlist query")
            .Type<UserWishlistSuccessDataResponseModelType>()
            .Type<FailureResponseModelType>();
    }
}
