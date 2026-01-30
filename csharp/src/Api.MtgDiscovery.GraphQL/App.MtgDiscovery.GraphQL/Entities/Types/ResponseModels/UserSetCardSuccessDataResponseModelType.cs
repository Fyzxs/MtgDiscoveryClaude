using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Types.UserSetCards;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class UserSetCardSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<UserSetCardOutEntity>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<UserSetCardOutEntity>> descriptor)
    {
        descriptor.Name("UserSetCardSuccessResponse")
            .Description("Response returned when querying user set card is successful");

        descriptor.Field(f => f.Data)
            .Name("data")
            .Type<NonNullType<UserSetCardOutEntityType>>()
            .Description("The user set card collection result");
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
