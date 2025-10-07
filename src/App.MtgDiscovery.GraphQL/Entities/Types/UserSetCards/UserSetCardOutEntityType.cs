using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.UserSetCards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.UserSetCards;

public sealed class UserSetCardOutEntityType : ObjectType<UserSetCardOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<UserSetCardOutEntity> descriptor)
    {
        descriptor.Name("UserSetCard")
            .Description("A user's collection summary for a specific set");

        descriptor.Field(f => f.UserId)
            .Name("userId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier for the user");
        descriptor.Field(f => f.SetId)
            .Name("setId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier for the set");
        descriptor.Field(f => f.TotalCards)
            .Name("totalCards")
            .Type<NonNullType<IntType>>()
            .Description("The total number of cards in this set");
        descriptor.Field(f => f.UniqueCards)
            .Name("uniqueCards")
            .Type<NonNullType<IntType>>()
            .Description("The number of unique cards the user has from this set");
        descriptor.Field(f => f.Groups)
            .Name("groups")
            .Type<NonNullType<ListType<NonNullType<UserSetCardRarityGroupOutEntityType>>>>()
            .Description("Card collection groups organized by rarity");
        descriptor.Field(f => f.Collecting)
            .Name("collecting")
            .Type<NonNullType<ListType<NonNullType<UserSetCardCollectingOutEntityType>>>>()
            .Description("Set groups that the user is actively collecting with counts");
    }
}
