using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.Users;

internal sealed class IsNotNullAuthUserArgEntityValidator : OperationResponseValidator<IAuthUserArgEntity, IUserRegistrationItrEntity>
{
    public IsNotNullAuthUserArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAuthUserArgEntity>
    {
        public Task<bool> IsValid(IAuthUserArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "User authentication argument cannot be null";
    }
}
