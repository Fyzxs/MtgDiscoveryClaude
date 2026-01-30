using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.Collections;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Collections;

public sealed class CollectionOutEntityType : ObjectType<CollectionOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<CollectionOutEntity> descriptor)
    {
        descriptor.Name("Collection")
            .Description("A named container for card data");

        descriptor.Field(f => f.CollectionId)
            .Name("collectionId")
            .Type<NonNullType<StringType>>();
        descriptor.Field(f => f.OwnerId)
            .Name("ownerId")
            .Type<NonNullType<StringType>>();
        descriptor.Field(f => f.Name)
            .Name("name")
            .Type<NonNullType<StringType>>();
        descriptor.Field(f => f.Type)
            .Name("type")
            .Type<NonNullType<StringType>>();
        descriptor.Field(f => f.Visibility)
            .Name("visibility")
            .Type<NonNullType<StringType>>();
        descriptor.Field(f => f.IsDefault)
            .Name("isDefault")
            .Type<NonNullType<BooleanType>>();
        descriptor.Field(f => f.AuthorizedUsers)
            .Name("authorizedUsers")
            .Type<NonNullType<ListType<NonNullType<AuthorizedUserOutEntityType>>>>();
        descriptor.Field(f => f.CreatedAt)
            .Name("createdAt")
            .Type<NonNullType<StringType>>();
        descriptor.Field(f => f.UpdatedAt)
            .Name("updatedAt")
            .Type<NonNullType<StringType>>();
    }
}
