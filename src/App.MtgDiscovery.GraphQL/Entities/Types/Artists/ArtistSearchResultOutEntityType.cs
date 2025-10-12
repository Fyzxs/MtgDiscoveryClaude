using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.Artists;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Artists;

public sealed class ArtistSearchResultOutEntityType : ObjectType<ArtistSearchResultOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<ArtistSearchResultOutEntity> descriptor)
    {
        descriptor.Name("ArtistSearchResult")
            .Description("An artist search result");

        descriptor.Field(f => f.ArtistId)
            .Name("artistId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier for the artist");
        descriptor.Field(f => f.Name)
            .Name("name")
            .Type<NonNullType<StringType>>()
            .Description("The name of the artist");
    }
}
