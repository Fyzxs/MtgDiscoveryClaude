using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.Users;

internal sealed class HasValidSourceIdAuthUserArgEntityValidator : OperationResponseValidator<IAuthUserArgEntity, IUserRegistrationItrEntity>
{
    public HasValidSourceIdAuthUserArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAuthUserArgEntity>
    {
        public Task<bool> IsValid(IAuthUserArgEntity arg) => Task.FromResult(arg.SourceId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Source ID cannot be empty";
    }
}
