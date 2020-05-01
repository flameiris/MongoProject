using Iris.FrameCore.ApiExtensions;
using Iris.FrameCore.ApiExtensions.Middleware;
using Iris.Service.IService;
using Iris.Service.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iris.AgentApi
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
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //添加基础组件依赖注入
            services.AddApiServiceCollection(Configuration, Env);

            //添加配置内容
            services.AddConfigure(Configuration);

            services.AddScoped(typeof(IAgentService), typeof(AgentService));
            services.AddScoped(typeof(IAuthService), typeof(AuthService));


            //IdentityServer
            services.AddAuthorization();
            services.AddAuthentication("Bearer")
            .AddIdentityServerAuthentication(options =>
            {
                //配置Identityserver的授权地址
                options.Authority = "http://localhost:10000";
                //不需要https    
                options.RequireHttpsMetadata = false;
                //api的name，需要和config的名称相同
                options.ApiName = "AgentApi";
            });


        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //app.UseSwagger();
                //app.UseSwaggerUI(c =>
                //{
                //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Iris.AgentApi");
                //    //DocExpansion设置为none可折叠所有方法
                //    c.DocExpansion(DocExpansion.None);
                //    //DefaultModelsExpandDepth设置为 - 1 可不显示models
                //    c.DefaultModelsExpandDepth(-1);
                //    //设置Model参数展示方式， Example value 或者 model scheme
                //    c.DefaultModelRendering(ModelRendering.Model);
                //    c.EnableValidator();
                //    c.EnableFilter();
                //    c.EnableDeepLinking();
                //    c.ShowExtensions();
                //});
            }


            app.UseExceptionHandling();

            //启用Authentication中间件
            app.UseAuthentication();

            // 配置跨域
            //app.UseCors("AllowSpecificOrigin");

            app.UseMvc();
        }
    }
}
