using Lib.Shared.DataModels.Entities.Outs.Sets;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Sets;

internal sealed class ScryfallSetOutEntityType : ObjectType<ScryfallSetOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<ScryfallSetOutEntity> descriptor)
    {
        descriptor.Name("Set");
        descriptor.Description("Represents a Magic: The Gathering set from Scryfall");

        descriptor.Field(f => f.Id)
            .Type<NonNullType<StringType>>()
            .Description("The unique Scryfall identifier of the set");

        descriptor.Field(f => f.Code)
            .Type<NonNullType<StringType>>()
            .Description("The three to five-letter set code");

        descriptor.Field(f => f.TcgPlayerId)
            .Description("The TCGPlayer group ID for this set");

        descriptor.Field(f => f.Name)
            .Type<NonNullType<StringType>>()
            .Description("The English name of the set");

        descriptor.Field(f => f.Uri)
            .Type<NonNullType<StringType>>()
            .Description("The Scryfall API URI for this set");

        descriptor.Field(f => f.ScryfallUri)
            .Type<NonNullType<StringType>>()
            .Description("The Scryfall web page for this set");

        descriptor.Field(f => f.SearchUri)
            .Type<NonNullType<StringType>>()
            .Description("A Scryfall API URI that you can request to search for cards in this set");

        descriptor.Field(f => f.ReleasedAt)
            .Description("The date the set was released or will be released");

        descriptor.Field(f => f.SetType)
            .Type<NonNullType<StringType>>()
            .Description("The type of set (core, expansion, masters, etc.)");

        descriptor.Field(f => f.CardCount)
            .Description("The number of cards in this set");

        descriptor.Field(f => f.Digital)
            .Description("Whether this set was only released in a digital format");

        descriptor.Field(f => f.NonFoilOnly)
            .Description("Whether this set contains only nonfoil cards");

        descriptor.Field(f => f.FoilOnly)
            .Description("Whether this set contains only foil cards");

        descriptor.Field(f => f.BlockCode)
            .Description("The block code for this set, if applicable");

        descriptor.Field(f => f.Block)
            .Description("The block name for this set, if applicable");

        descriptor.Field(f => f.IconSvgUri)
            .Type<NonNullType<StringType>>()
            .Description("A URI to an SVG file for this set's icon");

        descriptor.Field(f => f.Groupings)
            .Type<ListType<SetGroupingOutEntityType>>()
            .Description("Card groupings as displayed on the Scryfall set page");
    }
}
