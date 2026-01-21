using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Types.UserCards;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class UserCardsCollectionSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<List<UserCardOutEntity>>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<List<UserCardOutEntity>>> descriptor)
    {
        descriptor.Name("UserCardsCollectionSuccessResponse")
            .Description("Response returned when querying user cards collection is successful");

        descriptor.Field(f => f.Data)
            .Name("data")
            .Type<NonNullType<ListType<NonNullType<UserCardCollectionOutEntityType>>>>()
            .Description("The user card collection results");
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
