using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Cards;

/// <summary>
/// Validates search term argument is not null.
/// Standard null check following the typed validator pattern.
/// </summary>
internal sealed class IsNotNullCardSearchTermArgEntityValidator : OperationResponseValidator<ICardSearchTermArgEntity, ICardNameSearchCollectionOufEntity>
{
    public IsNotNullCardSearchTermArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICardSearchTermArgEntity>
    {
        public Task<bool> IsValid(ICardSearchTermArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Search term argument cannot be null";
    }
}