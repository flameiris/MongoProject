using IdentityServer4.Validation;
using Iris.Models.Model.UserPart;
using Iris.MongoDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Iris.Identity
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger _logger;
        public ResourceOwnerPasswordValidator(
            IServiceProvider provider,
            ILogger<ResourceOwnerPasswordValidator> logger
            )
        {
            _provider = provider;
            _logger = logger;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var userName = context.UserName;
                var password = context.Password;

                //判断账户密码是否正确
                var _userMongo = _provider.GetService<IMongoDbManager<User>>();
                var user = await _userMongo.GetFirstOrDefaultAsync(x => x.Username == userName && x.Password == password);
                if (user == null) throw new Exception();

                //获取用户详细数据、角色、权限

                //设置缓存，失效时间等于Token失效时间

                //设置用户Claim，值为缓存键
                Claim userinfo = new Claim("Userinfo", "");




                context.Result = new GrantValidationResult
                (
                    subject: user.Id,
                    authenticationMethod: "custom",
                    claims: new List<Claim> { userinfo }.ToArray()
                 );
            }
            catch (Exception ex)
            {
                //验证异常结果
                context.Result = new GrantValidationResult()
                {
                    IsError = true,
                    Error = ex.Message
                };
            }
        }
    }
}
