using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Types.Cards;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public class CardsSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<List<CardItemOutEntity>>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<List<CardItemOutEntity>>> descriptor)
    {
        descriptor.Name("CardsSuccessResponse")
            .Description("Response returned when cards are successfully retrieved");

        descriptor.Field(f => f.Data)
            .Name("data")
            .Type<ListType<ScryfallCardOutEntityType>>()
            .Description("The list of cards retrieved");
        descriptor.Field(f => f.Status)
            .Name("status")
            .Type<StatusDataModelType>()
            .Description("Status information about the success");
        descriptor.Field(f => f.MetaData)
            .Name("metaData")
            .Type<MetaDataModelType>()
            .Description("Metadata about the response");
    }
}
