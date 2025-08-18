using App.MtgDiscovery.GraphQL.Internal.Queries;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.MtgDiscovery.GraphQL.Internal.Schema;

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
