using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iris.MongoDB.Extensions
{
    public static class AddMongoDBServiceCollectionExtensions
    {
        /// <summary>
        /// 新增Mongodb依赖注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static void Add(this IServiceCollection services, IConfiguration configuration, IHostingEnvironment Env)
        {
            services.Configure<MongodbOptions>(configuration.GetSection("MongoConnection"));
            services.AddSingleton(typeof(IMongoDbManager<>), typeof(MongoDbManager<>));
        }
    }
}
