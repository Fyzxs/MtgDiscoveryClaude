using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Types.Artists;
using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.Artists;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class ArtistSearchResultsSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<List<ArtistSearchResultOutEntity>>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<List<ArtistSearchResultOutEntity>>> descriptor)
    {
        descriptor.Name("SuccessArtistSearchResultsResponse");
        descriptor.Description("Response returned when artist search is successful");

        descriptor.Field(f => f.Data)
            .Type<NonNullType<ListType<NonNullType<ArtistSearchResultOutEntityType>>>>()
            .Description("The list of artist search results");

        descriptor.Field(f => f.Status)
            .Type<StatusDataModelType>()
            .Description("Status information about the success");

        descriptor.Field(f => f.MetaData)
            .Type<MetaDataModelType>()
            .Description("Metadata about the response");
    }
}
