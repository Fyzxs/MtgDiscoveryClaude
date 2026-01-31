using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Sets;

internal sealed class SetGroupingFinishCountsType : ObjectType<FinishCountsOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<FinishCountsOutEntity> descriptor)
    {
        descriptor.Name("SetGroupingFinishCounts")
            .Description("Card finish counts within a set grouping");

        descriptor.Field(f => f.Total)
            .Name("total")
            .Type<NonNullType<IntType>>()
            .Description("Total number of cards in this grouping");

        descriptor.Field(f => f.NonFoil)
            .Name("nonFoil")
            .Type<NonNullType<IntType>>()
            .Description("Number of cards available in non-foil finish");

        descriptor.Field(f => f.Foil)
            .Name("foil")
            .Type<NonNullType<IntType>>()
            .Description("Number of cards available in foil finish");

        descriptor.Field(f => f.Etched)
            .Name("etched")
            .Type<NonNullType<IntType>>()
            .Description("Number of cards available in etched finish");
    }
}
