using IdentityServer4.Models;
using System.Collections.Generic;

namespace Iris.Identity.Models.Identity
{
    public class Client
    {
        public string ClientId { get; set; }
        /// <summary>
        /// 客户端加密方式
        /// </summary>
        private string _clientSecrets;
        public string ClientSecrets
        {
            get { return _clientSecrets; }
            set { _clientSecrets = value.Sha256(); }
        }
        private string _allowedGrantTypes;
        public string AllowedGrantTypes
        {
            get { return _allowedGrantTypes; }
            set { _allowedGrantTypes = value; }
        }

        /// <summary>
        /// 配置授权范围，这里指定哪些API 受此方式保护
        /// </summary>
        public List<string> AllowedScopes { get; set; }
        /// <summary>
        /// //配置Token 失效时间
        /// </summary>
        public int AccessTokenLifetime { get; set; }
    }
}
