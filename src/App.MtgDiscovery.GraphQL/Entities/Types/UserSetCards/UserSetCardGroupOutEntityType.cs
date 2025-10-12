using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.UserSetCards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.UserSetCards;

public sealed class UserSetCardGroupOutEntityType : ObjectType<UserSetCardGroupOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<UserSetCardGroupOutEntity> descriptor)
    {
        descriptor.Name("UserSetCardGroup")
            .Description("A group of cards organized by finish type");

        descriptor.Field(f => f.NonFoil)
            .Name("nonFoil")
            .Type<NonNullType<UserSetCardFinishGroupOutEntityType>>()
            .Description("Non-foil cards in this group");
        descriptor.Field(f => f.Foil)
            .Name("foil")
            .Type<NonNullType<UserSetCardFinishGroupOutEntityType>>()
            .Description("Foil cards in this group");
        descriptor.Field(f => f.Etched)
            .Name("etched")
            .Type<NonNullType<UserSetCardFinishGroupOutEntityType>>()
            .Description("Etched foil cards in this group");
    }
}
