using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.UserCards;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.UserCards;

public sealed class UserCardsBySetArgEntityInputType : InputObjectType<UserCardsBySetArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<UserCardsBySetArgEntity> descriptor)
    {
        descriptor.Name("UserCardsSetArgEntityInput");
        descriptor.Description("Input for querying user cards by set");

        descriptor.Field(x => x.SetId)
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the set");

        descriptor.Field(x => x.UserId)
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the user");
    }
}
