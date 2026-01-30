using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using App.MtgDiscovery.GraphQL.Entities.Args.UserSetCards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.UserSetCards;

public sealed class FinishCountsInputType : InputObjectType<FinishCountsArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<FinishCountsArgEntity> descriptor)
    {
        _ = descriptor.Name("FinishCountsInput")
            .Description("Card finish counts by type");

        _ = descriptor.Field(x => x.Total)
            .Name("total")
            .Type<NonNullType<IntType>>()
            .Description("Total count across all finishes");

        _ = descriptor.Field(x => x.NonFoil)
            .Name("nonFoil")
            .Type<NonNullType<IntType>>()
            .Description("Count of non-foil cards");

        _ = descriptor.Field(x => x.Foil)
            .Name("foil")
            .Type<NonNullType<IntType>>()
            .Description("Count of foil cards");

        _ = descriptor.Field(x => x.Etched)
            .Name("etched")
            .Type<NonNullType<IntType>>()
            .Description("Count of etched cards");
    }
}
