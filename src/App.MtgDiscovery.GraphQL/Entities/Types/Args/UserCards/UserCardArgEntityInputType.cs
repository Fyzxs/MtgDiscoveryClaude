using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.UserCards;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.UserCards;

public sealed class UserCardArgEntityInputType : InputObjectType<UserCardArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<UserCardArgEntity> descriptor)
    {
        _ = descriptor.Name("UserCardInput")
            .Description("Input for querying a specific user card");

        _ = descriptor.Field(x => x.UserId)
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the user");

        _ = descriptor.Field(x => x.CardId)
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the card");
    }
}
