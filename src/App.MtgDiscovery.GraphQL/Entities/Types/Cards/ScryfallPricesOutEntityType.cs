using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Cards;

internal sealed class ScryfallPricesOutEntityType : ObjectType<PricesOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<PricesOutEntity> descriptor)
    {
        descriptor.Name("Prices")
            .Description("Price information in various currencies");

        descriptor.Field(f => f.Usd)
            .Name("usd")
            .Type<StringType>()
            .Description("USD price");
        descriptor.Field(f => f.UsdFoil)
            .Name("usdFoil")
            .Type<StringType>()
            .Description("USD foil price");
        descriptor.Field(f => f.UsdEtched)
            .Name("usdEtched")
            .Type<StringType>()
            .Description("USD etched price");
        descriptor.Field(f => f.Eur)
            .Name("eur")
            .Type<StringType>()
            .Description("EUR price");
        descriptor.Field(f => f.EurFoil)
            .Name("eurFoil")
            .Type<StringType>()
            .Description("EUR foil price");
        descriptor.Field(f => f.Tix)
            .Name("tix")
            .Type<StringType>()
            .Description("MTGO Tix price");
    }
}
