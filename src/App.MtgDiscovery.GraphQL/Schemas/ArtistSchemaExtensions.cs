using App.MtgDiscovery.GraphQL.Entities.Types.Artists;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Queries;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.MtgDiscovery.GraphQL.Schemas;

internal static class ArtistSchemaExtensions
{
    public static IRequestExecutorBuilder AddArtistSchemaExtensions(this IRequestExecutorBuilder builder)
    {
        return builder
            .AddTypeExtension<ArtistQueryMethods>()
            .AddType<ArtistSearchResultOutEntityType>()
            .AddType<ScryfallArtistOutEntityType>()
            .AddType<ArtistSearchResponseModelUnionType>()
            .AddType<CardsByArtistResponseModelUnionType>()
            .AddType<ArtistSearchResultsSuccessDataResponseModelType>()
            .AddType<CardsByArtistSuccessDataResponseModelType>();
    }
}
