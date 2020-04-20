using IdentityServer4.AspNetIdentity;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Iris.IdentityCenter
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //添加IdentityServer
            services.AddIdentityServer()
            //添加证书加密方式，执行该方法，会先判断tempkey.rsa证书文件是否存在，如果不存在的话，就创建一个新的tempkey.rsa证书文件，如果存在的话，就使用此证书文件。
            .AddDeveloperSigningCredential()
            //用户（前端、后端用户等）
            .AddTestUsers(GetUsers().ToList())
            //自定义用户验证（前端、后端用户等）
            //.AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
            //客户端（App、H5、Web等）
            .AddInMemoryClients(GetClients())
            //自定义Client配置
            //.AddClientStore<ClientStore>()
            //资源站点(UserApi、OrderApi等)
            .AddInMemoryApiResources(GetApiResources());

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //IdentityServer4 中间件添加到Http管道中
            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }



        /// <summary>
        /// 资源站点(UserApi、OrderApi等)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource("UserApi", "UserApi DisplayName")
            };
        }

        /// <summary>
        /// Define which Apps will use thie IdentityServer
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "Client.App",
                    //客户端加密方式
                    ClientSecrets = new [] { new Secret("AppSecret".Sha256()) },
                    //配置授权类型，可以配置多个授权类型
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    //配置授权范围，这里指定哪些API 受此方式保护
                    AllowedScopes = new [] { "UserApi" },
                    //配置Token 失效时间
                    AccessTokenLifetime = 3600
                },
                new Client
                {
                    ClientId = "Client.H5",
                    //客户端加密方式
                    ClientSecrets = new [] { new Secret("H5Secret".Sha256()) },
                    //配置授权类型，可以配置多个授权类型
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    //配置授权范围，这里指定哪些API 受此方式保护
                    AllowedScopes = new [] { "UserApi" },
                    //配置Token 失效时间
                    AccessTokenLifetime = 3600
                }
            };
        }

        /// <summary>
        /// Define which uses will use this IdentityServer
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TestUser> GetUsers()
        {
            return new[]
            {
                new TestUser
                {
                    SubjectId = "10001",
                    Username = "FlameIris",
                    Password = "yq@123"
                }
            };
        }
    }






    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var userName = context.UserName;
                var password = context.Password;

                //验证用户,这么可以到数据库里面验证用户名和密码是否正确
                var claimList = await ValidateUserAsync(userName, password);

                // 验证账号
                context.Result = new GrantValidationResult
                (
                    subject: userName,
                    authenticationMethod: "custom",
                    claims: claimList.ToArray()
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

        #region Private Method
        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<List<Claim>> ValidateUserAsync(string loginName, string password)
        {
            //TODO 这里可以通过用户名和密码到数据库中去验证是否存在，
            // 以及角色相关信息，我这里还是使用内存中已经存在的用户和密码
            var user = new TestUser
            {
                SubjectId = "10001",
                Username = "FlameIris",
                Password = "yq@123"
            };

            if (user == null)
                throw new Exception("登录失败，用户名和密码不正确");

            return new List<Claim>()
            {
                new Claim(ClaimTypes.Name, $"{loginName}"),
            };
        }
        #endregion
    }





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
