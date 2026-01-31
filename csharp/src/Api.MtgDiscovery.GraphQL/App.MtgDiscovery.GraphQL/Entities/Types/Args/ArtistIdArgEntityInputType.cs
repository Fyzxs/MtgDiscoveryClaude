using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args;

internal sealed class ArtistIdArgEntityInputType : InputObjectType<ArtistIdArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<ArtistIdArgEntity> descriptor)
    {
        _ = descriptor.Name("ArtistIdInput")
            .Description("Input for querying cards by artist ID");

        _ = descriptor.Field(x => x.ArtistId)
            .Name("artistId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the artist");
        _ = descriptor.Field(x => x.UserId)
            .Name("userId")
            .Type<StringType>()
            .Description("Optional user identifier to enrich cards with collection data");
    }
}
