using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Iris.Identity.CustomConfig
{
    public class CustomProfileService : IProfileService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger _logger;
        public CustomProfileService(
            IServiceProvider provider,
            ILogger<CustomProfileService> logger
            )
        {
            _provider = provider;
            _logger = logger;
        }

        /// <summary>
        /// 只要有关用户的身份信息单元被请求（例如在令牌创建期间或通过用户信息终点），就会调用此方法
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //获取当前用户Claims
            var claims = context.Subject.Claims;
            var userinfo = claims.FirstOrDefault(x => x.Type == "Userinfo");
            if (userinfo == null)
                throw new Exception();

            //仅将当前用户自定义Claim添加到 请求资源时 可以获取到的 Claim
            context.AddRequestedClaims(new List<Claim> { userinfo });
            return Task.CompletedTask;
        }

        /// <summary>
        /// 验证用户是否有效 例如：token创建或者验证
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}