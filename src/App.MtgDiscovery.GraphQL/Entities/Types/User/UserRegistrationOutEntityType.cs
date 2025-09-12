using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Outs.User;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.User;

public sealed class UserRegistrationOutEntityType : ObjectType<UserRegistrationOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<UserRegistrationOutEntity> descriptor)
    {
        descriptor.Name("UserRegistration");
        descriptor.Description("A user registration result");

        descriptor.Field(f => f.UserId)
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier for the user");

        descriptor.Field(f => f.DisplayName)
            .Type<NonNullType<StringType>>()
            .Description("The display name of the user");
    }
}
