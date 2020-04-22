using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Iris.Identity.CustomConfig
{
    public class ResourceStore
    {
        /// <summary>
        /// 资源站点(UserApi、OrderApi等)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources(IConfiguration configuration) =>
            configuration.GetSection("ApiResourceList").Get<List<Models.Identity.ApiResource>>()
            .Select(x => new ApiResource(x.Name, x.DisplayName));
    }
}
