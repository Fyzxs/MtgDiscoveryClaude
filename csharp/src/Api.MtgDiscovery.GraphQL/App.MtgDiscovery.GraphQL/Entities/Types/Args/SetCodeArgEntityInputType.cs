using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args;

internal sealed class SetCodeArgEntityInputType : InputObjectType<SetCodeArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<SetCodeArgEntity> descriptor)
    {
        _ = descriptor.Name("SetCodeInput")
            .Description("Input for querying cards by set code");

        _ = descriptor.Field(x => x.SetCode)
            .Name("setCode")
            .Type<NonNullType<StringType>>()
            .Description("The set code to query");
        _ = descriptor.Field(x => x.UserId)
            .Name("userId")
            .Type<StringType>()
            .Description("Optional user identifier to enrich cards with collection data");
    }
}
