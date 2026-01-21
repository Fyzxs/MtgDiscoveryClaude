using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Types.UserSealedProducts;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSealedProducts;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class AddUserSealedProductSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<AddUserSealedProductResultOutEntity>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<AddUserSealedProductResultOutEntity>> descriptor)
    {
        descriptor.Name("AddUserSealedProductSuccessResponse")
            .Description("Response returned when a sealed product is successfully added to user collection");

        descriptor.Field(f => f.Data)
            .Name("data")
            .Type<NonNullType<AddUserSealedProductResultOutEntityType>>()
            .Description("The result of adding the sealed product");
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
