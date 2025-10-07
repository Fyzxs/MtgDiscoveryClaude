using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.Cards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Cards;

internal sealed class ScryfallPurchaseUrisOutEntityType : ObjectType<PurchaseUrisOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<PurchaseUrisOutEntity> descriptor)
    {
        descriptor.Name("PurchaseUris")
            .Description("URIs for purchasing the card");

        descriptor.Field(f => f.Tcgplayer)
            .Name("tcgplayer")
            .Type<StringType>()
            .Description("TCGPlayer purchase link");
        descriptor.Field(f => f.Cardmarket)
            .Name("cardmarket")
            .Type<StringType>()
            .Description("Cardmarket purchase link");
        descriptor.Field(f => f.Cardhoarder)
            .Name("cardhoarder")
            .Type<StringType>()
            .Description("Cardhoarder purchase link");
    }
}
