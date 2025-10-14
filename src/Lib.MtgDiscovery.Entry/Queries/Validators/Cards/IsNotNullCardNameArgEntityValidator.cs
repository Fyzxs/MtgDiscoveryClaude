using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Cards;

internal sealed class IsNotNullCardNameArgEntityValidator : OperationResponseValidator<ICardNameArgEntity, ICardItemCollectionOufEntity>
{
    public IsNotNullCardNameArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICardNameArgEntity>
    {
        public Task<bool> IsValid(ICardNameArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Card name argument cannot be null";
    }
}
