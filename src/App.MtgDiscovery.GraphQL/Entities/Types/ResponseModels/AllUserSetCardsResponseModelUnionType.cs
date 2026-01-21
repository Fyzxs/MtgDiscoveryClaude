using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class AllUserSetCardsResponseModelUnionType : UnionType<ResponseModel>
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        _ = descriptor.Type<AllUserSetCardsSuccessDataResponseModelType>();
        _ = descriptor.Type<FailureResponseModelType>();
    }
}
