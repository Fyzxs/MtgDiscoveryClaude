using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Types.UserSetCards;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class AllUserSetCardsSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<List<UserSetCardOutEntity>>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<List<UserSetCardOutEntity>>> descriptor)
    {
        _ = descriptor.Name("AllUserSetCardsSuccessResponse")
            .Description("Response returned when querying all user set cards is successful");

        _ = descriptor.Field(f => f.Data)
            .Name("data")
            .Type<NonNullType<ListType<NonNullType<UserSetCardOutEntityType>>>>()
            .Description("The list of user set card collection results");
        _ = descriptor.Field(f => f.Status)
            .Name("status")
            .Type<StatusDataModelType>()
            .Description("Status information about the success");
        _ = descriptor.Field(f => f.MetaData)
            .Name("metaData")
            .Type<MetaDataModelType>()
            .Description("Metadata about the response");
    }
}
