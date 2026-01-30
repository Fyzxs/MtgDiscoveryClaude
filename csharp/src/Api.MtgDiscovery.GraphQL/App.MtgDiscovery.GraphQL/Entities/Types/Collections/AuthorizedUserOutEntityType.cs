using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.Collections;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Collections;

public sealed class AuthorizedUserOutEntityType : ObjectType<AuthorizedUserOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<AuthorizedUserOutEntity> descriptor)
    {
        descriptor.Name("AuthorizedUser")
            .Description("A user authorized to access a collection");

        descriptor.Field(f => f.UserId)
            .Name("userId")
            .Type<NonNullType<StringType>>();
        descriptor.Field(f => f.Role)
            .Name("role")
            .Type<NonNullType<StringType>>();
        descriptor.Field(f => f.GrantedAt)
            .Name("grantedAt")
            .Type<NonNullType<StringType>>();
        descriptor.Field(f => f.GrantedBy)
            .Name("grantedBy")
            .Type<NonNullType<StringType>>();
    }
}
