using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Outs;
using HotChocolate.Types;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types;

public class ResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("CardsByIdResponse");
        descriptor.Description("Union type for different response types from CardsById query");

        // Register the concrete types that can be returned
        descriptor.Type<FailureResponseModelType>();
        descriptor.Type<SuccessDataResponseModelType>();
    }
}

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

public class SuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<List<ScryfallCardOutEntity>>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<List<ScryfallCardOutEntity>>> descriptor)
    {
        descriptor.Name("SuccessCardsResponse");
        descriptor.Description("Response returned when cards are successfully retrieved");

        descriptor.Field(f => f.Data)
            .Type<ListType<ScryfallCardOutEntityType>>()
            .Description("The list of cards retrieved");

        descriptor.Field(f => f.Status)
            .Type<StatusDataModelType>()
            .Description("Status information about the success");

        descriptor.Field(f => f.MetaData)
            .Type<MetaDataModelType>()
            .Description("Metadata about the response");
    }
}

public class StatusDataModelType : ObjectType<StatusDataModel>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<StatusDataModel> descriptor)
    {
        descriptor.Name("StatusData");
        descriptor.Description("Status information for the response");

        descriptor.Field(f => f.StatusCode)
            .Description("HTTP status code");

        descriptor.Field(f => f.Message)
            .Description("Status message");
    }
}

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
