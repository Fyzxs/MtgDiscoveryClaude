using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Cards;

internal sealed class ScryfallRelatedUrisOutEntityType : ObjectType<RelatedUrisOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<RelatedUrisOutEntity> descriptor)
    {
        descriptor.Name("RelatedUris")
            .Description("Related URIs for additional information");

        descriptor.Field(f => f.Gatherer)
            .Name("gatherer")
            .Type<StringType>()
            .Description("Gatherer page");
        descriptor.Field(f => f.TcgplayerInfiniteArticles)
            .Name("tcgplayerInfiniteArticles")
            .Type<StringType>()
            .Description("TCGPlayer articles");
        descriptor.Field(f => f.TcgplayerInfiniteDecks)
            .Name("tcgplayerInfiniteDecks")
            .Type<StringType>()
            .Description("TCGPlayer decks");
        descriptor.Field(f => f.Edhrec)
            .Name("edhrec")
            .Type<StringType>()
            .Description("EDHREC page");
    }
}
