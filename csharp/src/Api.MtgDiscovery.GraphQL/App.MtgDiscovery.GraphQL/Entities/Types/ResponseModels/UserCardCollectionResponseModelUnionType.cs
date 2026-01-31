using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

internal sealed class UserCardCollectionResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        _ = descriptor.Name("UserCardCollectionResponse")
            .Description("Union type for user card collection response")
            .Type<UserCardCollectionSuccessDataResponseModelType>()
            .Type<FailureResponseModelType>();
    }
}
