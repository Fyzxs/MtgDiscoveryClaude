using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class CardsByArtistResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("CardsByArtistResponseModel");
        descriptor.Description("Union type for cards by artist response");
        descriptor.Type<CardsByArtistSuccessDataResponseModelType>();
        descriptor.Type<FailureResponseModelType>();
    }
}
