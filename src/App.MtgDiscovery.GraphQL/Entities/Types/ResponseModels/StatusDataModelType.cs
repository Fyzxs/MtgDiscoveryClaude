using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public class StatusDataModelType : ObjectType<StatusDataModel>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<StatusDataModel> descriptor)
    {
        descriptor.Name("StatusData")
            .Description("Status information for the response");

        descriptor.Field(f => f.StatusCode)
            .Description("HTTP status code");

        descriptor.Field(f => f.Message)
            .Description("Status message");
    }
}
