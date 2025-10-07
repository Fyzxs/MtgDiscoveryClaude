using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class CardNameSearchResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("CardNameSearchResponse")
            .Description("Union type for card name search response")
            .Type<CardNameSearchResultsSuccessDataResponseModelType>()
            .Type<FailureResponseModelType>();
    }
}
