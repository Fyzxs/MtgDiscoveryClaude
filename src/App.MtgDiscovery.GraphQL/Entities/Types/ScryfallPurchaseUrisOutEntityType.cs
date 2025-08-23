using App.MtgDiscovery.GraphQL.Entities.Outs;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types;

internal class ScryfallPurchaseUrisOutEntityType : ObjectType<PurchaseUrisOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<PurchaseUrisOutEntity> descriptor)
    {
        descriptor.Name("PurchaseUris");
        descriptor.Description("URIs for purchasing the card");

        descriptor.Field(f => f.Tcgplayer).Description("TCGPlayer purchase link");
        descriptor.Field(f => f.Cardmarket).Description("Cardmarket purchase link");
        descriptor.Field(f => f.Cardhoarder).Description("Cardhoarder purchase link");
    }
}