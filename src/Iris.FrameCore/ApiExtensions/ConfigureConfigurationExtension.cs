using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iris.FrameCore.ApiExtensions
{
    public static class ConfigureConfigurationExtension
    {
        /// <summary>
        /// 添加配置内容
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddConfigure(this IServiceCollection services, IConfiguration configuration)
        {

            return services;
        }

    }
}
