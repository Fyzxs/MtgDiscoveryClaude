using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.Sets;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Sets;

public sealed class SetGroupingOutEntityType : ObjectType<SetGroupingOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SetGroupingOutEntity> descriptor)
    {
        descriptor.Name("SetGrouping")
            .Description("Card grouping within a set");

        descriptor.Field(f => f.Id)
            .Name("id")
            .Type<NonNullType<StringType>>();
        descriptor.Field(f => f.DisplayName)
            .Name("displayName")
            .Type<NonNullType<StringType>>();
        descriptor.Field(f => f.Order)
            .Name("order")
            .Type<NonNullType<IntType>>();
        descriptor.Field(f => f.CardCount)
            .Name("cardCount")
            .Type<NonNullType<IntType>>();
        descriptor.Field(f => f.RawQuery)
            .Name("rawQuery")
            .Type<NonNullType<StringType>>();
        descriptor.Field(f => f.Filters)
            .Name("filters")
            .Type<GroupingFiltersOutEntityType>();
    }
}
