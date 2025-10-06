using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class UserCardCollectionResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("UserCardCollectionResponse")
            .Description("Union type for user card collection response");
        descriptor.Type<UserCardCollectionSuccessDataResponseModelType>();
        descriptor.Type<FailureResponseModelType>();
    }
}
