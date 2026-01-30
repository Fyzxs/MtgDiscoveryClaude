using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Collections;

public sealed class AuthorizedUsersResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("AuthorizedUsersResponse")
            .Description("Union type for collection access list query response")
            .Type<AuthorizedUsersSuccessDataResponseModelType>()
            .Type<FailureResponseModelType>();
    }
}
