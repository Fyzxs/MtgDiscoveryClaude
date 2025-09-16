using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.UserCards;

public sealed class UserCardCollectionResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("UserCardCollectionResponseModel");
        descriptor.Description("Union type for user card collection response");
        descriptor.Type<UserCardCollectionSuccessDataResponseModelType>();
        descriptor.Type<FailureResponseModelType>();
    }
}
