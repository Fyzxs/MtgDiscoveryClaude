using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Cards;

internal sealed class ScryfallLegalitiesOutEntityType : ObjectType<LegalitiesOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<LegalitiesOutEntity> descriptor)
    {
        descriptor.Name("Legalities")
            .Description("Format legalities for the card");

        descriptor.Field(f => f.Standard)
            .Name("standard")
            .Type<StringType>()
            .Description("Standard legality");
        descriptor.Field(f => f.Future)
            .Name("future")
            .Type<StringType>()
            .Description("Future Standard legality");
        descriptor.Field(f => f.Historic)
            .Name("historic")
            .Type<StringType>()
            .Description("Historic legality");
        descriptor.Field(f => f.Timeless)
            .Name("timeless")
            .Type<StringType>()
            .Description("Timeless legality");
        descriptor.Field(f => f.Gladiator)
            .Name("gladiator")
            .Type<StringType>()
            .Description("Gladiator legality");
        descriptor.Field(f => f.Pioneer)
            .Name("pioneer")
            .Type<StringType>()
            .Description("Pioneer legality");
        descriptor.Field(f => f.Explorer)
            .Name("explorer")
            .Type<StringType>()
            .Description("Explorer legality");
        descriptor.Field(f => f.Modern)
            .Name("modern")
            .Type<StringType>()
            .Description("Modern legality");
        descriptor.Field(f => f.Legacy)
            .Name("legacy")
            .Type<StringType>()
            .Description("Legacy legality");
        descriptor.Field(f => f.Pauper)
            .Name("pauper")
            .Type<StringType>()
            .Description("Pauper legality");
        descriptor.Field(f => f.Vintage)
            .Name("vintage")
            .Type<StringType>()
            .Description("Vintage legality");
        descriptor.Field(f => f.Penny)
            .Name("penny")
            .Type<StringType>()
            .Description("Penny Dreadful legality");
        descriptor.Field(f => f.Commander)
            .Name("commander")
            .Type<StringType>()
            .Description("Commander legality");
        descriptor.Field(f => f.Oathbreaker)
            .Name("oathbreaker")
            .Type<StringType>()
            .Description("Oathbreaker legality");
        descriptor.Field(f => f.StandardBrawl)
            .Name("standardBrawl")
            .Type<StringType>()
            .Description("Standard Brawl legality");
        descriptor.Field(f => f.Brawl)
            .Name("brawl")
            .Type<StringType>()
            .Description("Brawl legality");
        descriptor.Field(f => f.Alchemy)
            .Name("alchemy")
            .Type<StringType>()
            .Description("Alchemy legality");
        descriptor.Field(f => f.PauperCommander)
            .Name("pauperCommander")
            .Type<StringType>()
            .Description("Pauper Commander legality");
        descriptor.Field(f => f.Duel)
            .Name("duel")
            .Type<StringType>()
            .Description("Duel Commander legality");
        descriptor.Field(f => f.Oldschool)
            .Name("oldschool")
            .Type<StringType>()
            .Description("Old School legality");
        descriptor.Field(f => f.Premodern)
            .Name("premodern")
            .Type<StringType>()
            .Description("Premodern legality");
        descriptor.Field(f => f.Predh)
            .Name("predh")
            .Type<StringType>()
            .Description("PreDH legality");
    }
}
