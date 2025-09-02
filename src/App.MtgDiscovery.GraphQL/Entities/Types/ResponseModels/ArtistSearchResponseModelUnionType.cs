using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class ArtistSearchResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("ArtistSearchResponseModel");
        descriptor.Description("Union type for artist search response");
        descriptor.Type<ArtistSearchResultsSuccessDataResponseModelType>();
        descriptor.Type<FailureResponseModelType>();
    }
}