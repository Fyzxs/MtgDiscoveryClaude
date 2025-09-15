using App.MtgDiscovery.GraphQL.Entities.Outs.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate.Types;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.UserCards;

public sealed class UserCardCollectionResponseModelUnionType : UnionType<ResponseModel>
{
    protected override void Configure(IUnionTypeDescriptor descriptor)
    {
        if (descriptor is null) { return; }

        descriptor
            .Name("UserCardCollectionResponseModel")
            .Type<ObjectType<SuccessDataResponseModel<UserCardCollectionOutEntity>>>()
            .Type<ObjectType<FailureResponseModel>>();
    }
}
