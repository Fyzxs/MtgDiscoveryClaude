using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Actions.Mappers;
using App.MtgDiscovery.GraphQL.Authentication;
using App.MtgDiscovery.GraphQL.Entities.Args.SealedProducts;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
public sealed class SealedProductsQueryMethods
{
    private readonly IEntryService _entryService;
    private readonly IOperationResponseToResponseModelMapper<List<SealedProductOutEntity>> _responseMapper;

    public SealedProductsQueryMethods(ILogger logger) : this(
        new EntryService(logger),
        new OperationResponseToResponseModelMapper<List<SealedProductOutEntity>>())
    {
    }

    private SealedProductsQueryMethods(
        IEntryService entryService,
        IOperationResponseToResponseModelMapper<List<SealedProductOutEntity>> responseMapper)
    {
        _entryService = entryService;
        _responseMapper = responseMapper;
    }

    [GraphQLType(typeof(SealedProductsResponseModelUnionType))]
    public async Task<ResponseModel> SealedProductsBySetCode(
        GetSealedProductsBySetCodeArgEntity args,
        [Service] ClaimsPrincipal claimsPrincipal = null)
    {
        IOperationResponse<List<SealedProductOutEntity>> productsResponse = await _entryService
            .SealedProductsBySetCodeAsync(args)
            .ConfigureAwait(false);

        if (productsResponse.IsSuccess is false)
        {
            return await _responseMapper.Map(productsResponse).ConfigureAwait(false);
        }

        List<SealedProductOutEntity> enrichedProducts;

        if (claimsPrincipal?.Identity?.IsAuthenticated == true)
        {
            AuthUserArgEntity authUser = new(claimsPrincipal);
            string userId = authUser.UserId;

            IOperationResponse<List<UserSealedProductOutEntity>> userProductsResponse =
                await _entryService.GetUserSealedProductsByUserIdAsync(userId).ConfigureAwait(false);

            if (userProductsResponse.IsSuccess)
            {
                Dictionary<string, int> userQuantities = userProductsResponse.ResponseData
                    .ToDictionary(up => up.ProductUuid, up => up.Count);

                enrichedProducts = productsResponse.ResponseData.Select(product => new SealedProductOutEntity
                {
                    Uuid = product.Uuid,
                    SetId = product.SetId,
                    SetCode = product.SetCode,
                    SetName = product.SetName,
                    Name = product.Name,
                    Category = product.Category,
                    Subtype = product.Subtype,
                    CardCount = product.CardCount,
                    ReleaseDate = product.ReleaseDate,
                    TcgplayerProductId = product.TcgplayerProductId,
                    ImageUrl = product.ImageUrl,
                    PurchaseUrlTcgplayer = product.PurchaseUrlTcgplayer,
                    PurchaseUrlCardmarket = product.PurchaseUrlCardmarket,
                    PurchaseUrlCardKingdom = product.PurchaseUrlCardKingdom,
                    UserQuantity = userQuantities.TryGetValue(product.Uuid, out int quantity) ? quantity : 0
                }).ToList();
            }
            else
            {
                enrichedProducts = productsResponse.ResponseData.Select(product => new SealedProductOutEntity
                {
                    Uuid = product.Uuid,
                    SetId = product.SetId,
                    SetCode = product.SetCode,
                    SetName = product.SetName,
                    Name = product.Name,
                    Category = product.Category,
                    Subtype = product.Subtype,
                    CardCount = product.CardCount,
                    ReleaseDate = product.ReleaseDate,
                    TcgplayerProductId = product.TcgplayerProductId,
                    ImageUrl = product.ImageUrl,
                    PurchaseUrlTcgplayer = product.PurchaseUrlTcgplayer,
                    PurchaseUrlCardmarket = product.PurchaseUrlCardmarket,
                    PurchaseUrlCardKingdom = product.PurchaseUrlCardKingdom,
                    UserQuantity = 0
                }).ToList();
            }
        }
        else
        {
            enrichedProducts = productsResponse.ResponseData.Select(product => new SealedProductOutEntity
            {
                Uuid = product.Uuid,
                SetId = product.SetId,
                SetCode = product.SetCode,
                SetName = product.SetName,
                Name = product.Name,
                Category = product.Category,
                Subtype = product.Subtype,
                CardCount = product.CardCount,
                ReleaseDate = product.ReleaseDate,
                TcgplayerProductId = product.TcgplayerProductId,
                ImageUrl = product.ImageUrl,
                PurchaseUrlTcgplayer = product.PurchaseUrlTcgplayer,
                PurchaseUrlCardmarket = product.PurchaseUrlCardmarket,
                PurchaseUrlCardKingdom = product.PurchaseUrlCardKingdom,
                UserQuantity = 0
            }).ToList();
        }

        IOperationResponse<List<SealedProductOutEntity>> enrichedResponse =
            new SuccessOperationResponse<List<SealedProductOutEntity>>(enrichedProducts);

        return await _responseMapper.Map(enrichedResponse).ConfigureAwait(false);
    }
}
