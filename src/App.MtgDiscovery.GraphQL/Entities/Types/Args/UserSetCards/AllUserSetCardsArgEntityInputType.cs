using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.UserSetCards;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.UserSetCards;

public sealed class AllUserSetCardsArgEntityInputType : InputObjectType<AllUserSetCardsArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<AllUserSetCardsArgEntity> descriptor)
    {
        _ = descriptor.Name("AllUserSetCardsInput")
            .Description("Input for querying all user set cards");

        _ = descriptor.Field(f => f.UserId)
            .Name("userId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier for the user");
    }
}
