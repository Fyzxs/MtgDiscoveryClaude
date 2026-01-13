using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Validators.UserSealedProducts;

internal sealed class HasValidSetIdAddUserSealedProductArgValidator : OperationResponseValidator<IAddUserSealedProductArgEntity, IUserSealedProductOufEntity>
{
    public HasValidSetIdAddUserSealedProductArgValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddUserSealedProductArgEntity>
    {
        public Task<bool> IsValid(IAddUserSealedProductArgEntity arg) => Task.FromResult(arg.SetId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Set ID cannot be empty";
    }
}
