using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using App.MtgDiscovery.GraphQL.Entities.Types.SealedProducts;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public class SealedProductsSuccessDataResponseModelType : ObjectType<SuccessDataResponseModel<List<SealedProductOutEntity>>>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<List<SealedProductOutEntity>>> descriptor)
    {
        descriptor.Name("SealedProductsSuccessResponse")
            .Description("Response returned when sealed products are successfully retrieved");

        descriptor.Field(f => f.Data)
            .Name("data")
            .Type<ListType<SealedProductType>>()
            .Description("The list of sealed products retrieved");
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
