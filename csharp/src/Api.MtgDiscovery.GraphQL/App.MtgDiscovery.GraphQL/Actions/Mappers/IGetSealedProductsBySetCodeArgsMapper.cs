using App.MtgDiscovery.GraphQL.Entities.Args.SealedProducts;
using Lib.Shared.DataModels.Entities.Args.SealedProducts;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers;

internal interface IGetSealedProductsBySetCodeArgsMapper
{
    ISealedProductsBySetCodeArgEntity Map(GetSealedProductsBySetCodeArgEntity source);
}
