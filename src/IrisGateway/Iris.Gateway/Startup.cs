using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Iris.MongoDB;
using Iris.MongoDB.Extensions;
using Iris.Ocelot.Extensions;
using Iris.Ocelot.Extensions.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Iris.Gateway
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IHostingEnvironment Env { get; }

        public Startup(IHostingEnvironment env)
        {
            Env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(Env.IsProduction() ? "appsettings.json" : $"appsettings.{Env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }



        public void ConfigureServices(IServiceCollection services)
        {
            AddMongoDBServiceCollectionExtensions.AddMongoDB(services, Configuration, Env);

            var authenticationProviderKey = "GatewayKey";

            services
                .AddAuthentication()
                .AddIdentityServerAuthentication(authenticationProviderKey, options =>
                {
                    //是否启用https
                    options.RequireHttpsMetadata = false;
                    //配置授权认证的地址
                    options.Authority = $"http://localhost:10000";
                    //资源名称，跟认证服务中注册的资源列表名称中的apiResource一致
                    options.ApiName = "Gateway";
                    options.ApiSecret = "Gateway.Secret";
                    options.SupportedTokens = SupportedTokens.Both;
                });




            services.AddOcelot()
                .AddMongoOcelot();
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseCustomOcelot().Wait();
        }
    }
}
