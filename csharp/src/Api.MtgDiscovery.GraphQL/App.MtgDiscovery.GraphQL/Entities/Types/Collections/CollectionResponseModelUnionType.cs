using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Collections;

internal sealed class CollectionResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("CollectionResponse")
            .Description("Union type for collection operation response")
            .Type<CollectionsSuccessDataResponseModelType>()
            .Type<FailureResponseModelType>();
    }
}
