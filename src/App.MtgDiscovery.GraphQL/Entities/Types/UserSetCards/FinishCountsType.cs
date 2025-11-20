using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.UserSetCards;

public sealed class FinishCountsType : ObjectType<FinishCountsOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<FinishCountsOutEntity> descriptor)
    {
        descriptor.Name("FinishCounts")
            .Description("Card finish counts by type");

        descriptor.Field(x => x.Total)
            .Name("total")
            .Type<NonNullType<IntType>>()
            .Description("Total count across all finishes");

        descriptor.Field(x => x.NonFoil)
            .Name("nonFoil")
            .Type<NonNullType<IntType>>()
            .Description("Count of non-foil cards");

        descriptor.Field(x => x.Foil)
            .Name("foil")
            .Type<NonNullType<IntType>>()
            .Description("Count of foil cards");

        descriptor.Field(x => x.Etched)
            .Name("etched")
            .Type<NonNullType<IntType>>()
            .Description("Count of etched cards");
    }
}
