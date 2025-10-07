using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.Sets;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Sets;

internal sealed class ScryfallSetOutEntityType : ObjectType<ScryfallSetOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<ScryfallSetOutEntity> descriptor)
    {
        descriptor.Name("Set")
            .Description("Represents a Magic: The Gathering set from Scryfall");

        descriptor.Field(f => f.Id)
            .Name("id")
            .Type<NonNullType<StringType>>()
            .Description("The unique Scryfall identifier of the set");
        descriptor.Field(f => f.Code)
            .Name("code")
            .Type<NonNullType<StringType>>()
            .Description("The three to five-letter set code");
        descriptor.Field(f => f.TcgPlayerId)
            .Name("tcgPlayerId")
            .Type<IntType>()
            .Description("The TCGPlayer group ID for this set");
        descriptor.Field(f => f.Name)
            .Name("name")
            .Type<NonNullType<StringType>>()
            .Description("The English name of the set");
        descriptor.Field(f => f.Uri)
            .Name("uri")
            .Type<NonNullType<StringType>>()
            .Description("The Scryfall API URI for this set");
        descriptor.Field(f => f.ScryfallUri)
            .Name("scryfallUri")
            .Type<NonNullType<StringType>>()
            .Description("The Scryfall web page for this set");
        descriptor.Field(f => f.SearchUri)
            .Name("searchUri")
            .Type<NonNullType<StringType>>()
            .Description("A Scryfall API URI that you can request to search for cards in this set");
        descriptor.Field(f => f.ReleasedAt)
            .Name("releasedAt")
            .Type<StringType>()
            .Description("The date the set was released or will be released");
        descriptor.Field(f => f.SetType)
            .Name("setType")
            .Type<NonNullType<StringType>>()
            .Description("The type of set (core, expansion, masters, etc.)");
        descriptor.Field(f => f.CardCount)
            .Name("cardCount")
            .Type<IntType>()
            .Description("The number of cards in this set");
        descriptor.Field(f => f.Digital)
            .Name("digital")
            .Type<BooleanType>()
            .Description("Whether this set was only released in a digital format");
        descriptor.Field(f => f.NonFoilOnly)
            .Name("nonFoilOnly")
            .Type<BooleanType>()
            .Description("Whether this set contains only nonfoil cards");
        descriptor.Field(f => f.FoilOnly)
            .Name("foilOnly")
            .Type<BooleanType>()
            .Description("Whether this set contains only foil cards");
        descriptor.Field(f => f.BlockCode)
            .Name("blockCode")
            .Type<StringType>()
            .Description("The block code for this set, if applicable");
        descriptor.Field(f => f.Block)
            .Name("block")
            .Type<StringType>()
            .Description("The block name for this set, if applicable");
        descriptor.Field(f => f.IconSvgUri)
            .Name("iconSvgUri")
            .Type<NonNullType<StringType>>()
            .Description("A URI to an SVG file for this set's icon");
        descriptor.Field(f => f.Groupings)
            .Name("groupings")
            .Type<ListType<SetGroupingOutEntityType>>()
            .Description("Card groupings as displayed on the Scryfall set page");
    }
}
