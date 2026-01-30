using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Sets;

public sealed class GroupingFiltersOutEntityType : ObjectType<GroupingFiltersOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<GroupingFiltersOutEntity> descriptor)
    {
        descriptor.Name("GroupingFilters")
            .Description("Filters for card groupings");

        descriptor.Field(f => f.CollectorNumberRange)
            .Name("collectorNumberRange")
            .Type<CollectorNumberRangeOutEntityType>();
        descriptor.Field(f => f.Properties)
            .Name("properties")
            .Type<AnyType>();
    }
}
