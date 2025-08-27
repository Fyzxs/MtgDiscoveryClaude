using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public class MetaDataModelType : ObjectType<MetaDataModel>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<MetaDataModel> descriptor)
    {
        descriptor.Name("MetaData");
        descriptor.Description("Metadata for the response");

        descriptor.Field(f => f.TimeStamp)
            .Description("Timestamp of the response");

        descriptor.Field(f => f.InvocationId)
            .Description("Unique ID for this invocation");

        descriptor.Field(f => f.ElapsedTime)
            .Description("Time taken to process the request");
    }
}