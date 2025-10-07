using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Types.UserCards;
using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.UserCards;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class UserCardCollectionSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<UserCardOutEntity>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<UserCardOutEntity>> descriptor)
    {
        descriptor.Name("UserCardCollectionSuccessResponse")
            .Description("Response returned when adding cards to collection is successful");

        descriptor.Field(f => f.Data)
            .Name("data")
            .Type<NonNullType<UserCardCollectionOutEntityType>>()
            .Description("The user card collection result");
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
