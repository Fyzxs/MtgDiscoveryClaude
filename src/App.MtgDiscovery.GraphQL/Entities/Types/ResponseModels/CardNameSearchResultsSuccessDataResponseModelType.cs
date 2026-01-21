using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class CardNameSearchResultsSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<List<CardNameSearchResultOutEntity>>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<List<CardNameSearchResultOutEntity>>> descriptor)
    {
        descriptor.Name("CardNameSearchResultsSuccessResponse")
            .Description("Successful response for a card name search");

        descriptor.Field(f => f.Data)
            .Name("data").Type<NonNullType<ListType<NonNullType<ObjectType<CardNameSearchResultOutEntity>>>>>();
    }
}
