using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.Signing;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Signing;

public sealed class SigningArtistGroupOutEntityType : ObjectType<SigningArtistGroupOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SigningArtistGroupOutEntity> descriptor)
    {
        descriptor.Name("SigningArtistGroup")
            .Description("An artist with cards to be signed");

        descriptor.Field(f => f.ArtistId)
            .Name("artistId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier for the artist");
        descriptor.Field(f => f.ArtistName)
            .Name("artistName")
            .Type<NonNullType<StringType>>()
            .Description("The name of the artist");
        descriptor.Field(f => f.UnsignedCount)
            .Name("unsignedCount")
            .Type<NonNullType<IntType>>()
            .Description("Number of unique cards with unsigned copies");
        descriptor.Field(f => f.PartiallySignedCount)
            .Name("partiallySignedCount")
            .Type<NonNullType<IntType>>()
            .Description("Number of cards with both signed and unsigned copies");
        descriptor.Field(f => f.Cards)
            .Name("cards")
            .Type<NonNullType<ListType<NonNullType<SigningCardOutEntityType>>>>()
            .Description("Cards by this artist");
    }
}
