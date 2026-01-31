using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.Collections;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.Collections;

internal sealed class CreateCollectionArgEntityInputType : InputObjectType<CreateCollectionArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<CreateCollectionArgEntity> descriptor)
    {
        _ = descriptor.Name("CreateCollectionInput")
            .Description("Input for creating a new collection");

        _ = descriptor.Field(x => x.Name)
            .Name("name")
            .Type<NonNullType<StringType>>()
            .Description("The name of the collection");
        _ = descriptor.Field(x => x.Type)
            .Name("type")
            .Type<NonNullType<StringType>>()
            .Description("The type of the collection");
        _ = descriptor.Field(x => x.Visibility)
            .Name("visibility")
            .Type<NonNullType<StringType>>()
            .Description("The visibility of the collection (public/private)");
    }
}
