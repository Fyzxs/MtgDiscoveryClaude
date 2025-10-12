using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class CardsByArtistResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("CardsByArtistResponse")
            .Description("Union type for cards by artist response")
            .Type<CardsByArtistSuccessDataResponseModelType>()
            .Type<FailureResponseModelType>();
    }
}
