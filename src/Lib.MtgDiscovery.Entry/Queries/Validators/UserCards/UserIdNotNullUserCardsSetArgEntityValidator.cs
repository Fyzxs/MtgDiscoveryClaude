using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.UserCards;

internal sealed class UserIdNotNullUserCardsSetArgEntityValidator : OperationResponseValidator<IUserCardsBySetArgEntity, IEnumerable<IUserCardOufEntity>>
{
    public UserIdNotNullUserCardsSetArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardsBySetArgEntity>
    {
        public Task<bool> IsValid(IUserCardsBySetArgEntity arg) => Task.FromResult(arg.UserId is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "User Id cannot be null";
    }
}
