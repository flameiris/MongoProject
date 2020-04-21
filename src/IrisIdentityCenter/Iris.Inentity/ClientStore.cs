using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.Identity
{
    public class ClientStore : IClientStore
    {
        public IConfiguration Configuration { get; }
        public ClientStore(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var clients = Configuration.GetSection("ClientList").Get<List<Identity.Models.Identity.Client>>()
                           .Select(x => new Client
                           {
                               ClientId = x.ClientId,
                               //客户端加密方式
                               ClientSecrets = new[] { new Secret(x.ClientSecrets) },
                               //配置授权类型，可以配置多个授权类型
                               AllowedGrantTypes = (ICollection<string>)typeof(GrantTypes).GetProperty(x.AllowedGrantTypes).GetValue(null, null),
                               //配置授权范围，这里指定哪些API 受此方式保护
                               AllowedScopes = x.AllowedScopes,
                               //配置Token 失效时间
                               AccessTokenLifetime = x.AccessTokenLifetime
                           });

            return clients.FirstOrDefault(x => x.ClientId == clientId);
        }
    }
}
