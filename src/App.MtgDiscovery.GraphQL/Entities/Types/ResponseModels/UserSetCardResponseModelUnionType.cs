using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class UserSetCardResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("UserSetCardResponse")
            .Description("Response for user set card queries")
            .Type<UserSetCardSuccessDataResponseModelType>()
            .Type<FailureResponseModelType>();
    }
}
