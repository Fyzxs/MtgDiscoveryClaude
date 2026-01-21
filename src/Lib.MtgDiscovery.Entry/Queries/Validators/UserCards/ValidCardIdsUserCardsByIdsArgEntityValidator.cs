using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.UserCards;

internal sealed class ValidCardIdsUserCardsByIdsArgEntityValidator : OperationResponseValidator<IUserCardsByIdsArgEntity, IEnumerable<IUserCardOufEntity>>
{
    public ValidCardIdsUserCardsByIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardsByIdsArgEntity>
    {
        public Task<bool> IsValid(IUserCardsByIdsArgEntity arg) => Task.FromResult(arg.CardIds.All(id => StringExtensions.IzNotNullOrWhiteSpace(id)));
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "All Card Ids must be valid";
    }
}