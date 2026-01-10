using App.MtgDiscovery.GraphQL.Entities.Types.Args.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.Args.UserSetCards;
using App.MtgDiscovery.GraphQL.Entities.Types.Args.UserWishlistCards;
using App.MtgDiscovery.GraphQL.Entities.Types.Cards;
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
            // Input types for mutations
            .AddType<AddCardToCollectionArgEntityInputType>()
            .AddType<CollectedItemArgEntityInputType>()
            .AddType<AddSetGroupToUserSetCardArgEntityInputType>()
            // Response types for mutations
            .AddType<UserRegistrationResponseModelUnionType>()
            .AddType<UserRegistrationSuccessDataResponseModelType>()
            .AddType<UserRegistrationOutEntityType>()
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
            // UserWishlistCards response types
            .AddType<AddCardToWishlistResponseModelUnionType>()
            .AddType<UserWishlistSuccessDataResponseModelType>()
            .AddType<UserWishlistCardOutEntityType>()
            .AddType<WishlistItemOutEntityType>()
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);
    }
}
