using App.MtgDiscovery.GraphQL.Entities.Types.Args.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Entities.Types.User;
using App.MtgDiscovery.GraphQL.Entities.Types.UserCards;
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
            // Input types for mutations
            .AddType<AddCardToCollectionArgEntityInputType>()
            .AddType<CollectedItemArgEntityInputType>()
            // Response types for mutations
            .AddType<UserRegistrationResponseModelUnionType>()
            .AddType<UserRegistrationSuccessDataResponseModelType>()
            .AddType<UserRegistrationOutEntityType>()
            .AddType<UserCardCollectionResponseModelUnionType>()
            .AddType<UserCardCollectionSuccessDataResponseModelType>()
            .AddType<UserCardCollectionOutEntityType>()
            .AddType<CollectedItemOutEntityType>()
            .AddType<FailureResponseModelType>()
            .AddType<StatusDataModelType>()
            .AddType<MetaDataModelType>()
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);
    }
}
