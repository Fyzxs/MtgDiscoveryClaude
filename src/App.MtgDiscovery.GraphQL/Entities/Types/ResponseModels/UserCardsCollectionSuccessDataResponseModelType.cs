using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Outs.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.UserCards;
using HotChocolate.Types;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class UserCardsCollectionSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<List<UserCardOutEntity>>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<List<UserCardOutEntity>>> descriptor)
    {
        descriptor.Name("SuccessUserCardsCollectionResponse");
        descriptor.Description("Response returned when querying user cards collection is successful");

        descriptor.Field(f => f.Data)
            .Type<NonNullType<ListType<NonNullType<UserCardCollectionOutEntityType>>>>()
            .Description("The user card collection results");

        descriptor.Field(f => f.Status)
            .Type<StatusDataModelType>()
            .Description("Status information about the success");

        descriptor.Field(f => f.MetaData)
            .Type<MetaDataModelType>()
            .Description("Metadata about the response");
    }
}
