using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.Collections;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.Collections;

internal sealed class RenameCollectionArgEntityInputType : InputObjectType<RenameCollectionArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<RenameCollectionArgEntity> descriptor)
    {
        _ = descriptor.Name("RenameCollectionInput")
            .Description("Input for renaming a collection");

        _ = descriptor.Field(x => x.CollectionId)
            .Name("collectionId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the collection");
        _ = descriptor.Field(x => x.Name)
            .Name("name")
            .Type<NonNullType<StringType>>()
            .Description("The new name for the collection");
    }
}
