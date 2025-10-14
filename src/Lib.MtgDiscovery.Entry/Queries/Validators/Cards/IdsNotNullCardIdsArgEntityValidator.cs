using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Cards;

internal sealed class IdsNotNullCardIdsArgEntityValidator : OperationResponseValidator<ICardIdsArgEntity, ICardItemCollectionOufEntity>
{
    public IdsNotNullCardIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICardIdsArgEntity>
    {
        public Task<bool> IsValid(ICardIdsArgEntity arg) => Task.FromResult(arg.CardIds is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided list is null";
    }
}
