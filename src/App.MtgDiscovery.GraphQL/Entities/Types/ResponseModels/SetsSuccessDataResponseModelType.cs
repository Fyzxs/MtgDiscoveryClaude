using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Types.Sets;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

internal sealed class SetsSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<List<ScryfallSetOutEntity>>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<List<ScryfallSetOutEntity>>> descriptor)
    {
        descriptor.Name("SetsSuccessResponse")
            .Description("Response returned when sets are successfully retrieved");

        descriptor.Field(f => f.Data)
            .Name("data")
            .Type<ListType<ScryfallSetOutEntityType>>()
            .Description("The list of sets retrieved");
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
