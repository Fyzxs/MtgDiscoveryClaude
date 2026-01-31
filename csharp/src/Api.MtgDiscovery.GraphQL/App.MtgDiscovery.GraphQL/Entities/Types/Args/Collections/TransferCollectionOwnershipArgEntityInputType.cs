using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Args.Collections;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Args.Collections;

internal sealed class TransferCollectionOwnershipArgEntityInputType : InputObjectType<TransferCollectionOwnershipArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<TransferCollectionOwnershipArgEntity> descriptor)
    {
        _ = descriptor.Name("TransferCollectionOwnershipInput")
            .Description("Input for transferring collection ownership");

        _ = descriptor.Field(x => x.CollectionId)
            .Name("collectionId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the collection");
        _ = descriptor.Field(x => x.TargetUserId)
            .Name("targetUserId")
            .Type<NonNullType<StringType>>()
            .Description("The unique identifier of the user to transfer ownership to");
    }
}
