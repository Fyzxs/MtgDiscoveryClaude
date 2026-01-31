using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args;

internal sealed class ArtistSearchTermArgEntityInputType : InputObjectType<ArtistSearchTermArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<ArtistSearchTermArgEntity> descriptor)
    {
        _ = descriptor.Name("ArtistSearchTermInput")
            .Description("Input for searching artists by search term");

        _ = descriptor.Field(x => x.SearchTerm)
            .Name("searchTerm")
            .Type<NonNullType<StringType>>()
            .Description("The search term to use for artist search");
    }
}
