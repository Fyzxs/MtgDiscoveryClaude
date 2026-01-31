using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.Collections;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.Collections;

internal sealed class UpdateCollectionVisibilityArgEntityInputType : InputObjectType<UpdateCollectionVisibilityArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<UpdateCollectionVisibilityArgEntity> descriptor)
    {
        _ = descriptor.Name("UpdateCollectionVisibilityInput")
            .Description("Input for updating collection visibility");

        _ = descriptor.Field(x => x.CollectionId)
            .Name("collectionId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the collection");
        _ = descriptor.Field(x => x.Visibility)
            .Name("visibility")
            .Type<NonNullType<StringType>>()
            .Description("The new visibility level (public/private)");
    }
}
