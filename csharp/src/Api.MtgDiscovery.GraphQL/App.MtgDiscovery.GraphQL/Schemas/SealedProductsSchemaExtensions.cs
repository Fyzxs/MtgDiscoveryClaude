using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Entities.Types.SealedProducts;
using App.MtgDiscovery.GraphQL.Queries;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.MtgDiscovery.GraphQL.Schemas;

internal static class SealedProductsSchemaExtensions
{
    public static IRequestExecutorBuilder AddSealedProductsSchemaExtensions(this IRequestExecutorBuilder builder)
    {
        return builder
            .AddTypeExtension<SealedProductsQueryMethods>()
            .AddType<SealedProductType>()
            .AddType<SealedProductsSuccessDataResponseModelType>()
            .AddType<SealedProductsResponseModelUnionType>();
    }
}
