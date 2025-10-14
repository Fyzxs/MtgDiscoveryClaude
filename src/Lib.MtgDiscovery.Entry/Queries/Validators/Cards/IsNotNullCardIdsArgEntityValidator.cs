using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Cards;

/// <summary>
/// Validates that the CardIds argument entity is not null.
/// 
/// Structure Pattern: Validator + Nested Types
///   - Validator: Typed behavior (not Func) for OOP and testability
///   - Message: Typed message (not string) following No Primitives
///   - Both are immutable and never change after creation
/// 
/// This class should NEVER change. If different validation is needed, create a new class.
/// </summary>
internal sealed class IsNotNullCardIdsArgEntityValidator : OperationResponseValidator<ICardIdsArgEntity, ICardItemCollectionOufEntity>
{
    public IsNotNullCardIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICardIdsArgEntity>
    {
        public Task<bool> IsValid(ICardIdsArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided object is null";
    }
}
