using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;

namespace App.MtgDiscovery.GraphQL.Entities.Types.SealedProducts;

internal sealed class SealedProductType : ObjectType<SealedProductOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<SealedProductOutEntity> descriptor)
    {
        descriptor.Name("SealedProduct")
            .Description("Represents a Magic: The Gathering sealed product");

        descriptor.Field(f => f.Uuid)
            .Name("uuid")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the sealed product");
        descriptor.Field(f => f.SetId)
            .Name("setId")
            .Type<NonNullType<StringType>>()
            .Description("The set identifier this product belongs to");
        descriptor.Field(f => f.SetCode)
            .Name("setCode")
            .Type<NonNullType<StringType>>()
            .Description("The set code this product belongs to");
        descriptor.Field(f => f.SetName)
            .Name("setName")
            .Type<StringType>()
            .Description("The name of the set this product belongs to");
        descriptor.Field(f => f.Name)
            .Name("name")
            .Type<NonNullType<StringType>>()
            .Description("The name of the sealed product");
        descriptor.Field(f => f.Category)
            .Name("category")
            .Type<StringType>()
            .Description("The category of the product (booster_box, booster_pack, bundle, etc.)");
        descriptor.Field(f => f.Subtype)
            .Name("subtype")
            .Type<StringType>()
            .Description("The subtype of the product");
        descriptor.Field(f => f.CardCount)
            .Name("cardCount")
            .Type<IntType>()
            .Description("The number of cards in this product");
        descriptor.Field(f => f.ReleaseDate)
            .Name("releaseDate")
            .Type<StringType>()
            .Description("The release date of the product");
        descriptor.Field(f => f.TcgplayerProductId)
            .Name("tcgplayerProductId")
            .Type<StringType>()
            .Description("The TCGPlayer product ID");
        descriptor.Field(f => f.ImageUrl)
            .Name("imageUrl")
            .Type<StringType>()
            .Description("The URL to the product image");
        descriptor.Field(f => f.PurchaseUrlTcgplayer)
            .Name("purchaseUrlTcgplayer")
            .Type<StringType>()
            .Description("The TCGPlayer purchase URL");
        descriptor.Field(f => f.PurchaseUrlCardmarket)
            .Name("purchaseUrlCardmarket")
            .Type<StringType>()
            .Description("The Cardmarket purchase URL");
        descriptor.Field(f => f.PurchaseUrlCardKingdom)
            .Name("purchaseUrlCardKingdom")
            .Type<StringType>()
            .Description("The Card Kingdom purchase URL");
    }
}
