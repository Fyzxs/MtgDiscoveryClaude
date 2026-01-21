using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Cards;

internal sealed class HasValidSetCodeArgEntityValidator : OperationResponseValidator<ISetCodeArgEntity, ICardItemCollectionOufEntity>
{
    public HasValidSetCodeArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ISetCodeArgEntity>
    {
        public Task<bool> IsValid(ISetCodeArgEntity arg) => Task.FromResult(arg.SetCode.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Set code cannot be empty";
    }
}
