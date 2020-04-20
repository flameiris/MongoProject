using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.Identity
{
    public class ClientStore : IClientStore
    {
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            #region 用户名密码
            var memoryClients = new List<Client>();
            if (memoryClients.Any(oo => oo.ClientId == clientId))
            {
                return memoryClients.FirstOrDefault(oo => oo.ClientId == clientId);
            }
            #endregion

            #region 通过数据库查询Client 信息
            return GetClient(clientId);
            #endregion
        }

        private Client GetClient(string client)
        {
            //TODO 根据数据库查询
            return null;
        }
    }
}
