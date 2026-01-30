using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Types.User;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.User;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class UserRegistrationSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<UserSyncOutEntity>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<UserSyncOutEntity>> descriptor)
    {
        descriptor.Name("UserRegistrationSuccessResponse")
            .Description("Response returned when user registration is successful");

        descriptor.Field(f => f.Data)
            .Name("data")
            .Type<NonNullType<UserSyncOutEntityType>>()
            .Description("The user sync result with login status");
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
