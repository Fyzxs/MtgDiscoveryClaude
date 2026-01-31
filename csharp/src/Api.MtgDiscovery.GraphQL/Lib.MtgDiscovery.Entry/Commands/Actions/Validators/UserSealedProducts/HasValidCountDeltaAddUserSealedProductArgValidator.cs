using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Validators.UserSealedProducts;

internal sealed class HasValidCountDeltaAddUserSealedProductArgValidator : OperationResponseValidator<IAddUserSealedProductArgEntity, IUserSealedProductOufEntity>
{
    public HasValidCountDeltaAddUserSealedProductArgValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddUserSealedProductArgEntity>
    {
        public Task<bool> IsValid(IAddUserSealedProductArgEntity arg) => Task.FromResult(arg.UserSealedProductDetails.Count is not 0);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Count delta cannot be zero";
    }
}
