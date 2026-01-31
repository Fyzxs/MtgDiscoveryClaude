using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args;

internal sealed class ArtistNameArgEntityInputType : InputObjectType<ArtistNameArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<ArtistNameArgEntity> descriptor)
    {
        _ = descriptor.Name("ArtistNameInput")
            .Description("Input for querying cards by artist name");

        _ = descriptor.Field(x => x.ArtistName)
            .Name("artistName")
            .Type<NonNullType<StringType>>()
            .Description("The artist name to query");
        _ = descriptor.Field(x => x.UserId)
            .Name("userId")
            .Type<StringType>()
            .Description("Optional user identifier to enrich cards with collection data");
    }
}
