using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Outs.User;
using App.MtgDiscovery.GraphQL.Entities.Types.User;
using HotChocolate.Types;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class UserRegistrationSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<UserRegistrationOutEntity>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<UserRegistrationOutEntity>> descriptor)
    {
        descriptor.Name("SuccessUserRegistrationResponse");
        descriptor.Description("Response returned when user registration is successful");

        descriptor.Field(f => f.Data)
            .Type<NonNullType<UserRegistrationOutEntityType>>()
            .Description("The user registration result");

        descriptor.Field(f => f.Status)
            .Type<StatusDataModelType>()
            .Description("Status information about the success");

        descriptor.Field(f => f.MetaData)
            .Type<MetaDataModelType>()
            .Description("Metadata about the response");
    }
}