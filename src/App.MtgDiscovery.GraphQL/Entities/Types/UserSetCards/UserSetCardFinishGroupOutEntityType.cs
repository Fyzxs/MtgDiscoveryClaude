using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.UserSetCards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.UserSetCards;

public sealed class UserSetCardFinishGroupOutEntityType : ObjectType<UserSetCardFinishGroupOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<UserSetCardFinishGroupOutEntity> descriptor)
    {
        descriptor.Name("UserSetCardFinishGroup")
            .Description("A collection of card IDs for a specific finish type");

        descriptor.Field(f => f.Cards)
            .Type<NonNullType<ListType<NonNullType<StringType>>>>()
            .Description("List of card IDs in this finish group");
    }
}