using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.Cards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Cards;

internal sealed class ScryfallPurchaseUrisOutEntityType : ObjectType<PurchaseUrisOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<PurchaseUrisOutEntity> descriptor)
    {
        descriptor.Name("PurchaseUris")
            .Description("URIs for purchasing the card");

        descriptor.Field(f => f.Tcgplayer).Description("TCGPlayer purchase link");
        descriptor.Field(f => f.Cardmarket).Description("Cardmarket purchase link");
        descriptor.Field(f => f.Cardhoarder).Description("Cardhoarder purchase link");
    }
}
