using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.Signing;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Signing;

public sealed class SigningSetGroupOutEntityType : ObjectType<SigningSetGroupOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SigningSetGroupOutEntity> descriptor)
    {
        descriptor.Name("SigningSetGroup")
            .Description("A set containing cards to be signed");

        descriptor.Field(f => f.SetId)
            .Name("setId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier for the set");
        descriptor.Field(f => f.SetCode)
            .Name("setCode")
            .Type<NonNullType<StringType>>()
            .Description("The set code (e.g., DOM, LEA)");
        descriptor.Field(f => f.SetName)
            .Name("setName")
            .Type<NonNullType<StringType>>()
            .Description("The full name of the set");
        descriptor.Field(f => f.ArtistCount)
            .Name("artistCount")
            .Type<NonNullType<IntType>>()
            .Description("Number of selected artists with cards in this set");
        descriptor.Field(f => f.UnsignedCardCount)
            .Name("unsignedCardCount")
            .Type<NonNullType<IntType>>()
            .Description("Total number of unsigned cards in this set");
        descriptor.Field(f => f.ReleasedAt)
            .Name("releasedAt")
            .Type<NonNullType<StringType>>()
            .Description("The release date of the set in ISO format (e.g., 1993-12-17)");
        descriptor.Field(f => f.Artists)
            .Name("artists")
            .Type<NonNullType<ListType<NonNullType<SigningArtistGroupOutEntityType>>>>()
            .Description("Artists with cards in this set");
    }
}
