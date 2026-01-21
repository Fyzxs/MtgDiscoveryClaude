using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public class CardResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("CardsByIdResponse");
        descriptor.Description("Union type for different response types from CardsById query");
        descriptor.Type<CardsSuccessDataResponseModelType>();
        descriptor.Type<FailureResponseModelType>();
    }
}
