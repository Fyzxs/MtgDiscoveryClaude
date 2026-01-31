using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args;

internal sealed class SetCodesArgEntityInputType : InputObjectType<SetCodesArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<SetCodesArgEntity> descriptor)
    {
        _ = descriptor.Name("SetCodesInput")
            .Description("Input for querying sets by set codes");

        _ = descriptor.Field(x => x.SetCodes)
            .Name("setCodes")
            .Type<NonNullType<ListType<NonNullType<StringType>>>>()
            .Description("The collection of set codes to query");
        _ = descriptor.Field(x => x.UserId)
            .Name("userId")
            .Type<StringType>()
            .Description("Optional user identifier to enrich sets with collection data");
    }
}
