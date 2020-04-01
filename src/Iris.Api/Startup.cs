using AutoMapper;
using Iris.FrameCore;
using Iris.FrameCore.Extensions;
using Iris.Service.IService;
using Iris.Service.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Iris.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
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
            services.Add(Configuration, Env);

            //添加配置内容
            services.AddConfigure(Configuration);

            services.AddScoped(typeof(IUserService), typeof(UserService));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Iris.Api");
                    //DocExpansion设置为none可折叠所有方法
                    c.DocExpansion(DocExpansion.None);
                    //DefaultModelsExpandDepth设置为 - 1 可不显示models
                    c.DefaultModelsExpandDepth(-1);
                    //设置Model参数展示方式， Example value 或者 model scheme
                    c.DefaultModelRendering(ModelRendering.Model);
                    c.EnableValidator();
                    c.EnableFilter();
                    c.EnableDeepLinking();
                    c.ShowExtensions();
                });
            }


            app.UseExceptionHandling();

            // 配置跨域
            //app.UseCors("AllowSpecificOrigin");

            app.UseMvc();
        }
    }
}
