using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Outs.Sets;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Sets;

public sealed class GroupingFiltersOutEntityType : ObjectType<GroupingFiltersOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<GroupingFiltersOutEntity> descriptor)
    {
        descriptor.Field(f => f.CollectorNumberRange).Type<CollectorNumberRangeOutEntityType>();
        descriptor.Field(f => f.Properties).Type<AnyType>();
    }
}
