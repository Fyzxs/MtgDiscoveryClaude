using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args;

internal sealed class AllSetsArgEntityInputType : InputObjectType<AllSetsArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<AllSetsArgEntity> descriptor)
    {
        _ = descriptor.Name("AllSetsInput")
            .Description("Input for querying all sets");

        _ = descriptor.Field(x => x.UserId)
            .Name("userId")
            .Type<StringType>()
            .Description("Optional user identifier to enrich sets with collection data");
    }
}
