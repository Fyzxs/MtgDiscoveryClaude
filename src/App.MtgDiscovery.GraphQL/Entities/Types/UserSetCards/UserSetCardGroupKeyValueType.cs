using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.UserSetCards;

public sealed class UserSetCardGroupKeyValueType : ObjectType<UserSetCardGroupKeyValue>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<UserSetCardGroupKeyValue> descriptor)
    {
        descriptor.Name("UserSetCardGroupKeyValue")
            .Description("A key-value pair for card rarity groups");

        descriptor.Field(f => f.Key)
            .Type<NonNullType<StringType>>()
            .Description("The rarity key (e.g., 'common', 'uncommon', 'rare', 'mythic')");

        descriptor.Field(f => f.Value)
            .Type<NonNullType<UserSetCardGroupOutEntityType>>()
            .Description("The card group data for this rarity");
    }
}