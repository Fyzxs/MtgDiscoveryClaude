using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Types.Signing;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.Signing;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

internal sealed class SigningResultSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<SigningResultOutEntity>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<SigningResultOutEntity>> descriptor)
    {
        descriptor.Name("SigningResultSuccessResponse")
            .Description("Response returned when querying user cards for signing is successful");

        descriptor.Field(f => f.Data)
            .Name("data")
            .Type<NonNullType<SigningResultOutEntityType>>()
            .Description("The signing result data");
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
