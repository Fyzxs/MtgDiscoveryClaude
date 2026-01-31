using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.Collections;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Collections;

internal sealed class AuthorizedUsersSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<IEnumerable<AuthorizedUserOutEntity>>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<IEnumerable<AuthorizedUserOutEntity>>> descriptor)
    {
        descriptor.Name("AuthorizedUsersSuccessResponse")
            .Description("Response returned when a collection access list query is successful");

        descriptor.Field(f => f.Data)
            .Name("data")
            .Type<NonNullType<ListType<NonNullType<AuthorizedUserOutEntityType>>>>()
            .Description("The list of authorized users");
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
