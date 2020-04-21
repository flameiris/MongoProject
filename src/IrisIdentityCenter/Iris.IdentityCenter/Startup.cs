using Iris.Identity;
using Iris.MongoDB.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Env { get; }
        public ServiceProvider Provider { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //添加IdentityServer
            services.AddIdentityServer()
            //添加证书加密方式，执行该方法，会先判断tempkey.rsa证书文件是否存在，如果不存在的话，就创建一个新的tempkey.rsa证书文件，如果存在的话，就使用此证书文件。
            .AddDeveloperSigningCredential()
            //自定义用户验证（前端、后端用户等）
            .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
            //自定义Client配置
            .AddClientStore<ClientStore>()
            //资源站点(UserApi、OrderApi等)
            .AddInMemoryApiResources(ResourceStore.GetApiResources(Configuration))
            .AddProfileService<CustomProfileService>();

            services.AddMongoDB(Configuration, Env);
        }

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

    }

}
