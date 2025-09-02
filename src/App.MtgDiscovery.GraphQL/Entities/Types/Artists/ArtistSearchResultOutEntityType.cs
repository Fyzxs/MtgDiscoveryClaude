using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Outs.Artists;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Artists;

public sealed class ArtistSearchResultOutEntityType : ObjectType<ArtistSearchResultOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<ArtistSearchResultOutEntity> descriptor)
    {
        descriptor.Name("ArtistSearchResult");
        descriptor.Description("An artist search result");

        descriptor.Field(f => f.ArtistId)
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier for the artist");

        descriptor.Field(f => f.Name)
            .Type<NonNullType<StringType>>()
            .Description("The name of the artist");
    }
}
