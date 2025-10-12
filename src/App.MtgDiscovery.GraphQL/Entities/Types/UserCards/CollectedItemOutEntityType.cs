using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.UserCards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.UserCards;

public sealed class CollectedItemOutEntityType : ObjectType<CollectedItemOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<CollectedItemOutEntity> descriptor)
    {
        descriptor.Name("CollectedItem")
            .Description("A collected item variant with finish and special treatment");

        descriptor.Field(f => f.Finish)
            .Name("finish")
            .Type<NonNullType<StringType>>()
            .Description("The finish type (nonfoil, foil, etched)");
        descriptor.Field(f => f.Special)
            .Name("special")
            .Type<NonNullType<StringType>>()
            .Description("The special treatment (none, showcase, extended, retro, promo, altered, serialized)");
        descriptor.Field(f => f.Count)
            .Name("count")
            .Type<NonNullType<IntType>>()
            .Description("The number of this variant owned");
    }
}
