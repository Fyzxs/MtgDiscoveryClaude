using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.UserSetCards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.UserSetCards;

public sealed class UserSetCardOutEntityType : ObjectType<UserSetCardOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<UserSetCardOutEntity> descriptor)
    {
        descriptor.Name("UserSetCard")
            .Description("A user's collection summary for a specific set");

        descriptor.Field(f => f.UserId)
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier for the user");

        descriptor.Field(f => f.SetId)
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier for the set");

        descriptor.Field(f => f.TotalCards)
            .Type<NonNullType<IntType>>()
            .Description("The total number of cards in this set");

        descriptor.Field(f => f.UniqueCards)
            .Type<NonNullType<IntType>>()
            .Description("The number of unique cards the user has from this set");

        descriptor.Field(f => f.Groups)
            .Type<NonNullType<ListType<NonNullType<UserSetCardGroupKeyValueType>>>>()
            .Description("Card collection groups organized by rarity")
            .Resolve(context =>
            {
                UserSetCardOutEntity parent = context.Parent<UserSetCardOutEntity>();
                if (parent.Groups == null) { return []; }

                return parent.Groups.Select(kvp => new UserSetCardGroupKeyValue { Key = kvp.Key, Value = kvp.Value }).ToList();
            });
    }
}
