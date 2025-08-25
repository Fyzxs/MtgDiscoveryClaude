using App.MtgDiscovery.GraphQL.Entities.Types.Sets;
using App.MtgDiscovery.GraphQL.Queries;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.MtgDiscovery.GraphQL.Schemas;

internal static class SetSchemaExtensions
{
    public static IRequestExecutorBuilder AddSetSchemaExtensions(this IRequestExecutorBuilder builder)
    {
        return builder
            .AddTypeExtension<SetQueryMethods>()
            .AddType<ScryfallSetOutEntityType>();
    }
}
