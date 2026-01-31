using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.Collections;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.Collections;

internal sealed class RevokeCollectionAccessArgEntityInputType : InputObjectType<RevokeCollectionAccessArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<RevokeCollectionAccessArgEntity> descriptor)
    {
        _ = descriptor.Name("RevokeCollectionAccessInput")
            .Description("Input for revoking collection access from a user");

        _ = descriptor.Field(x => x.CollectionId)
            .Name("collectionId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the collection");
        _ = descriptor.Field(x => x.TargetUserId)
            .Name("targetUserId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the user to revoke access from");
    }
}
