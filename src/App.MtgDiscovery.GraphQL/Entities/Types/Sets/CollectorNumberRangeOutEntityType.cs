using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.Sets;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Sets;

public sealed class CollectorNumberRangeOutEntityType : ObjectType<CollectorNumberRangeOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<CollectorNumberRangeOutEntity> descriptor)
    {
        descriptor.Name("CollectorNumberRange")
            .Description("Range of collector numbers");

        descriptor.Field(f => f.Min)
            .Name("min")
            .Type<StringType>();
        descriptor.Field(f => f.Max)
            .Name("max")
            .Type<StringType>();
        descriptor.Field(f => f.OrConditions)
            .Name("orConditions")
            .Type<ListType<StringType>>();
    }
}
