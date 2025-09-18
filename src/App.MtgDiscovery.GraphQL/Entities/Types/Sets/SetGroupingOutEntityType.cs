using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Outs.Sets;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Sets;

public sealed class SetGroupingOutEntityType : ObjectType<SetGroupingOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SetGroupingOutEntity> descriptor)
    {
        descriptor.Field(f => f.Id).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.DisplayName).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.Order).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.CardCount).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.RawQuery).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.Filters).Type<GroupingFiltersOutEntityType>();
    }
}
