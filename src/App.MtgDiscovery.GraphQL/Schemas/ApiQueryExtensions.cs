using App.MtgDiscovery.GraphQL.Entities.Types.Args.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.Cards;
using App.MtgDiscovery.GraphQL.Entities.Types.Collections;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Entities.Types.Signing;
using App.MtgDiscovery.GraphQL.Entities.Types.User;
using App.MtgDiscovery.GraphQL.Entities.Types.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.UserWishlistCards;
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
            .AddTypeExtension<UserInfoQueryMethods>()
            .AddTypeExtension<UserWishlistCardsQueryMethods>()
            .AddTypeExtension<CollectionQueryMethods>()
            // Input types for queries
            .AddType<UserCardsBySetArgEntityInputType>()
            .AddType<UserCardsByIdsArgEntityInputType>()
            .AddType<UserCardArgEntityInputType>()
            .AddType<UserCardsForSigningArgEntityInputType>()
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
            // Signing query types
            .AddType<SigningResultResponseModelUnionType>()
            .AddType<SigningResultSuccessDataResponseModelType>()
            .AddType<SigningResultOutEntityType>()
            .AddType<SigningSetGroupOutEntityType>()
            .AddType<SigningArtistGroupOutEntityType>()
            .AddType<SigningCardOutEntityType>()
            // UserInfo query types
            .AddType<UserInfoOutEntityType>()
            // UserWishlistCards query types
            .AddType<UserWishlistResponseModelUnionType>()
            .AddType<UserWishlistSuccessDataResponseModelType>()
            .AddType<UserWishlistCardOutEntityType>()
            .AddType<WishlistItemOutEntityType>()
            // Collections query types
            .AddType<CollectionsResponseModelUnionType>()
            .AddType<CollectionsSuccessDataResponseModelType>()
            // AuthorizedUsers query types
            .AddType<AuthorizedUsersResponseModelUnionType>()
            .AddType<AuthorizedUsersSuccessDataResponseModelType>()
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);
    }
}
