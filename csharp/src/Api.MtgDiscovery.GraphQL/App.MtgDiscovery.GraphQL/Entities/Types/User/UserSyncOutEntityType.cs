using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.User;

namespace App.MtgDiscovery.GraphQL.Entities.Types.User;

public sealed class UserSyncOutEntityType : ObjectType<UserSyncOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<UserSyncOutEntity> descriptor)
    {
        descriptor.Name("UserSync")
            .Description("A user sync result containing login status information");

        descriptor.Field(f => f.UserId)
            .Name("userId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier for the user");
        descriptor.Field(f => f.DisplayName)
            .Name("displayName")
            .Type<NonNullType<StringType>>()
            .Description("The display name of the user");
        descriptor.Field(f => f.Email)
            .Name("email")
            .Type<NonNullType<StringType>>()
            .Description("The email address of the user");
        descriptor.Field(f => f.CreatedAt)
            .Name("createdAt")
            .Type<NonNullType<DateTimeType>>()
            .Description("When the user account was created");
        descriptor.Field(f => f.LastLoginAt)
            .Name("lastLoginAt")
            .Type<NonNullType<DateTimeType>>()
            .Description("When the user last logged in");
        descriptor.Field(f => f.IsFirstLogin)
            .Name("isFirstLogin")
            .Type<NonNullType<BooleanType>>()
            .Description("Whether this is the user's first login (true for new users, false for returning users)");
    }
}
