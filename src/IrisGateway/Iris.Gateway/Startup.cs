using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Iris.Gateway
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
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




            services.AddOcelot();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseOcelot();
        }
    }
}
