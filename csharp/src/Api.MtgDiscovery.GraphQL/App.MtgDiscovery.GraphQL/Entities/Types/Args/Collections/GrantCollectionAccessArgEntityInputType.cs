using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.Collections;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.Collections;

internal sealed class GrantCollectionAccessArgEntityInputType : InputObjectType<GrantCollectionAccessArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<GrantCollectionAccessArgEntity> descriptor)
    {
        _ = descriptor.Name("GrantCollectionAccessInput")
            .Description("Input for granting collection access to a user");

        _ = descriptor.Field(x => x.CollectionId)
            .Name("collectionId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the collection");
        _ = descriptor.Field(x => x.TargetUserId)
            .Name("targetUserId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the user to grant access to");
        _ = descriptor.Field(x => x.Role)
            .Name("role")
            .Type<NonNullType<StringType>>()
            .Description("The role to assign (viewer/editor/owner)");
    }
}
