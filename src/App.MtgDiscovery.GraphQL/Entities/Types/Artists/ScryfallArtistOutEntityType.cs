using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Outs.Artists;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Artists;

public sealed class ScryfallArtistOutEntityType : ObjectType<ScryfallArtistOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<ScryfallArtistOutEntity> descriptor)
    {
        descriptor.Name("ScryfallArtist");
        descriptor.Description("Artist data from Scryfall");

        descriptor.Field(f => f.ArtistId)
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier for the artist");

        descriptor.Field(f => f.ArtistNames)
            .Type<NonNullType<ListType<NonNullType<StringType>>>>()
            .Description("All known names for this artist");

        descriptor.Field(f => f.ArtistNamesSearch)
            .Type<NonNullType<StringType>>()
            .Description("Combined searchable artist names");

        descriptor.Field(f => f.CardIds)
            .Type<NonNullType<ListType<NonNullType<StringType>>>>()
            .Description("IDs of cards illustrated by this artist");

        descriptor.Field(f => f.SetIds)
            .Type<NonNullType<ListType<NonNullType<StringType>>>>()
            .Description("IDs of sets containing cards by this artist");
    }
}
