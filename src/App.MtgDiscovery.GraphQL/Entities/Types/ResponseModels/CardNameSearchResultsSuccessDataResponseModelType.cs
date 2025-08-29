using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Outs.Cards;
using HotChocolate.Types;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class CardNameSearchResultsSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<List<CardNameSearchResultOutEntity>>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<List<CardNameSearchResultOutEntity>>> descriptor)
    {
        descriptor.Name("SuccessCardNameSearchResultsResponse");
        descriptor.Field(f => f.Data).Type<NonNullType<ListType<NonNullType<ObjectType<CardNameSearchResultOutEntity>>>>>();
    }
}