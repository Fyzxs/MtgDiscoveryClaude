using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args;

internal sealed class CardSearchTermArgEntityInputType : InputObjectType<CardSearchTermArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<CardSearchTermArgEntity> descriptor)
    {
        _ = descriptor.Name("CardSearchTermInput")
            .Description("Input for searching cards by search term");

        _ = descriptor.Field(x => x.SearchTerm)
            .Name("searchTerm")
            .Type<NonNullType<StringType>>()
            .Description("The search term to use for card search");
    }
}
