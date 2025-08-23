using App.MtgDiscovery.GraphQL.Queries;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.MtgDiscovery.GraphQL.Schema;

internal static class ExampleSchemaExtensions
{
    public static IRequestExecutorBuilder AddExampleSchema(this IRequestExecutorBuilder builder)
    {
        return builder
            .AddQueryType<ExampleQuery>()
            .AddTypeExtension<ExampleQueryMethods>()
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);
    }
}
