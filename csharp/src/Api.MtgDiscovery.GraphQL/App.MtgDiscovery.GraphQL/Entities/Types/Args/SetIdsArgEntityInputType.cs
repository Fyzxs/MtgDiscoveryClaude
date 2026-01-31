using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args;

internal sealed class SetIdsArgEntityInputType : InputObjectType<SetIdsArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<SetIdsArgEntity> descriptor)
    {
        _ = descriptor.Name("SetIdsInput")
            .Description("Input for querying sets by set IDs");

        _ = descriptor.Field(x => x.SetIds)
            .Name("setIds")
            .Type<NonNullType<ListType<NonNullType<StringType>>>>()
            .Description("The collection of set IDs to query");
        _ = descriptor.Field(x => x.UserId)
            .Name("userId")
            .Type<StringType>()
            .Description("Optional user identifier to enrich sets with collection data");
    }
}
