using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Collections;

public sealed class CollectionsResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("CollectionsResponse")
            .Description("Union type for collections query response")
            .Type<CollectionsSuccessDataResponseModelType>()
            .Type<FailureResponseModelType>();
    }
}
