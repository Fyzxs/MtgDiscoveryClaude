using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Types.User;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.User;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class UserRegistrationSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<UserRegistrationOutEntity>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<UserRegistrationOutEntity>> descriptor)
    {
        descriptor.Name("UserRegistrationSuccessResponse")
            .Description("Response returned when user registration is successful");

        descriptor.Field(f => f.Data)
            .Name("data")
            .Type<NonNullType<UserRegistrationOutEntityType>>()
            .Description("The user registration result");
        descriptor.Field(f => f.Status)
            .Name("status")
            .Type<StatusDataModelType>()
            .Description("Status information about the success");
        descriptor.Field(f => f.MetaData)
            .Name("metaData")
            .Type<MetaDataModelType>()
            .Description("Metadata about the response");
    }
}
