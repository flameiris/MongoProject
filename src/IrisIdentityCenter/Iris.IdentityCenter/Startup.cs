using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Iris.Identity;
using Iris.MongoDB.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Iris.IdentityCenter
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(Env.IsProduction() ? "appsettings.json" : $"appsettings.{Env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            System.Console.WriteLine(Env.IsProduction() ? "appsettings.json" : $"appsettings.{Env.EnvironmentName}.json");
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Env { get; }
        public ServiceProvider Provider { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //services.Configure<List<ApiResource>>(Configuration.GetSection("ApiResources"));



            //添加IdentityServer
            services.AddIdentityServer()
            //添加证书加密方式，执行该方法，会先判断tempkey.rsa证书文件是否存在，如果不存在的话，就创建一个新的tempkey.rsa证书文件，如果存在的话，就使用此证书文件。
            .AddDeveloperSigningCredential()
            //用户（前端、后端用户等）
            //.AddTestUsers(GetUsers().ToList())
            //自定义用户验证（前端、后端用户等）
            .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
            //客户端（App、H5、Web等）
            .AddInMemoryClients(GetClients())
            //自定义Client配置
            //.AddClientStore<ClientStore>()
            //资源站点(UserApi、OrderApi等)
            .AddInMemoryApiResources(GetApiResources())
            .AddProfileService<CustomProfileService>();

            services.AddMongoDB(Configuration, Env);



            //services.Configure<List<Client>>(Configuration.GetSection("AppSettings"));
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
        public IEnumerable<ApiResource> GetApiResources()
        {
            var str = Configuration["ApiResources:0"];
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<ApiResource>>(str);
        }

        /// <summary>
        /// Define which Apps will use thie IdentityServer
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Client> GetClients()
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

}
