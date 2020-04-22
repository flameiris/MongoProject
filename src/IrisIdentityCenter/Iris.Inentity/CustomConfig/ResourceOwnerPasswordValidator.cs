using IdentityServer4.Validation;
using Iris.Identity.Service;
using Iris.Models.Common;
using Iris.Models.Dto;
using Iris.Models.Enums;
using Iris.Models.Request.AgentPart;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Iris.Identity.CustomConfig
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly ILogger _logger;
        private readonly IAgentService _agentService;
        public ResourceOwnerPasswordValidator(
            ILogger<ResourceOwnerPasswordValidator> logger,
            IAgentService agentService
            )
        {
            _logger = logger;
            _agentService = agentService;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var userName = context.UserName;
                var password = context.Password;

                //判断账户密码是否正确
                var res = await _agentService.GetAgentByNameAndPwd(new AgentForIdCenterRequest
                {
                    Name = context.UserName,
                    Password = context.Password
                });
                if (res.Code != BusinessStatusType.OK)
                {

                }
                dynamic dto = res.Response;
                string id = dto.Id;
                //获取用户详细数据、角色、权限

                //设置缓存，失效时间等于Token失效时间

                //设置用户Claim，值为缓存键
                Claim userinfo = new Claim("Userinfo", JsonConvert.SerializeObject(dto));




                context.Result = new GrantValidationResult
                (
                    subject: id,
                    authenticationMethod: "custom",
                    claims: new List<Claim> { userinfo }
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
