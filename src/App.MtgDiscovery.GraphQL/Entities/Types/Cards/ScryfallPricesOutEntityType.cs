using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.Cards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Cards;

internal sealed class ScryfallPricesOutEntityType : ObjectType<PricesOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<PricesOutEntity> descriptor)
    {
        descriptor.Name("Prices");
        descriptor.Description("Price information in various currencies");

        descriptor.Field(f => f.Usd).Description("USD price");
        descriptor.Field(f => f.UsdFoil).Description("USD foil price");
        descriptor.Field(f => f.UsdEtched).Description("USD etched price");
        descriptor.Field(f => f.Eur).Description("EUR price");
        descriptor.Field(f => f.EurFoil).Description("EUR foil price");
        descriptor.Field(f => f.Tix).Description("MTGO Tix price");
    }
}
