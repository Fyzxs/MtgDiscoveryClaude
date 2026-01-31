using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.UserCards;

internal sealed class UserIdNotNullUserCardArgEntityValidator : OperationResponseValidator<IUserCardArgEntity, IEnumerable<IUserCardOufEntity>>
{
    public UserIdNotNullUserCardArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardArgEntity>
    {
        public Task<bool> IsValid(IUserCardArgEntity arg) => Task.FromResult(arg.UserId is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "User Id cannot be null";
    }
}
