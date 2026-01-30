using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.Collections;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Collections;

public sealed class CollectionsSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<List<CollectionOutEntity>>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<List<CollectionOutEntity>>> descriptor)
    {
        descriptor.Name("CollectionsSuccessResponse")
            .Description("Response returned when a collections query is successful");

        descriptor.Field(f => f.Data)
            .Name("data")
            .Type<NonNullType<ListType<NonNullType<CollectionOutEntityType>>>>()
            .Description("The list of collections");
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
