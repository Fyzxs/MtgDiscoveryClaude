using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.UserCards;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.UserCards;

public sealed class UserCardsForSigningArgEntityInputType : InputObjectType<UserCardsForSigningArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<UserCardsForSigningArgEntity> descriptor)
    {
        _ = descriptor.Name("UserCardsForSigningArgEntityInput")
            .Description("Input for querying user cards for convention signing planning");

        _ = descriptor.Field(x => x.UserId)
            .Name("userId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the user");
        _ = descriptor.Field(x => x.ArtistIds)
            .Name("artistIds")
            .Type<NonNullType<ListType<NonNullType<StringType>>>>()
            .Description("The collection of artist IDs to query cards for");
    }
}
