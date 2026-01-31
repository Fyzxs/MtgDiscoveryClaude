using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args;

internal sealed class CardNameArgEntityInputType : InputObjectType<CardNameArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<CardNameArgEntity> descriptor)
    {
        _ = descriptor.Name("CardNameInput")
            .Description("Input for querying cards by card name");

        _ = descriptor.Field(x => x.CardName)
            .Name("cardName")
            .Type<NonNullType<StringType>>()
            .Description("The card name to query");
        _ = descriptor.Field(x => x.UserId)
            .Name("userId")
            .Type<StringType>()
            .Description("Optional user identifier to enrich cards with collection data");
    }
}
