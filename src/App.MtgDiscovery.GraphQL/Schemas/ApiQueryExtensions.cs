using App.MtgDiscovery.GraphQL.Entities.Types.Args.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.Args.UserSetCards;
using App.MtgDiscovery.GraphQL.Entities.Types.Cards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Entities.Types.User;
using App.MtgDiscovery.GraphQL.Entities.Types.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.UserSetCards;
using App.MtgDiscovery.GraphQL.Queries;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.MtgDiscovery.GraphQL.Schemas;

internal static class ApiQueryExtensions
{
    public static IRequestExecutorBuilder AddApiQuery(this IRequestExecutorBuilder builder)
    {
        return builder
            .AddQueryType<ApiQuery>()
            .AddTypeExtension<CardQueryMethods>()
            .AddTypeExtension<SetQueryMethods>()
            .AddTypeExtension<ArtistQueryMethods>()
            .AddTypeExtension<UserCardsQueryMethods>()
            .AddTypeExtension<UserSetCardsQueryMethods>()
            .AddTypeExtension<UserInfoQueryMethods>()
            // Input types for queries
            .AddType<UserCardsBySetArgEntityInputType>()
            .AddType<UserCardsByIdsArgEntityInputType>()
            .AddType<UserCardArgEntityInputType>()
            .AddType<CardResponseModelUnionType>()
            .AddType<FailureResponseModelType>()
            .AddType<CardsSuccessDataResponseModelType>()
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
            // UserCards query types
            .AddType<UserCardsCollectionResponseModelUnionType>()
            .AddType<UserCardsCollectionSuccessDataResponseModelType>()
            .AddType<UserCardCollectionOutEntityType>()
            .AddType<CollectedItemOutEntityType>()
            // UserSetCards query types
            .AddType<UserSetCardArgEntityInputType>()
            .AddType<UserSetCardResponseModelUnionType>()
            .AddType<UserSetCardSuccessDataResponseModelType>()
            .AddType<UserSetCardOutEntityType>()
            .AddType<UserSetCardRarityGroupOutEntityType>()
            .AddType<UserSetCardGroupOutEntityType>()
            .AddType<UserSetCardFinishGroupOutEntityType>()
            // UserInfo query types
            .AddType<UserInfoOutEntityType>()
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);
    }
}
