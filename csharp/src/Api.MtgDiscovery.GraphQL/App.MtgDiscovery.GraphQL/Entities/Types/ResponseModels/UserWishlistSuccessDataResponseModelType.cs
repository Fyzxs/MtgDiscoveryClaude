using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserWishlistCards;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

internal sealed class UserWishlistSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<List<UserWishlistCardOutEntity>>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<List<UserWishlistCardOutEntity>>> descriptor)
    {
        descriptor.Name("UserWishlistSuccessDataResponseModel")
            .Description("Success response containing user wishlist cards");

        descriptor.Field(f => f.Data)
            .Name("data")
            .Type<NonNullType<ListType<NonNullType<ObjectType<UserWishlistCardOutEntity>>>>>()
            .Description("The list of wishlist cards");

        descriptor.Field(f => f.MetaData)
            .Name("metaData")
            .Type<NonNullType<MetaDataModelType>>()
            .Description("Response metadata");

        descriptor.Field(f => f.Status)
            .Name("status")
            .Type<NonNullType<StatusDataModelType>>()
            .Description("Response status information");
    }
}
