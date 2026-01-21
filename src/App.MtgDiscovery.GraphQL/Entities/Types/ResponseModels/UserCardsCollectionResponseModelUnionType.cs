using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class UserCardsCollectionResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("UserCardsCollectionResponse")
            .Description("Response for user cards collection queries")
            .Type<UserCardsCollectionSuccessDataResponseModelType>()
            .Type<FailureResponseModelType>();
    }
}
