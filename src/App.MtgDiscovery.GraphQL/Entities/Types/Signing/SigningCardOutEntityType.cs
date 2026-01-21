using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Types.UserCards;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.Signing;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Signing;

public sealed class SigningCardOutEntityType : ObjectType<SigningCardOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SigningCardOutEntity> descriptor)
    {
        descriptor.Name("SigningCard")
            .Description("A card to be signed");

        descriptor.Field(f => f.CardId)
            .Name("cardId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier for the card");
        descriptor.Field(f => f.CardName)
            .Name("cardName")
            .Type<NonNullType<StringType>>()
            .Description("The name of the card");
        descriptor.Field(f => f.ImageUri)
            .Name("imageUri")
            .Type<NonNullType<StringType>>()
            .Description("The URI for the card image");
        descriptor.Field(f => f.UnsignedCopies)
            .Name("unsignedCopies")
            .Type<NonNullType<IntType>>()
            .Description("Number of unsigned copies");
        descriptor.Field(f => f.IsPartiallySigned)
            .Name("isPartiallySigned")
            .Type<NonNullType<BooleanType>>()
            .Description("True if the card has both signed and unsigned copies");
        descriptor.Field(f => f.IsFullySigned)
            .Name("isFullySigned")
            .Type<NonNullType<BooleanType>>()
            .Description("True if all copies of this card are signed");
        descriptor.Field(f => f.CollectedDetails)
            .Name("collectedDetails")
            .Type<NonNullType<ListType<NonNullType<CollectedItemOutEntityType>>>>()
            .Description("Details of collected variants");
    }
}
