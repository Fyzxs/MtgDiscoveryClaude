using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.UserSetCards;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.UserSetCards;

public sealed class AddSetGroupToUserSetCardArgEntityInputType : InputObjectType<AddSetGroupToUserSetCardArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<AddSetGroupToUserSetCardArgEntity> descriptor)
    {
        _ = descriptor.Name("AddSetGroupToUserSetCardInput")
            .Description("Input for adding a set group to a user's set cards collection");

        _ = descriptor.Field(x => x.SetId)
            .Name("setId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the set");
        _ = descriptor.Field(x => x.SetGroupId)
            .Name("setGroupId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the set group");
        _ = descriptor.Field(x => x.Collecting)
            .Name("collecting")
            .Type<NonNullType<BooleanType>>()
            .Description("Whether the user is collecting this set group");
        _ = descriptor.Field(x => x.Counts)
            .Name("counts")
            .Type<NonNullType<FinishCountsInputType>>()
            .Description("The card counts by finish type for this set group");
        _ = descriptor.Field(x => x.CollectingFinishes)
            .Name("collectingFinishes")
            .Type<NonNullType<ListType<NonNullType<StringType>>>>()
            .Description("The finishes being collected for this set group (e.g., 'nonFoil', 'foil', 'etched')");
    }
}
