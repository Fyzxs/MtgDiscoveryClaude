using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

internal sealed class SetResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("SetsByIdResponse");
        descriptor.Description("Union type for different response types from SetsById query");

        // Register the concrete types that can be returned
        descriptor.Type<FailureResponseModelType>();
        descriptor.Type<SetsSuccessDataResponseModelType>();
    }
}
