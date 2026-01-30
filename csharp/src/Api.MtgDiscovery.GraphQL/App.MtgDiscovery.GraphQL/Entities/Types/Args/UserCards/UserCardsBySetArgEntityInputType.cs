using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.UserCards;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.UserCards;

public sealed class UserCardsBySetArgEntityInputType : InputObjectType<UserCardsBySetArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<UserCardsBySetArgEntity> descriptor)
    {
        _ = descriptor.Name("UserCardsBySetInput")
            .Description("Input for querying user cards by set");

        _ = descriptor.Field(x => x.SetId)
            .Name("setId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the set");
        _ = descriptor.Field(x => x.UserId)
            .Name("userId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the user");
    }
}
