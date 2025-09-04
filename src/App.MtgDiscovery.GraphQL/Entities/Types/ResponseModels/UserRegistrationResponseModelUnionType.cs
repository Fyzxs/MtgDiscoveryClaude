using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class UserRegistrationResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("UserRegistrationResponseModel");
        descriptor.Description("Union type for user registration response");
        descriptor.Type<UserRegistrationSuccessDataResponseModelType>();
        descriptor.Type<FailureResponseModelType>();
    }
}