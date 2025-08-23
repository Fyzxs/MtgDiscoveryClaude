using App.MtgDiscovery.GraphQL.Entities.Outs;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types;

internal class ScryfallLegalitiesOutEntityType : ObjectType<LegalitiesOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<LegalitiesOutEntity> descriptor)
    {
        descriptor.Name("Legalities");
        descriptor.Description("Format legalities for the card");

        descriptor.Field(f => f.Standard).Description("Standard legality");
        descriptor.Field(f => f.Future).Description("Future Standard legality");
        descriptor.Field(f => f.Historic).Description("Historic legality");
        descriptor.Field(f => f.Timeless).Description("Timeless legality");
        descriptor.Field(f => f.Gladiator).Description("Gladiator legality");
        descriptor.Field(f => f.Pioneer).Description("Pioneer legality");
        descriptor.Field(f => f.Explorer).Description("Explorer legality");
        descriptor.Field(f => f.Modern).Description("Modern legality");
        descriptor.Field(f => f.Legacy).Description("Legacy legality");
        descriptor.Field(f => f.Pauper).Description("Pauper legality");
        descriptor.Field(f => f.Vintage).Description("Vintage legality");
        descriptor.Field(f => f.Penny).Description("Penny Dreadful legality");
        descriptor.Field(f => f.Commander).Description("Commander legality");
        descriptor.Field(f => f.Oathbreaker).Description("Oathbreaker legality");
        descriptor.Field(f => f.StandardBrawl).Description("Standard Brawl legality");
        descriptor.Field(f => f.Brawl).Description("Brawl legality");
        descriptor.Field(f => f.Alchemy).Description("Alchemy legality");
        descriptor.Field(f => f.PauperCommander).Description("Pauper Commander legality");
        descriptor.Field(f => f.Duel).Description("Duel Commander legality");
        descriptor.Field(f => f.Oldschool).Description("Old School legality");
        descriptor.Field(f => f.Premodern).Description("Premodern legality");
        descriptor.Field(f => f.Predh).Description("PreDH legality");
    }
}