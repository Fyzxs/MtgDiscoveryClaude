using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Validators.UserSealedProducts;

internal sealed class HasValidProductUuidAddUserSealedProductArgValidator : OperationResponseValidator<IAddUserSealedProductArgEntity, IUserSealedProductOufEntity>
{
    public HasValidProductUuidAddUserSealedProductArgValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddUserSealedProductArgEntity>
    {
        public Task<bool> IsValid(IAddUserSealedProductArgEntity arg) => Task.FromResult(arg.ProductUuid.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Product UUID cannot be empty";
    }
}
