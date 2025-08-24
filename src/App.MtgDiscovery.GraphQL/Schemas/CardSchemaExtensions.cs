using App.MtgDiscovery.GraphQL.Entities.Types;
using App.MtgDiscovery.GraphQL.Queries;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.MtgDiscovery.GraphQL.Schemas;

internal static class CardSchemaExtensions
{
    public static IRequestExecutorBuilder AddCardSchema(this IRequestExecutorBuilder builder)
    {
        return builder
            .AddQueryType<CardQuery>()
            .AddTypeExtension<CardQueryMethods>()
            .AddType<ResponseModelUnionType>()
            .AddType<FailureResponseModelType>()
            .AddType<SuccessDataResponseModelType>()
            .AddType<StatusDataModelType>()
            .AddType<MetaDataModelType>()
            .AddType<ScryfallCardOutEntityType>()
            .AddType<ScryfallImageUrisOutEntityType>()
            .AddType<ScryfallLegalitiesOutEntityType>()
            .AddType<ScryfallPricesOutEntityType>()
            .AddType<ScryfallRelatedUrisOutEntityType>()
            .AddType<ScryfallPurchaseUrisOutEntityType>()
            .AddType<ScryfallCardFaceOutEntityType>()
            .AddType<ScryfallAllPartsOutEntityType>()
            .AddType<ScryfallPreviewOutEntityType>()
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);
    }
}
