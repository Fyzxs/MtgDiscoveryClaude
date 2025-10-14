using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Cards;

internal sealed class HasValidSearchTermArgEntityValidator : OperationResponseValidator<ICardSearchTermArgEntity, ICardNameSearchCollectionOufEntity>
{
    public HasValidSearchTermArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICardSearchTermArgEntity>
    {
        public Task<bool> IsValid(ICardSearchTermArgEntity arg) => Task.FromResult(arg.SearchTerm.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Search term cannot be empty";
    }
}
