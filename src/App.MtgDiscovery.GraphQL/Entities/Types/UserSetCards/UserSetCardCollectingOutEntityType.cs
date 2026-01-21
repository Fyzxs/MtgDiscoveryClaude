using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.UserSetCards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.UserSetCards;

public sealed class UserSetCardCollectingOutEntityType : ObjectType<UserSetCardCollectingOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<UserSetCardCollectingOutEntity> descriptor)
    {
        descriptor.Name("UserSetCardCollecting")
            .Description("Collecting status for a specific set group");

        descriptor.Field(f => f.SetGroupId)
            .Type<NonNullType<StringType>>()
            .Description("The set group ID (e.g., 'normal', 'borderless')");

        descriptor.Field(f => f.Collecting)
            .Type<NonNullType<BooleanType>>()
            .Description("Whether the user is actively collecting this set group");

        descriptor.Field(f => f.Count)
            .Type<NonNullType<IntType>>()
            .Description("The count of cards in this collecting group");
    }
}
