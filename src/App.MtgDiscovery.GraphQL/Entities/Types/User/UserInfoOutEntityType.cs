using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.User;

public sealed class UserInfoOutEntityType : ObjectType<UserInfoOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<UserInfoOutEntity> descriptor)
    {
        descriptor.Name("UserInfo");
        descriptor.Description("Basic user information extracted from JWT");

        descriptor.Field(x => x.UserId)
            .Type<NonNullType<StringType>>()
            .Description("The unique user ID generated from JWT claims");

        descriptor.Field(x => x.Email)
            .Type<NonNullType<StringType>>()
            .Description("The user's email address from JWT claims");
    }
}
