using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.UserCards;

public sealed class UserCardCollectionOutEntityType : ObjectType<UserCardOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<UserCardOutEntity> descriptor)
    {
        descriptor.Name("UserCardCollection")
            .Description("A user's card collection entry");

        descriptor.Field(f => f.UserId)
            .Name("userId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier for the user");
        descriptor.Field(f => f.CardId)
            .Name("cardId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier for the card");
        descriptor.Field(f => f.SetId)
            .Name("setId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier for the set");
        descriptor.Field(f => f.CollectedList)
            .Name("collectedList")
            .Type<NonNullType<ListType<NonNullType<CollectedItemOutEntityType>>>>()
            .Description("The list of collected item variations");
    }
}
