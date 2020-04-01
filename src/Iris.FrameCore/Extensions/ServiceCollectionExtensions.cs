using Iris.FrameCore.MongoDb;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Iris.FrameCore.Filters;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Iris.FrameCore.Resolvers;
using CSRedis;
using System.Globalization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Iris.FrameCore.RabbitMQ;

namespace Iris.FrameCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 新增基础组件的依赖注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static void Add(this IServiceCollection services, IConfiguration configuration, IHostingEnvironment Env)
        {
            AddMvc(services);

            AddLogging(services, configuration, Env);

            AddMongoDb(services, configuration);

            AddRedis(services, configuration);

            AddRabbitMQ(services, configuration);

            AddAutoMapper(services);

            AddHttpClient(services, configuration);

            //AddCors(services, configuration);
        }

        /// <summary>
        /// 新增MVC、Api版本控制
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static void AddMvc(IServiceCollection services)
        {
            services.AddMvc(mvcOptions =>
            {
                //添加全局过滤器， order 数字越小的越先执行

                //跨域
                mvcOptions.Filters.Add(new CorsAuthorizationFilterFactory("AllowSpecificOrigin"));

                //接口参数校验
                mvcOptions.Filters.Add(typeof(ParameterCheckAttribute));

                // 结果过滤器
                mvcOptions.Filters.Add(typeof(ResponseResultAttribute));

                //是否使用重点路由，不使用
                mvcOptions.EnableEndpointRouting = false;
            })
            //返回json格式化
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new NullToEmptyStringResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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


        /// <summary>
        /// 新增Mongodb依赖注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static void AddMongoDb(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongodbOptions>(configuration.GetSection("MongoConnection"));
            services.AddSingleton(typeof(IMongoDbManager<>), typeof(MongoDbManager<>));
        }


        /// <summary>
        /// 新增Redis依赖注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static void AddRedis(IServiceCollection services, IConfiguration configuration)
        {
            var csredis = new CSRedisClient(null, configuration.GetValue<string>("ConnectionStrings:RedisConnectionString"));
            RedisHelper.Initialization(csredis);
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("zh-CN");
            services.AddSingleton(typeof(IDistributedCache), new CSRedisCache(RedisHelper.Instance));
        }


        /// <summary>
        /// 新增RabbitMQ依赖注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static void AddRabbitMQ(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMqConfig"));
            services.AddSingleton(typeof(RabbitMqClient));
        }


        /// <summary>
        /// 新增AutoMapper依赖注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static void AddAutoMapper(IServiceCollection services)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps("Iris.Models");

                //cfg.CreateMap(typeof(User), typeof(UserForDetailDto));

            });
            configuration.AssertConfigurationIsValid();
            var mapper = configuration.CreateMapper();

            services.AddSingleton(typeof(IMapper), mapper);

        }


        /// <summary>
        /// 新增 HttpClient 依赖注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void AddHttpClient(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddHttpClient("UploadImg", x =>
            //{
            //    x.BaseAddress = new Uri(configuration.GetValue<string>("SiteBaseConfig:UploadUrl"));
            //});
            //.AddHttpMessageHandler(() => new ApiRetryHandler(3));
        }


        /// <summary>
        /// 新增跨域依赖注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void AddCors(IServiceCollection services, IConfiguration configuration)
        {
            var allowOriginArr = configuration.GetSection("AllowSpecificOrigin").Get<string[]>();
            var AllowAnyOrigin = configuration.GetSection("AllowAnyOrigin").Get<string>();
            if (AllowAnyOrigin == "True")
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("AllowSpecificOrigin",
                        builder => builder
                        .WithOrigins(allowOriginArr)
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        );
                });
            }
            else
            {
                services.AddCors(options =>
                {

                    options.AddPolicy("AllowSpecificOrigin",
                        builder => builder
                        .WithOrigins(allowOriginArr)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        );
                });
            }
        }
    }
}
