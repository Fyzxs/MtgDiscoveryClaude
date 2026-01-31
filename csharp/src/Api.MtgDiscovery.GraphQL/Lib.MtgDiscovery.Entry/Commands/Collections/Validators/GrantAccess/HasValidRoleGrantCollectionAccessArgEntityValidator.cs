using System;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.GrantAccess;

internal sealed class HasValidRoleGrantCollectionAccessArgEntityValidator : OperationResponseValidator<IGrantCollectionAccessArgEntity, IGrantCollectionAccessItrEntity>
{
    private static readonly string[] s_validRoleValues = ["editor", "viewer"];

    public HasValidRoleGrantCollectionAccessArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IGrantCollectionAccessArgEntity>
    {
        public Task<bool> IsValid(IGrantCollectionAccessArgEntity arg)
        {
            if (arg.Role.IzNullOrWhiteSpace())
            {
                return Task.FromResult(false);
            }

            string normalizedRole = arg.Role.ToLowerInvariant();
            return Task.FromResult(Array.Exists(s_validRoleValues, r => r == normalizedRole));
        }
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Role must be either 'editor' or 'viewer'";
    }
}
