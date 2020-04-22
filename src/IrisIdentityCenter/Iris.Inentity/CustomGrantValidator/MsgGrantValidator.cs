using IdentityServer4.Validation;
using System.Threading.Tasks;

namespace Iris.Identity.CustomGrantValidator
{
    public class MsgGrantValidator : IExtensionGrantValidator
    {
        public string GrantType => "msg_grant";

        public Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            return Task.CompletedTask;
        }
    }
}
