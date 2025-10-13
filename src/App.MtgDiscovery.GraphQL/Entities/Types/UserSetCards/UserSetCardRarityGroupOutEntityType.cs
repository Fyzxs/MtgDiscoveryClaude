using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.UserSetCards;

public sealed class UserSetCardRarityGroupOutEntityType : ObjectType<UserSetCardRarityGroupOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<UserSetCardRarityGroupOutEntity> descriptor)
    {
        descriptor.Name("UserSetCardRarityGroup")
            .Description("A card group organized by rarity");

        descriptor.Field(f => f.Rarity)
            .Name("rarity")
            .Type<NonNullType<StringType>>()
            .Description("The rarity key (e.g., 'common', 'uncommon', 'rare', 'mythic')");
        descriptor.Field(f => f.Group)
            .Name("group")
            .Type<NonNullType<UserSetCardGroupOutEntityType>>()
            .Description("The card group data for this rarity");
    }
}
