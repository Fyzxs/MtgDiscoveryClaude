using App.MtgDiscovery.GraphQL.Entities.Types.Args.UserSetCards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Entities.Types.UserSetCards;
using App.MtgDiscovery.GraphQL.Queries;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.MtgDiscovery.GraphQL.Schemas;

internal static class UserSetCardsSchemaExtensions
{
    public static IRequestExecutorBuilder AddUserSetCardsSchema(this IRequestExecutorBuilder builder)
    {
        return builder
            .AddTypeExtension<UserSetCardsQueryMethods>()
            .AddTypeExtension<AllUserSetCardsQueryMethods>()
            // Input types
            .AddType<UserSetCardArgEntityInputType>()
            .AddType<AllUserSetCardsArgEntityInputType>()
            .AddType<AddSetGroupToUserSetCardArgEntityInputType>()
            // Output types
            .AddType<UserSetCardOutEntityType>()
            .AddType<UserSetCardGroupOutEntityType>()
            .AddType<UserSetCardFinishGroupOutEntityType>()
            .AddType<UserSetCardCollectingOutEntityType>()
            // Response union types
            .AddType<UserSetCardResponseModelUnionType>()
            .AddType<UserSetCardSuccessDataResponseModelType>()
            .AddType<AllUserSetCardsResponseModelUnionType>()
            .AddType<AllUserSetCardsSuccessDataResponseModelType>();
    }
}
