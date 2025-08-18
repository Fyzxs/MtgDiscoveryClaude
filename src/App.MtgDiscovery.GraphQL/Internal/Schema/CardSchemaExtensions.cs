using App.MtgDiscovery.GraphQL.Internal.Queries;
using App.MtgDiscovery.GraphQL.Internal.Schema.Types;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.MtgDiscovery.GraphQL.Internal.Schema;

internal static class CardSchemaExtensions
{
    public static IRequestExecutorBuilder AddCardSchema(this IRequestExecutorBuilder builder)
    {
        return builder
            .AddQueryType<CardQuery>()
            .AddTypeExtension<CardQueryMethods>()
            .AddType<ScryfallCardEntityExtension>()
            .AddType<ScryfallImageUrisTypeExtension>()
            .AddType<ScryfallLegalitiesTypeExtension>()
            .AddType<ScryfallPricesTypeExtension>()
            .AddType<ScryfallRelatedUrisTypeExtension>()
            .AddType<ScryfallPurchaseUrisTypeExtension>()
            .AddType<ScryfallCardFaceTypeExtension>()
            .AddType<ScryfallAllPartsTypeExtension>()
            .AddType<ScryfallPreviewTypeExtension>()
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);
    }
}
