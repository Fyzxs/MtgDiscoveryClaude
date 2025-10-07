using App.MtgDiscovery.GraphQL.Entities.Types.Args.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.Args.UserSetCards;
using App.MtgDiscovery.GraphQL.Entities.Types.Cards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Entities.Types.User;
using App.MtgDiscovery.GraphQL.Entities.Types.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.UserSetCards;
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
            .AddType<UserSetCardRarityGroupOutEntityType>()
            .AddType<UserSetCardGroupOutEntityType>()
            .AddType<UserSetCardFinishGroupOutEntityType>()
            .AddType<FailureResponseModelType>()
            .AddType<StatusDataModelType>()
            .AddType<MetaDataModelType>()
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);
    }
}
