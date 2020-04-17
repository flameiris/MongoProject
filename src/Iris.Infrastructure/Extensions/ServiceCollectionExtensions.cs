using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;

namespace Iris.Infrastructure.Extensions
{
    public static class AddLoggingServiceCollectionExtensions
    {
        /// <summary>
        /// 新增基础组件的依赖注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static void Add(this IServiceCollection services, IConfiguration configuration, IHostingEnvironment Env)
        {

            AddLogging(services, configuration, Env);

        }


        /// <summary>
        /// 添加日志依赖注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static void AddLogging(IServiceCollection services, IConfiguration configuration, IHostingEnvironment Env)
        {
            services.AddLogging(configure =>
            {
                configure.AddNLog(Env.IsProduction() ? "NLog.config" : $"NLog.{Env.EnvironmentName}.config");
            });
        }
    }
}
