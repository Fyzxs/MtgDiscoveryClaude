using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class CardNameSearchResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("CardNameSearchResponseModel");
        descriptor.Type<CardNameSearchResultsSuccessDataResponseModelType>();
        descriptor.Type<FailureResponseModelType>();
    }
}
