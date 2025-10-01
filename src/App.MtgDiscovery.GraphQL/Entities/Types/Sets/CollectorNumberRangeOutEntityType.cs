using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.Sets;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Sets;

public sealed class CollectorNumberRangeOutEntityType : ObjectType<CollectorNumberRangeOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<CollectorNumberRangeOutEntity> descriptor)
    {
        descriptor.Field(f => f.Min).Type<StringType>();
        descriptor.Field(f => f.Max).Type<StringType>();
        descriptor.Field(f => f.OrConditions).Type<ListType<StringType>>();
    }
}
