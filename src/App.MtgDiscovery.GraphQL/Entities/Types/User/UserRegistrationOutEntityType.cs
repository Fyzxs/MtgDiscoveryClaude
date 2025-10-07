using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.User;

namespace App.MtgDiscovery.GraphQL.Entities.Types.User;

public sealed class UserRegistrationOutEntityType : ObjectType<UserRegistrationOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<UserRegistrationOutEntity> descriptor)
    {
        descriptor.Name("UserRegistration")
            .Description("A user registration result");

        descriptor.Field(f => f.UserId)
            .Name("userId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier for the user");
        descriptor.Field(f => f.DisplayName)
            .Name("displayName")
            .Type<NonNullType<StringType>>()
            .Description("The display name of the user");
    }
}
