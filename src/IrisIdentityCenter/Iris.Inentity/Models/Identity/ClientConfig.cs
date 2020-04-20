using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Iris.Identity.Models
{
    public class IdentitConfig
    {
        public List<Client> Clients { get; set; }
        public List<ApiResource> ApiResources { get; set; }

    }
}
