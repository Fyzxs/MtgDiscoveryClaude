using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Outs.Cards;
using App.MtgDiscovery.GraphQL.Entities.Types.Cards;
using HotChocolate.Types;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class CardsByArtistSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<List<ScryfallCardOutEntity>>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<List<ScryfallCardOutEntity>>> descriptor)
    {
        descriptor.Name("SuccessCardsByArtistResponse");
        descriptor.Description("Response returned when cards by artist are successfully retrieved");

        descriptor.Field(f => f.Data)
            .Type<ListType<ScryfallCardOutEntityType>>()
            .Description("The list of cards illustrated by the artist");

        descriptor.Field(f => f.Status)
            .Type<StatusDataModelType>()
            .Description("Status information about the success");

        descriptor.Field(f => f.MetaData)
            .Type<MetaDataModelType>()
            .Description("Metadata about the response");
    }
}