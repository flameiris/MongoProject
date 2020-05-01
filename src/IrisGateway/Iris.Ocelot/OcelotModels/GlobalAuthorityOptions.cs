using System.Collections.Generic;

namespace Iris.Ocelot.OcelotModels
{
    public class GlobalAuthorityOptions
    {
        public OcelotAuthorityOptions AuthorityOptions { get; set; }

        public List<OcelotIdentityServerOptions> IdentityServerOptions { get; set; } = new List<OcelotIdentityServerOptions>();
    }
}