using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args;

internal sealed class CardIdsArgEntityInputType : InputObjectType<CardIdsArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<CardIdsArgEntity> descriptor)
    {
        _ = descriptor.Name("CardIdsInput")
            .Description("Input for querying cards by card IDs");

        _ = descriptor.Field(x => x.CardIds)
            .Name("cardIds")
            .Type<NonNullType<ListType<NonNullType<StringType>>>>()
            .Description("The collection of card IDs to query");
        _ = descriptor.Field(x => x.UserId)
            .Name("userId")
            .Type<StringType>()
            .Description("Optional user identifier to enrich cards with collection data");
    }
}
