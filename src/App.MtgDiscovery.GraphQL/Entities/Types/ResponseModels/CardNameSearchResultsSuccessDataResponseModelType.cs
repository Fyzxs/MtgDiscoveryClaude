using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.Shared.DataModels.Entities.Outs.Cards;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class CardNameSearchResultsSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<List<CardNameSearchResultOutEntity>>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<List<CardNameSearchResultOutEntity>>> descriptor)
    {
        descriptor.Name("CardNameSearchResultsSuccessResponse");
        descriptor.Field(f => f.Data).Type<NonNullType<ListType<NonNullType<ObjectType<CardNameSearchResultOutEntity>>>>>();
    }
}
