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
        descriptor.Name("SuccessUserCardCollectionResponse");
        descriptor.Description("Response returned when adding cards to collection is successful");

        descriptor.Field(f => f.Data)
            .Type<NonNullType<UserCardCollectionOutEntityType>>()
            .Description("The user card collection result");

        descriptor.Field(f => f.Status)
            .Type<StatusDataModelType>()
            .Description("Status information about the success");

        descriptor.Field(f => f.MetaData)
            .Type<MetaDataModelType>()
            .Description("Metadata about the response");
    }
}
