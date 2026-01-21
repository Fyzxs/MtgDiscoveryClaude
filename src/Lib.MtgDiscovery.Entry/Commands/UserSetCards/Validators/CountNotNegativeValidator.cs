using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards.Validators;

internal sealed class CountNotNegativeValidator : OperationResponseValidator<IAddSetGroupToUserSetCardArgsEntity, IUserSetCardOufEntity>
{
    public CountNotNegativeValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddSetGroupToUserSetCardArgsEntity>
    {
        public Task<bool> IsValid(IAddSetGroupToUserSetCardArgsEntity arg)
        {
            if (arg.AddSetGroupToUserSetCard is null) return Task.FromResult(false);

            if (arg.AddSetGroupToUserSetCard.Counts is null) return Task.FromResult(false);

            bool allCountsValid = 0 <= arg.AddSetGroupToUserSetCard.Counts.Total
                                  && 0 <= arg.AddSetGroupToUserSetCard.Counts.NonFoil
                                  && 0 <= arg.AddSetGroupToUserSetCard.Counts.Foil
                                  && 0 <= arg.AddSetGroupToUserSetCard.Counts.Etched;

            return Task.FromResult(allCountsValid);
        }
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Finish counts cannot be negative";
    }
}
