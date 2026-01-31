using App.MtgDiscovery.GraphQL.Entities.Types.Args.Collections;
using App.MtgDiscovery.GraphQL.Entities.Types.Args.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.Args.UserSealedProducts;
using App.MtgDiscovery.GraphQL.Entities.Types.Args.UserSetCards;
using App.MtgDiscovery.GraphQL.Entities.Types.Args.UserWishlistCards;
using App.MtgDiscovery.GraphQL.Entities.Types.Cards;
using App.MtgDiscovery.GraphQL.Entities.Types.Collections;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Entities.Types.User;
using App.MtgDiscovery.GraphQL.Entities.Types.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.UserSetCards;
using App.MtgDiscovery.GraphQL.Entities.Types.UserWishlistCards;
using App.MtgDiscovery.GraphQL.Mutations;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.MtgDiscovery.GraphQL.Schemas;

internal static class ApiMutationExtensions
{
    public static IRequestExecutorBuilder AddApiMutation(this IRequestExecutorBuilder builder)
    {
        return builder
            .AddMutationType<ApiMutation>()
            .AddTypeExtension<UserMutationMethods>()
            .AddTypeExtension<UserCardsMutationMethods>()
            .AddTypeExtension<UserSetCardsMutationMethods>()
            .AddTypeExtension<UserWishlistCardsMutationMethods>()
            .AddTypeExtension<UserSealedProductsMutationMethods>()
            .AddTypeExtension<CollectionMutationMethods>()
            // Input types for mutations - Collections
            .AddType<CreateCollectionArgEntityInputType>()
            .AddType<RenameCollectionArgEntityInputType>()
            .AddType<UpdateCollectionVisibilityArgEntityInputType>()
            .AddType<GrantCollectionAccessArgEntityInputType>()
            .AddType<RevokeCollectionAccessArgEntityInputType>()
            .AddType<DeleteCollectionArgEntityInputType>()
            .AddType<TransferCollectionOwnershipArgEntityInputType>()
            // Input types for mutations - UserCards
            .AddType<AddCardToCollectionArgEntityInputType>()
            .AddType<CollectedItemArgEntityInputType>()
            // Input types for mutations - UserSetCards
            .AddType<AddSetGroupToUserSetCardArgEntityInputType>()
            // Response types for mutations
            .AddType<UserRegistrationResponseModelUnionType>()
            .AddType<UserRegistrationSuccessDataResponseModelType>()
            .AddType<UserSyncOutEntityType>()
            .AddType<AddCardToCollectionResponseModelUnionType>()
            .AddType<CardsSuccessDataResponseModelType>()
            .AddType<ScryfallCardOutEntityType>()
            .AddType<CollectedItemOutEntityType>()
            .AddType<UserSetCardResponseModelUnionType>()
            .AddType<UserSetCardSuccessDataResponseModelType>()
            .AddType<UserSetCardOutEntityType>()
            .AddType<UserSetCardCollectingOutEntityType>()
            .AddType<UserSetCardCollectionGroupOutEntityType>()
            .AddType<UserSetCardGroupOutEntityType>()
            .AddType<UserSetCardFinishGroupOutEntityType>()
            .AddType<FailureResponseModelType>()
            .AddType<StatusDataModelType>()
            .AddType<MetaDataModelType>()
            // UserWishlistCards input types
            .AddType<AddCardToWishlistArgEntityInputType>()
            .AddType<WishlistItemArgEntityInputType>()
            // UserSealedProducts input types
            .AddType<AddUserSealedProductArgEntityInputType>()
            .AddType<UserSealedProductDetailsArgEntityInputType>()
            // UserWishlistCards response types
            .AddType<AddCardToWishlistResponseModelUnionType>()
            .AddType<UserWishlistSuccessDataResponseModelType>()
            .AddType<UserWishlistCardOutEntityType>()
            .AddType<WishlistItemOutEntityType>()
            // UserSealedProducts response types
            .AddType<AddUserSealedProductResponseModelUnionType>()
            .AddType<AddUserSealedProductSuccessDataResponseModelType>()
            // Collection types
            .AddType<CollectionResponseModelUnionType>()
            .AddType<CollectionsSuccessDataResponseModelType>()
            .AddType<CollectionOutEntityType>()
            .AddType<AuthorizedUserOutEntityType>()
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);
    }
}
