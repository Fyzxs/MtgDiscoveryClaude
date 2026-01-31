using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

internal sealed class SigningResultResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("SigningResultResponse")
            .Description("Response for user cards for signing queries")
            .Type<SigningResultSuccessDataResponseModelType>()
            .Type<FailureResponseModelType>();
    }
}
