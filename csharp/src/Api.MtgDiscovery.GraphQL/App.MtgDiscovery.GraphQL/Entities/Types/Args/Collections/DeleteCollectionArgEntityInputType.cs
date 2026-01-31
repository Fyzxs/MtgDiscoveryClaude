using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.Collections;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.Collections;

internal sealed class DeleteCollectionArgEntityInputType : InputObjectType<DeleteCollectionArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<DeleteCollectionArgEntity> descriptor)
    {
        _ = descriptor.Name("DeleteCollectionInput")
            .Description("Input for deleting a collection");

        _ = descriptor.Field(x => x.CollectionId)
            .Name("collectionId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the collection to delete");
    }
}
