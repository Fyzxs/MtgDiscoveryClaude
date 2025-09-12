using App.MtgDiscovery.GraphQL.Entities.Outs.Cards;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Cards;

internal class ScryfallRelatedUrisOutEntityType : ObjectType<RelatedUrisOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<RelatedUrisOutEntity> descriptor)
    {
        descriptor.Name("RelatedUris");
        descriptor.Description("Related URIs for additional information");

        descriptor.Field(f => f.Gatherer).Description("Gatherer page");
        descriptor.Field(f => f.TcgplayerInfiniteArticles).Description("TCGPlayer articles");
        descriptor.Field(f => f.TcgplayerInfiniteDecks).Description("TCGPlayer decks");
        descriptor.Field(f => f.Edhrec).Description("EDHREC page");
    }
}
