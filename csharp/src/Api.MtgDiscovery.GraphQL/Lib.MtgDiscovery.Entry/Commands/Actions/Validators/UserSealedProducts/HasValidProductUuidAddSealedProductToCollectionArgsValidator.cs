using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Validators.UserSealedProducts;

internal sealed class HasValidProductUuidAddSealedProductToCollectionArgsValidator
    : OperationResponseValidator<IAddSealedProductToCollectionArgsEntity, List<SealedProductOutEntity>>
{
    public HasValidProductUuidAddSealedProductToCollectionArgsValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddSealedProductToCollectionArgsEntity>
    {
        public Task<bool> IsValid(IAddSealedProductToCollectionArgsEntity arg) => Task.FromResult(arg.AddUserSealedProduct is not null && arg.AddUserSealedProduct.ProductUuid.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Product UUID cannot be empty";
    }
}
