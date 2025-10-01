using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Types.Sets;
using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.Sets;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

internal class SetsSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<List<ScryfallSetOutEntity>>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<List<ScryfallSetOutEntity>>> descriptor)
    {
        descriptor.Name("SuccessSetsResponse");
        descriptor.Description("Response returned when sets are successfully retrieved");

        descriptor.Field(f => f.Data)
            .Type<ListType<ScryfallSetOutEntityType>>()
            .Description("The list of sets retrieved");

        descriptor.Field(f => f.Status)
            .Type<StatusDataModelType>()
            .Description("Status information about the success");

        descriptor.Field(f => f.MetaData)
            .Type<MetaDataModelType>()
            .Description("Metadata about the response");
    }
}
