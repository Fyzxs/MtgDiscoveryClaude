using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.UserCards;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.UserCards;

public sealed class UserCardsByIdsArgEntityInputType : InputObjectType<UserCardsByIdsArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<UserCardsByIdsArgEntity> descriptor)
    {
        descriptor.Name("UserCardsByIdsArgEntityInput");
        descriptor.Description("Input for querying user cards by card IDs");

        descriptor.Field(x => x.UserId)
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the user");

        descriptor.Field(x => x.CardIds)
            .Type<NonNullType<ListType<NonNullType<StringType>>>>()
            .Description("The collection of card IDs to query");
    }
}
