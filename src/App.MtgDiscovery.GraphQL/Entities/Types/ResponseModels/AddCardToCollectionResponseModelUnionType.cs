using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public class AddCardToCollectionResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("AddCardToCollectionResponse")
            .Description("Union type for different response types from AddCardToCollection mutation")
            .Type<CardsSuccessDataResponseModelType>()
            .Type<FailureResponseModelType>();
    }
}
