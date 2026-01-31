using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserWishlistCards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.UserWishlistCards;

internal sealed class UserWishlistCardOutEntityType : ObjectType<UserWishlistCardOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<UserWishlistCardOutEntity> descriptor)
    {
        descriptor.Name("UserWishlistCard")
            .Description("A card in a user's wishlist with associated wishlist items");

        descriptor.Field(f => f.UserId)
            .Name("userId")
            .Type<NonNullType<StringType>>()
            .Description("The user's unique identifier");
        descriptor.Field(f => f.CardId)
            .Name("cardId")
            .Type<NonNullType<StringType>>()
            .Description("The Scryfall card ID");
        descriptor.Field(f => f.SetId)
            .Name("setId")
            .Type<NonNullType<StringType>>()
            .Description("The Scryfall set ID");
        descriptor.Field(f => f.CardName)
            .Name("cardName")
            .Type<NonNullType<StringType>>()
            .Description("The name of the card");
        descriptor.Field(f => f.SetName)
            .Name("setName")
            .Type<NonNullType<StringType>>()
            .Description("The name of the set");
        descriptor.Field(f => f.SetCode)
            .Name("setCode")
            .Type<NonNullType<StringType>>()
            .Description("The set code");
        descriptor.Field(f => f.ArtistIds)
            .Name("artistIds")
            .Type<NonNullType<ListType<NonNullType<StringType>>>>()
            .Description("The artist IDs for this card");
        descriptor.Field(f => f.CardNameGuid)
            .Name("cardNameGuid")
            .Type<NonNullType<StringType>>()
            .Description("The deterministic GUID for the card name");
        descriptor.Field(f => f.CreatedAt)
            .Name("createdAt")
            .Type<NonNullType<StringType>>()
            .Description("When this wishlist entry was created");
        descriptor.Field(f => f.UpdatedAt)
            .Name("updatedAt")
            .Type<NonNullType<StringType>>()
            .Description("When this wishlist entry was last updated");
        descriptor.Field(f => f.WishlistItems)
            .Name("wishlistItems")
            .Type<NonNullType<ListType<NonNullType<WishlistItemOutEntityType>>>>()
            .Description("The wishlist items for this card");
    }
}
