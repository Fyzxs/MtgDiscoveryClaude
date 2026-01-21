using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.UserSetCards;

public sealed class UserSetCardCollectionGroupOutEntityType : ObjectType<UserSetCardCollectionGroupOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<UserSetCardCollectionGroupOutEntity> descriptor)
    {
        descriptor.Name("UserSetCardCollectionGroup")
            .Description("A card group for a set group (e.g., 'booster', 'extended-art', 'promo')");

        descriptor.Field(f => f.SetGroupId)
            .Name("setGroupId")
            .Type<NonNullType<StringType>>()
            .Description("The set group identifier (e.g., 'booster', 'extended-art', 'promo', 'showcase')");
        descriptor.Field(f => f.Group)
            .Name("group")
            .Type<NonNullType<UserSetCardGroupOutEntityType>>()
            .Description("The card group data for this set group");
    }
}
