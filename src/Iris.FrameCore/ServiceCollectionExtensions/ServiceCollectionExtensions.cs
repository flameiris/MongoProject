using Iris.FrameCore.MongoDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Iris.FrameCore
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 新增Mongodb依赖注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongodbOptions>(options =>
            {
                options.IP = configuration.GetSection("MongoConnection:IP").Value;
                options.Port = configuration.GetValue<int>("MongoConnection:Port");
                options.Database = configuration.GetSection("MongoConnection:Database").Value;
                options.TimeOutSeconds = configuration.GetValue<int>("MongoConnection:TimeOutSeconds");
                options.Username = configuration.GetSection("MongoConnection:Username").Value;
                options.Password = configuration.GetSection("MongoConnection:Password").Value;
            });


            services.AddSingleton(typeof(IMongoDbManager<>), typeof(MongoDbManager<>));
            return services;
        }
    }
}
