using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.UserCards;

internal sealed class HasValidSetIdUserCardsSetArgEntityValidator : OperationResponseValidator<IUserCardsBySetArgEntity, IEnumerable<IUserCardOufEntity>>
{
    public HasValidSetIdUserCardsSetArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardsBySetArgEntity>
    {
        public Task<bool> IsValid(IUserCardsBySetArgEntity arg) => Task.FromResult(arg.SetId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Set Id cannot be empty";
    }
}
