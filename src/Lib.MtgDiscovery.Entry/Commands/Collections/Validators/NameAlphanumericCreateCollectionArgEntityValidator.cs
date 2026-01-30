using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators;

internal sealed partial class NameAlphanumericCreateCollectionArgEntityValidator : OperationResponseValidator<ICreateCollectionArgEntity, ICollectionItrEntity>
{
    public NameAlphanumericCreateCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICreateCollectionArgEntity>
    {
        private static readonly Regex s_allowedPattern = AllowedCharsRegex();

        public Task<bool> IsValid(ICreateCollectionArgEntity arg) =>
            Task.FromResult(s_allowedPattern.IsMatch(arg.Name));
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Collection name can only contain letters, numbers, spaces, and dashes";
    }

    [GeneratedRegex("^[a-zA-Z0-9 -]+$")]
    private static partial Regex AllowedCharsRegex();
}
