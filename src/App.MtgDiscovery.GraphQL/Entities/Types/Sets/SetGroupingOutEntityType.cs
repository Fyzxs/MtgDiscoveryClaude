using System;
using App.MtgDiscovery.GraphQL.Entities.Outs.Sets;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Sets;

public sealed class SetGroupingOutEntityType : ObjectType<SetGroupingOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<SetGroupingOutEntity> descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        descriptor.Field(f => f.Id).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.DisplayName).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.Order).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.CardCount).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.RawQuery).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.Filters).Type<GroupingFiltersOutEntityType>();
    }
}

public sealed class GroupingFiltersOutEntityType : ObjectType<GroupingFiltersOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<GroupingFiltersOutEntity> descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        descriptor.Field(f => f.CollectorNumberRange).Type<CollectorNumberRangeOutEntityType>();
        descriptor.Field(f => f.Properties).Type<AnyType>();
    }
}

public sealed class CollectorNumberRangeOutEntityType : ObjectType<CollectorNumberRangeOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<CollectorNumberRangeOutEntity> descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        descriptor.Field(f => f.Min).Type<StringType>();
        descriptor.Field(f => f.Max).Type<StringType>();
        descriptor.Field(f => f.OrConditions).Type<ListType<StringType>>();
    }
}