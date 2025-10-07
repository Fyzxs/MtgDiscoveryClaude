using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

internal sealed class SetResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("SetResponse")
            .Description("Union type for different response types from SetsById query")
            .Type<FailureResponseModelType>()
            .Type<SetsSuccessDataResponseModelType>();
    }
}
