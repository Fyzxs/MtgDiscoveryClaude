using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.Artists;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Artists;

public sealed class ScryfallArtistOutEntityType : ObjectType<ScryfallArtistOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<ScryfallArtistOutEntity> descriptor)
    {
        descriptor.Name("ScryfallArtist")
            .Description("Artist data from Scryfall");

        descriptor.Field(f => f.ArtistId)
            .Name("artistId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier for the artist");
        descriptor.Field(f => f.ArtistNames)
            .Name("artistNames")
            .Type<NonNullType<ListType<NonNullType<StringType>>>>()
            .Description("All known names for this artist");
        descriptor.Field(f => f.ArtistNamesSearch)
            .Name("artistNamesSearch")
            .Type<NonNullType<StringType>>()
            .Description("Combined searchable artist names");
        descriptor.Field(f => f.CardIds)
            .Name("cardIds")
            .Type<NonNullType<ListType<NonNullType<StringType>>>>()
            .Description("IDs of cards illustrated by this artist");
        descriptor.Field(f => f.SetIds)
            .Name("setIds")
            .Type<NonNullType<ListType<NonNullType<StringType>>>>()
            .Description("IDs of sets containing cards by this artist");
    }
}
