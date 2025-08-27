using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public class FailureResponseModelType : ObjectType<FailureResponseModel>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<FailureResponseModel> descriptor)
    {
        descriptor.Name("FailureResponse");
        descriptor.Description("Response returned when the query fails");

        descriptor.Field(f => f.Status)
            .Type<StatusDataModelType>()
            .Description("Status information about the failure");

        descriptor.Field(f => f.MetaData)
            .Type<MetaDataModelType>()
            .Description("Metadata about the response");
    }
}
