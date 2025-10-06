using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.UserSetCards;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.UserSetCards;

public sealed class UserSetCardArgEntityInputType : InputObjectType<UserSetCardArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<UserSetCardArgEntity> descriptor)
    {
        _ = descriptor.Name("UserSetCardInput")
            .Description("Input for querying a user's set card collection");

        _ = descriptor.Field(f => f.UserId)
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier for the user");

        _ = descriptor.Field(f => f.SetId)
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier for the set");
    }
}
