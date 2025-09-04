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
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);
    }
}